using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Photo_You.Resources;

namespace Photo_You
{
    public partial class Help : PhoneApplicationPage
    {
        public Help()
        {
            InitializeComponent();
            InitAppBar();
        }
        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateIcon(ApplicationBar, "/Assets/Images/" + theme + "/appbar.back.png", Msg.Back, AskToBack);
        }

        private void AskToBack(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}