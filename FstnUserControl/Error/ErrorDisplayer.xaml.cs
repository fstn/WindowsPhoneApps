using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using FstnCommon.Util;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl.Error
{
    public partial class ErrorDisplayer : UserControl,IErrorDisplayer
    {
        public DispatcherTimer dt;
        public ErrorDisplayer()
        {
            InitializeComponent();
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(5);
            dt.Tick += dt_Tick;
            this.Tap += ErrorDisplayer_Tap;
        }

        void dt_Tick(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            dt.Stop();
        }

        void ErrorDisplayer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Show(String text) {
            this.Visibility = Visibility.Visible;
            this.Text = text;
            dt.Start();
        }

        public String Text
        {
            set
            {
                ErrorText.Text = value;
            }
        }
    }
}
