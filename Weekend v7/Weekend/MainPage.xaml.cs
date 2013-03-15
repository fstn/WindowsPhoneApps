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
using FstnUserControl;
using FstnCommon;
using System.Diagnostics;
using FstnAnimation;
using Microsoft.Phone.Tasks;
using FstnCommon.Util;
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
            coinFlip.Completed += coinFlip_Completed;

            coinFlip.TimeCounterTextBox.Changed += MainViewModel_Changed;
            coinFlip.TimeCounterTextBox.EventDate = MainViewModel.EventDate;
            coinFlip.TimeCounterTextBox.StartCount();
            

        }

        void coinFlip_Completed(object sender, EventArgs e)
        {
            resultText.Visibility = Visibility.Visible;
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

        private void Button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(LayoutRoot).GetPath();
            task.Show();
        }


    }
}