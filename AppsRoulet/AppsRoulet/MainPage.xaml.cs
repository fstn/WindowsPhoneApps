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
            StoryBoard.Seek(new TimeSpan(0));
            StoryBoard.Resume();
            StoryBoard.GetCurrentTime();
            timeCounterTextBox.Opacity = 100;
            canvas.Opacity = 100;
            canvas.RenderTransform.Transform(new Point(0, 0));
            finalResponseImg.RenderTransform.Transform(new Point(0, 0));
            finalResponseImg.RenderTransform.Transform(new Point(0, 0));
            //resultImage.SetValue(Image.SourceProperty, img);
            // resultText.Text = isWeekendResult;
            Uri nonResponseImg;
            Uri uriFinalResponseImg;
            Uri uriFinalScreenImg;
            if (IsWeekend())
            {
                timeCounterTextBox.Visibility = Visibility.Collapsed;
                nonResponseImg = new Uri("/images/non.png", UriKind.Relative);
                uriFinalResponseImg = new Uri("/images/oui.png", UriKind.Relative);
                uriFinalScreenImg = new Uri("/images/sun.png", UriKind.Relative);
                ResultText.Text = "OUIIIII";
            }
            else
            {
                timeCounterTextBox.Visibility = Visibility.Visible;
                nonResponseImg = new Uri("/images/oui.png", UriKind.Relative);
                uriFinalResponseImg = new Uri("/images/non.png", UriKind.Relative);
                uriFinalScreenImg = new Uri("/images/chrono.png", UriKind.Relative);
                ResultText.Text = "NON";
            }
            non.Source = new BitmapImage(nonResponseImg);
            finalResponseImg.Source = new BitmapImage(uriFinalResponseImg);
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
                StoryBoard.Begin();
            }
        }

        private void daysOfweek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).MainViewModel.SaveSettings();
        }

        private void StoryBoard_Completed(object sender, EventArgs e)
        {
            if (!IsWeekend())
            {
                timeCounterTextBox.StartCount();
            }



        }

    }
}