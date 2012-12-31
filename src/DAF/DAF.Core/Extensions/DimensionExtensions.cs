using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DAF.Core
{
    public static class DimensionExtension
    {
        public static Size Resize(this Size size, int width, int height, ResizeMode mode, float scale)
        {
            SizeF sizef = new SizeF(width, height);
            SizeF newSizef = Resize(sizef, width, height, mode, scale);
            return new Size((int)newSizef.Width, (int)newSizef.Height);
        }

        public static SizeF Resize(this SizeF size, float width, float height, ResizeMode mode, float scale)
        {
            SizeF newSize = new SizeF(width, height);
            switch (mode)
            {
                case ResizeMode.FixedWidthAndHeight:
                    break;
                case ResizeMode.FixedWidth:
                    newSize.Height = size.Height * width / size.Width;
                    break;
                case ResizeMode.FixedHeight:
                    newSize.Width = size.Width * height / size.Height;
                    break;
                case ResizeMode.Crop:
                    if (width > size.Width)
                        newSize.Width = size.Width;
                    if (height > size.Height)
                        newSize.Height = size.Height;
                    break;
                case ResizeMode.Scale:
                    if (scale < 0)
                    {
                        newSize.Width = -1 * size.Width / scale;
                        newSize.Height = -1 * size.Height / scale;
                    }
                    else if (scale > 0)
                    {
                        newSize.Width = size.Width * scale;
                        newSize.Height = size.Height * scale;
                    }
                    break;
                case ResizeMode.MaxWidthOrHeight:
                    if (size.Width > width || size.Height > height)
                    {
                        if (size.Width / size.Height > width / height)
                        {
                            newSize.Height = size.Height * width / size.Width;
                        }
                        else
                        {
                            newSize.Width = size.Width * height / size.Height;
                        }
                    }
                    else
                    {
                        newSize.Width = size.Width;
                        newSize.Height = size.Height;
                    }
                    break;
                default:
                    break;
            }
            return newSize;
        }

        public static Rectangle Resize(this Rectangle rect, Point offset, Size size, ContentAlignment alignment)
        {
            RectangleF rectf = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            SizeF sizef = new SizeF(size.Width, size.Height);
            RectangleF newRectf = Resize(rectf, offset, sizef, alignment);
            return new Rectangle((int)newRectf.X, (int)newRectf.Y, (int)newRectf.Width, (int)newRectf.Height);
        }

        public static RectangleF Resize(this RectangleF rect, PointF offset, SizeF size, ContentAlignment alignment)
        {
            RectangleF newRect = new RectangleF(rect.X, rect.Y, size.Width, size.Height);
            switch (alignment)
            {
                case ContentAlignment.BottomLeft:
                    newRect.Y = rect.Height - size.Height - offset.Y;
                    break;
                case ContentAlignment.BottomRight:
                    newRect.Y = rect.Height - size.Height - offset.Y;
                    newRect.X = rect.Width - size.Width - offset.X;
                    break;
                case ContentAlignment.BottomCenter:
                    newRect.Y = rect.Height - size.Height - offset.Y;
                    newRect.X = (rect.Width - size.Width - offset.X) / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    newRect.Y = (rect.Height - size.Height - offset.Y) / 2;
                    newRect.X = rect.Width - size.Width - offset.X;
                    break;
                case ContentAlignment.MiddleCenter:
                    newRect.Y = (rect.Height - size.Height - offset.Y) / 2;
                    newRect.X = (rect.Width - size.Width - offset.X) / 2;
                    break;
                case ContentAlignment.MiddleLeft:
                    newRect.Y = (rect.Height - size.Height - offset.Y) / 2;
                    break;
                case ContentAlignment.TopRight:
                    newRect.X = rect.Width - size.Width - offset.X;
                    break;
                case ContentAlignment.TopCenter:
                    newRect.X = (rect.Width - size.Width - offset.X) / 2;
                    break;
                case ContentAlignment.TopLeft:
                default:
                    break;
            }

            return newRect;
        }
    }

    public enum ResizeMode
    {
        FixedWidth,
        FixedHeight,
        FixedWidthAndHeight,
        MaxWidthOrHeight,
        Crop,
        Scale,
    }
}
