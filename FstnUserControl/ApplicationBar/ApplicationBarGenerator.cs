using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Shell;

namespace FstnUserControl.ApplicationBar
{
    public class ApplicationBarGenerator
    {


        #region Singleton

        private static ApplicationBarGenerator _instance = null;

        private ApplicationBarGenerator() { }

        public static ApplicationBarGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    ApplicationBarGenerator instance = new ApplicationBarGenerator();
                    instance.Initialize();
                    _instance = instance;
                }
                return _instance;
            }
        }

        #endregion
        
        private void Initialize()
        {

        }

        public void CreateDouble(Microsoft.Phone.Shell.IApplicationBar ApplicationBar, string image, string text,EventHandler functionToCall)
        {
            var appBarButtonSave = new ApplicationBarIconButton(new Uri(image, UriKind.Relative))
            {
                Text = text
            };
            appBarButtonSave.Click += functionToCall;
            ApplicationBar.Buttons.Add(appBarButtonSave);

            var appBarMenuSave = new ApplicationBarMenuItem(text);
            appBarMenuSave.Click += functionToCall;
            ApplicationBar.MenuItems.Add(appBarMenuSave);
        }
        public void CreateIcon(Microsoft.Phone.Shell.IApplicationBar ApplicationBar, string image, string text, EventHandler functionToCall)
        {
            var appBarButtonSave = new ApplicationBarIconButton(new Uri(image, UriKind.Relative))
            {
                Text = text
            };
            appBarButtonSave.Click += functionToCall;
            ApplicationBar.Buttons.Add(appBarButtonSave);
        }
        public void CreateHidden(Microsoft.Phone.Shell.IApplicationBar ApplicationBar, string text, EventHandler functionToCall)
        {
            var appBarMenuSave = new ApplicationBarMenuItem(text);
            appBarMenuSave.Click += functionToCall;
            ApplicationBar.MenuItems.Add(appBarMenuSave);
        }
    }
}
