﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace DAF.Core
{
    public class ImageHelper
    {
        public static Image ResizeImage(Image originalImage, int width, int height, ResizeMode mode, float scale)
        {
            if (width == 0 || height == 0)
                return originalImage;
            Size newSize = originalImage.Size.Resize(width, height, mode, scale);
            float x = 0;
            float y = 0;
            float w = originalImage.Width;
            float h = originalImage.Height;
            if (mode == ResizeMode.Crop)
            {
                x = (originalImage.Width - newSize.Width) / 2;
                y = (originalImage.Height - newSize.Height) / 2;
                w = newSize.Width;
                h = newSize.Height;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(newSize.Width, newSize.Height);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new RectangleF(0, 0, newSize.Width, newSize.Height),
                new RectangleF(x, y, w, h),
                GraphicsUnit.Pixel);

            g.Dispose();
            return bitmap;
        }

        public static Image Crop(Image originalImage, RectangleF rect)
        {
            Bitmap bmpImage = new Bitmap(originalImage);
            Bitmap bmpCrop = bmpImage.Clone(rect, bmpImage.PixelFormat);
            originalImage.Dispose();
            return (Image)(bmpCrop);
        }

        public static Image Resize(Image originalImage, Size size)
        {
            int sourceWidth = originalImage.Width;
            int sourceHeight = originalImage.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(originalImage, 0, 0, destWidth, destHeight);
            g.Dispose();
            originalImage.Dispose();

            return (Image)b;
        }

        public static void WaterMark(Image image, Image watermarkImage, Point offset, ContentAlignment imagePosition)
        {
            using (Graphics newGp = Graphics.FromImage(image))
            {
                newGp.CompositingQuality = CompositingQuality.HighQuality;

                //设置高质量插值法
                newGp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                //设置高质量,低速度呈现平滑程度
                newGp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                Rectangle newRect = rect.Resize(offset, new Size(watermarkImage.Width, watermarkImage.Height), imagePosition);

                newGp.DrawImage(watermarkImage, newRect);
            }
        }

        public static void WriteStringImage(Image image, string waterStr, Font font, Brush brush, Point p)
        {
            using (Graphics newGp = Graphics.FromImage(image))
            {
                Int32 stringWidth;
                Int32 stringHeight;
                stringHeight = (int)font.Size;
                stringWidth = (int)((font.Size + 1));

                newGp.CompositingQuality = CompositingQuality.HighQuality;

                //设置高质量插值法
                newGp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                //设置高质量,低速度呈现平滑程度
                newGp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //文字抗锯齿
                newGp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                newGp.DrawString(waterStr, font, brush, p);
            }
        }
    }

}
