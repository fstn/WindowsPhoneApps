using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using FstnCommon.Util.Settings;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using MarketRoulet.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MarketRoulet
{
    public partial class Settings : PhoneApplicationPage
    {
        private String theme;
        public Settings()
        {
            InitializeComponent();
            this.Loaded += Settings_Loaded;
           
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            Double minRateValue = SettingsService.Instance.Value<Double>(SettingsKeys.MinRateValue);
            RatingControl.Value = minRateValue/2;
            int minCounts = SettingsService.Instance.Value<int>(SettingsKeys.MinCounts);
            if(minCounts>0 && minCounts<int.MaxValue)
                ListOfCounts.SelectedItem = minCounts;

            theme = ThemeManager.Instance.Theme;

            ApplicationBar = new ApplicationBar();
            ApplicationBar.BackgroundColor = (Color)App.Current.Resources["PhoneAccentColor"];
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.save.png", Msg.Save, AskToSave);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.cancel.png", Msg.Cancel, AskToCancel);

        }

        private void AskToCancel(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AskToSave(object sender, EventArgs e)
        {
            SettingsService.Instance.Save(SettingsKeys.MinRateValue, RatingControl.Value * 2);
            SettingsService.Instance.Save(SettingsKeys.MinCounts, ListOfCounts.SelectedItem);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}