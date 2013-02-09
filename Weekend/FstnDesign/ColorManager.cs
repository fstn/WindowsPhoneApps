using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FstnDesign
{
    public class ColorManager
    {
        private Brush lightAccent;
        private Brush lightLightAccent;
        public  Brush LightAccent
        {
            get
            {
                if (lightAccent == null)
                {
                    Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                    HslColor hslBase = HslColor.FromColor(accentColor);
                    lightAccent= new SolidColorBrush(hslBase.Lighten(.2).ToColor());
                }
                return lightAccent;
            }
            set { lightAccent = value; }
        }
        public Brush LightLightAccent
        {
            get
            {
                if (lightLightAccent == null)
                {
                    Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                    HslColor hslBase = HslColor.FromColor(accentColor);
                    lightLightAccent = new SolidColorBrush(hslBase.Lighten(.3).ToColor());
                }
                return lightLightAccent;
            }
            set { lightLightAccent = value; }
        }
	
    }
}
