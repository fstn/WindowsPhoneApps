using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using BugSense;
using AppsRoulet.ViewModel;
using System.Diagnostics;
using AppsRoulet.Resources;

namespace AppsRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {

        // public DateTime endOfWeek { get; set; }
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            (App.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.Changed += MainViewModel_Changed;
            timeCounterTextBox.Changed += MainViewModel_Changed;
        }

        void MainViewModel_Changed(object sender, EventArgs e)
        {
            Reload();
        }

        private bool IsWeekend()
        {
            return (App.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.IsWeekend();
        }

        private void TimePicker_ValueChanged_1(object sender, DateTimeValueChangedEventArgs e)
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.SaveSettings();
        }


        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            Reload();
        }

        public void Reload()
        {
            timeCounterTextBox.Opacity = 100;
            canvas.Opacity = 100;
            canvas.RenderTransform.Transform(new Point(0, 0));
            Uri uriFinalScreenImg;
            if (IsWeekend())
            {
                timeCounterTextBox.Visibility = Visibility.Collapsed;
                non.ImageSource = "/images/non.png";
                non.Text = Msg.No;
                finalResponseImg.ImageSource = "/images/oui.png";
                finalResponseImg.Text = Msg.Yes;
                uriFinalScreenImg = new Uri("/images/sun.png", UriKind.Relative);
            }
            else
            {
                timeCounterTextBox.Visibility = Visibility.Visible;
                non.ImageSource = "/images/oui.png";
                non.Text = Msg.Yes;
                finalResponseImg.ImageSource = "/images/non.png";
                finalResponseImg.Text = Msg.No;
                uriFinalScreenImg = new Uri("/images/chrono.png", UriKind.Relative);
            }
            finalScreenImg.Source = new BitmapImage(uriFinalScreenImg);
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("MainPage.xaml?TileID=2"));
            //test if Tile was created
            if (TileToFind == null)
            {
                StandardTileData tileData = new StandardTileData()
                {
                    BackgroundImage = new Uri("/images/bad.png", UriKind.RelativeOrAbsolute),
                    Title = "Mon application !",
                    Count = 1,
                    BackContent = "Info au dos...",
                    BackTitle = "Mon appli"
                };
                finalResponseImg.Loaded += finalResponseImg_Loaded;
            }

            if (!IsWeekend())
            {
                timeCounterTextBox.StartCount();
            }
        }

        void finalResponseImg_Loaded(object sender, EventArgs e)
        {
            StoryBoard1.Begin();
        }

        private void daysOfweek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.SaveSettings();
        }

        private void StoryBoard_Completed(object sender, EventArgs e)
        {



        }

    }
}