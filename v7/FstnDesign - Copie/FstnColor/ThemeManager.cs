using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FstnDesign.FstnColor
{
    public class ThemeManager
    {
        private static ThemeManager instance = new ThemeManager();

        public static ThemeManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }
        private ThemeManager()
        {

        }
        public String Theme
        {
            get
            {
                String theme = "";
                if ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
                {
                    theme = "dark";
                }
                else
                {
                    theme = "light";
                }
                return theme;
            }
        }
    }
}
