using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using DAF.Core.Caching;
using DAF.Core.Security;

namespace DAF.Web.Security
{
	public class DefaultCaptchaGenerator : ICaptchaGenerator
	{
		private ICacheProvider cache;

		public DefaultCaptchaGenerator(ICacheManager cacheManager)
		{
			this.cache = cacheManager.CreateCacheProvider(CacheScope.Global);
		}

		public bool Verify(string sessionId, string userInput)
		{
			if (this.cache != null)
			{
				string key = BuildKey(sessionId);
				string captcha = (this.cache[key] ?? "").ToString();
				bool result = captcha == userInput;
				this.cache.Remove(key);
				return result;
			}
			return false;
		}

		public Bitmap Generate(string sessionId, string randomText, int width, int height)
		{
			Random random = new Random();
			string fontName = "Arial";

			// Create instance of bitmap object
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			// Create instance of graphics object
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle rectangle = new Rectangle(0, 0, width, height);

			// Fill the background in a light gray pattern
			int hatchStyle = random.Next(0, 52);
			HatchBrush hatchBrush = new HatchBrush((HatchStyle)hatchStyle, Color.LightGray, Color.White);
			graphics.FillRectangle(hatchBrush, rectangle);

			// At this point need to figure out fontsize.
			SizeF sizeF;
			float fontSize = rectangle.Height + 1;
			Font font;
			do
			{
				// Adjust font size since it needs to fit on screen.
				fontSize--;
				font = new Font(fontName, fontSize, FontStyle.Bold);
				sizeF = graphics.MeasureString(randomText, font);
			} while (sizeF.Width > rectangle.Width + 30);

			// Format the text
			StringFormat objStringFormat = new StringFormat();
			objStringFormat.Alignment = StringAlignment.Center;
			objStringFormat.LineAlignment = StringAlignment.Center;

			// Create a path using the text and randomly warp it
			GraphicsPath objGraphicsPath = new GraphicsPath();
			objGraphicsPath.AddString(randomText, font.FontFamily, (int)font.Style, font.Size, rectangle, objStringFormat);
			float flV = 4F;

			// Create a parallelogram for the text to draw into
			PointF[] arrPoints =
			{
				new PointF(random.Next(rectangle.Width) / flV, random.Next(rectangle.Height) / flV),
				new PointF(rectangle.Width - random.Next(rectangle.Width) / flV, random.Next(rectangle.Height) / flV),
				new PointF(random.Next(rectangle.Width) / flV, rectangle.Height - random.Next(rectangle.Height) / flV),
				new PointF(rectangle.Width - random.Next(rectangle.Width) / flV, rectangle.Height - random.Next(rectangle.Height) / flV)
			};

			// Create the warped parallelogram for the text
			Matrix objMatrix = new Matrix();
			objMatrix.Translate(0F, 0F);
			objGraphicsPath.Warp(arrPoints, rectangle, objMatrix, WarpMode.Perspective, 0F);

			// Add text
			hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.DarkGray, Color.Black);
			graphics.FillPath(hatchBrush, objGraphicsPath);

			// Add noise
			int intMax = Math.Max(rectangle.Width, rectangle.Height);
			int total = (int)(rectangle.Width * rectangle.Height / 30F);

			for (int i = 0; i < total; i++)
			{
				int x = random.Next(rectangle.Width);
				int y = random.Next(rectangle.Height);
				int w = random.Next(intMax / 15);
				int h = random.Next(intMax / 70);
				graphics.FillEllipse(hatchBrush, x, y, w, h);
			}

			// Dispose the graphics objects.
			font.Dispose();
			hatchBrush.Dispose();
			graphics.Dispose();

			if (cache != null)
				cache.Add(BuildKey(sessionId), randomText, null, TimeSpan.FromMinutes(20), DateTime.MaxValue);

			return bitmap;
		}

		private string BuildKey(string sessionId)
		{
			return string.Format("UserCaptcha:{0}", sessionId);
		}

		public string Name
		{
			get { return "DefaultCaptchaGenerator"; }
		}
	}
}
