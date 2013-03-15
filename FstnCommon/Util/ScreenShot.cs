using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;

namespace FstnCommon.Util
{
    public class ScreenShot
    {
        public static Picture Take(UIElement elt)
        {
            var bmp = new WriteableBitmap(elt, null);
            var width = (int)bmp.PixelWidth;
            var height = (int)bmp.PixelHeight;
            using (var ms = new MemoryStream(width * height * 4))
            {
                Picture picture;

                bmp.SaveJpeg(ms, width, height, 0, 100);
                ms.Seek(0, SeekOrigin.Begin);
                var lib = new MediaLibrary();
                picture = lib.SavePicture(string.Format("test.jpg"), ms);

                return picture;
            }
        }
    }
}
