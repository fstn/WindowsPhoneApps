using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl.Error
{
    public partial class ErrorDisplayer : UserControl,IErrorDisplayer
    {
        public ErrorDisplayer()
        {
            InitializeComponent();
            this.Tap += ErrorDisplayer_Tap;
        }

        void ErrorDisplayer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Show(String text) {
            this.Visibility = Visibility.Visible;
            this.Text = text;
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
