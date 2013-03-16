using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;

namespace FstnCommon.Util
{
    public class ScreenShot
    {
        public static String TakeInIsolatedSotrage(UIElement elt, String name)
        {
            string imageFolder = "Shared/Shellcontent/";
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.DirectoryExists(imageFolder))
                {
                    myIsolatedStorage.CreateDirectory(imageFolder);
                }

                if (myIsolatedStorage.FileExists(name))
                {
                    myIsolatedStorage.DeleteFile(name);
                }

                string filePath = System.IO.Path.Combine(imageFolder, name);
                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filePath);

                var bmp = new WriteableBitmap(elt, null);
                var width = (int)bmp.PixelWidth;
                var height = (int)bmp.PixelHeight;
                using (var ms = new MemoryStream(width * height * 4))
                {
                    bmp.SaveJpeg(ms, width, height, 0, 100);
                    ms.Seek(0, SeekOrigin.Begin);
                    var lib = new MediaLibrary();
                    Extensions.SaveJpeg(bmp, fileStream, width, height, 0, 80);
                }
                fileStream.Close();
                Debugger.Log(0, "", "isostore:/" + filePath);
                return  "isostore:/"+filePath; 
            }
        }
        public static Picture Take(UIElement elt,String name)
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
                picture = lib.SavePicture(string.Format(name), ms);
                
                return picture;
            }
        }
        public static Picture Take(UIElement elt)
        {
            return Take(elt, "test.jpeg");
        }
    }
}
