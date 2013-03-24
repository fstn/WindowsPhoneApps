using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace FstnUserControl.Camera
{
    public class OrientedCameraImage
    {
        public BitmapImage Image { get; set; }
        public double Angle { get; set; }
        public PageOrientation Orientation { get; set; }
    }
}
