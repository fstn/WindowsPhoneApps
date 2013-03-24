using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace FstnCommon.Util
{
    public class ImageUtils
    {
        public static BitmapImage GetBitmapFromIsostore(String path)
        {
            // define data array to hold image data to be read from isolated storage
            byte[] imageBytes;
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(path, FileMode.Open, FileAccess.Read);
                                    // allocate array large enough to hold the whole file
                    imageBytes = new byte[fileStream.Length];

                    // read all data to memory
                    fileStream.Read(imageBytes, 0, imageBytes.Length);
                    fileStream.Close();
            }

            // create memory stream and bitmap
            MemoryStream memoryStream = new MemoryStream(imageBytes);
            BitmapImage bitmapImage = new BitmapImage();

            // memory stream is source of bitmap
            bitmapImage.SetSource(memoryStream);
            return bitmapImage;
        }
    }
}
