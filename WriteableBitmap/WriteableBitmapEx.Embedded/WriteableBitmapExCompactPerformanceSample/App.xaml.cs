using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WBEX.SWE.Performance
{
    public class App : Application
    {
#if WindowsCE
	private bool _contentLoaded;

        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Application.LoadComponent(this, new Uri("/WBEX.SWE.Performance;component/App.xaml", UriKind.Relative));
            }
        }

#endif

        public App()
        {
            InitializeComponent();

            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
        }
    }
}
