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
        private static ColorManager instance = new ColorManager();
        public static ColorManager Instance { get { return instance; } }
        private Brush lightAccent;
        private Brush lightLightAccent;
        private Random random;

        public ColorManager()
        {
            random = new Random(DateTime.Now.Millisecond);
        }

        public Brush RandomAccentBrush
        {
            get
            {
                Brush retour = null;
                int nb = random.Next(0, 2);
                if (nb == 0)
                {
                    retour = (Brush)Application.Current.Resources["PhoneAccentBrush"];
                }
                else if (nb == 1)
                {
                    retour = LightAccent;
                }
                else if (nb == 2)
                {
                    retour = LightLightAccent;
                }
                return retour;
            }

        }
        public Brush LightAccent
        {
            get
            {
                if (lightAccent == null)
                {
                    Color accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                    HslColor hslBase = HslColor.FromColor(accentColor);
                    lightAccent = new SolidColorBrush(hslBase.Lighten(.2).ToColor());
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
