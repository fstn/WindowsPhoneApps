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

namespace AppsRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {
        
        private IsolatedStorageSettings settings=IsolatedStorageSettings.ApplicationSettings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
           
        }

        private bool isWeekend(){
            int minutes = ((DateTime)endOfWeek.Value).Minute;
            int hour = ((DateTime)endOfWeek.Value).Hour;

            return ((DateTime.Now.Hour >=hour && DateTime.Now.Minute >= minutes && DateTime.Now.DayOfWeek == DayOfWeek.Friday) || DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday);
        }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TimePicker_ValueChanged_1(object sender, DateTimeValueChangedEventArgs e)
        {
            settings.Clear();
            settings.Add("endTime",((TimePicker)sender).Value);
            settings.Save();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {

            if (settings.Contains("endTime") &&  settings["endTime"] != null)
            {
                endOfWeek.Value = ((DateTime)settings["endTime"]);
            }
            else
            {
                endOfWeek.Value = new DateTime(2012, 12, 12, 18, 0, 0);
            }

            //resultImage.SetValue(Image.SourceProperty, img);
           // resultText.Text = isWeekendResult;
            Uri nonReponseImg;
            Uri uriFinalReponseImg;
            Uri uriFinalScreenImg;
            if (isWeekend())
            {
                nonReponseImg = new Uri("/images/non.png", UriKind.Relative);
                uriFinalReponseImg = new Uri("/images/oui.png", UriKind.Relative);
                uriFinalScreenImg = new Uri("/images/sun.png", UriKind.Relative);
            }
            else
            {
                nonReponseImg = new Uri("/images/oui.png", UriKind.Relative);
                uriFinalReponseImg = new Uri("/images/non.png", UriKind.Relative);
                uriFinalScreenImg = new Uri("/images/broke.png", UriKind.Relative);
            }
            non.Source = new BitmapImage(nonReponseImg);
            finalResponseImg.Source = new BitmapImage(uriFinalReponseImg);
            finalScreenImg.Source = new BitmapImage(uriFinalScreenImg);
            StoryBoard.Begin();
        }
        StandardTileData tileData = new StandardTileData()
        {
            BackgroundImage = new Uri("/images/bad.png", UriKind.RelativeOrAbsolute),
            Title = "Mon application !",
            Count = 1,
            BackContent = "Info au dos...",
            BackTitle = "Mon appli"
        };

    }
}