using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Weekend.ViewModel;
using Weekend.Resources;
using FstnAnimation.Dynamic;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using FstnUserControl;
using FstnCommon;
using System.Diagnostics;
using FstnAnimation;
using Microsoft.Phone.Tasks;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
namespace Weekend
{
    public partial class MainPage : PhoneApplicationPage
    {

        // public DateTime endOfWeek { get; set; }
        // Constructor
        private MainViewModel MainViewModel;
        public MainPage()
        {
            InitializeComponent();
            MainViewModel = (App.Current.Resources["Locator"] as ViewModelLocator).MainViewModel;
            MainViewModel.Changed += MainViewModel_Changed; 
            this.Loaded += MainPage_Loaded;
       
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            coinFlip.Completed += coinFlip_Completed;
            coinFlip.TimeCounterTextBox.Changed += MainViewModel_Changed;
            coinFlip.TimeCounterTextBox.EventDate = MainViewModel.EventDate;
            coinFlip.TimeCounterTextBox.StartCount();
            ApplicationBar = new ApplicationBar();
            String theme = ThemeManager.Instance.Theme;
            Background.Source=new BitmapImage(new Uri("images/"+theme+"/Background.png",UriKind.Relative));
            ApplicationBar.BackgroundColor = (Color)App.Current.Resources["PhoneAccentColor"];
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/images/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/images/" + theme + "/appbar.lock.png", Msg.Lock, AskToLock);
            
        }

        private async void AskToLock(object sender, EventArgs e)
        {
            var op = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
        }

        private void AskToShare(object sender, EventArgs e)
        {
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(LayoutRoot).GetPath();
            task.Show();
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
            InitialLoad();
        }

        public void Reload()
        {
            if(coinFlip.Toss==IsWeekend()){
                InitialLoad();
            }
        }

        public void InitialLoad(){
            coinFlip.Image1Source = "/images/chrono.png";
            coinFlip.Image2Source = "/images/sun.png";
            if (IsWeekend())
            {
                coinFlip.Toss = false;
                resultText.Text = Msg.Enjoy;
            }
            else
            {
                coinFlip.Toss = true;
                resultText.Text = Msg.BePatient;
            }
            coinFlip.Start();

        }

        void coinFlip_Completed(object sender, EventArgs e)
        {

            resultText.Visibility = Visibility.Visible;

            var tile = ShellTile.ActiveTiles.First();
            var tileData = new FlipTileData()
            {
                Count = IsWeekend()?1:0,
                Title=coinFlip.TimeCounterTextBox.Text.Text.ToString(),
                WideBackContent = resultText.Text.ToString()
            };

            tile.Update(tileData);
        }

        private void Pivot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (Pivot.SelectedIndex == 1)
            {
                AnimationFactory.Instance.Open("MainPage");
            }
            if (Pivot.SelectedIndex == 0)
            {
                AnimationFactory.Instance.Close("MainPage");
            }

        }

        private void daysOfweekT_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ((ListPicker)daysOfweekCT.Element).Open();
        }

        private void endOfWeekT_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           ((TimePicker)endOfWeekCT.Element).OpenPickerPage();
        }

        private void weekendDurationT_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ((ListPicker)weekendDurationCT.Element).Open();
        }

    }
}