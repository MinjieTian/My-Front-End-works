using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace WebApplication.Helper
{
    public static class ImageHelper
    {
        public static Image Resize(Image image, Size newSize, out string imageType)
        {
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
                imageType = "JPEG";
            else if (image.RawFormat.Equals(ImageFormat.Png))
                imageType = "PNG";
            else
                imageType = "GIF";
            Bitmap b = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public static string ImageToString(Image image, ImageFormat imageFormat)
        {
            string photoString;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                byte[] photoBytes = ms.ToArray();
                photoString = Convert.ToBase64String(photoBytes);
            }
            return photoString;
        }
    }
}
