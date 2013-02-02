using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Library.Manager
{
    public class ColorManager
    {
        public static SolidColorBrush getDarkerShade(SolidColorBrush brush)
        {
            SolidColorBrush darkerShade = new SolidColorBrush();
            byte alpha = 255;
            byte red = (byte)(brush.Color.R - (brush.Color.R * 0.4));
            byte blue = (byte)(brush.Color.B - (brush.Color.B * 0.4));
            byte green = (byte)(brush.Color.G - (brush.Color.G * 0.4));
            darkerShade.Color = Color.FromArgb(alpha, red, green, blue);
            return darkerShade;
        }
        public static SolidColorBrush getLighterShade(SolidColorBrush brush)
        {
            SolidColorBrush lighterShade = new SolidColorBrush();
            byte alpha = 255;
            byte red = (byte)(brush.Color.R + (brush.Color.R * 0.1));
            byte blue = (byte)(brush.Color.B + (brush.Color.B * 0.1));
            byte green = (byte)(brush.Color.G + (brush.Color.G * 0.1));
            lighterShade.Color = Color.FromArgb(alpha, red, green, blue);
            return lighterShade;
        }
    }
}
