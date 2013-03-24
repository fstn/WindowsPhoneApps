using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnDesign
{
    public partial class Error : PhoneApplicationPage
    {
        public Error()
        {
            InitializeComponent();

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string error = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("error", out error))
            {
                ErrorText.Text = error;
            }
        }
    }
}