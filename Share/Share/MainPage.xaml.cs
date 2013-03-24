

using System;
using System.Windows;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System.Xml.Linq;
using System.Linq;
using Share.Resources;
using System.Diagnostics;

namespace Share
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SpeechSynthesizer ss;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MicButton.Tap += MicButton_Tap;
            ss = new SpeechSynthesizer();
        }

      

        private async void MicButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SpeechRecognizerUI sr = new SpeechRecognizerUI();
            sr.Settings.ListenText = "Let's go to the beach";
            sr.Settings.ReadoutEnabled = false;
            sr.Settings.ShowConfirmation = true;
            SpeechRecognitionUIResult result = await sr.RecognizeWithUIAsync();
            if (result.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                ShareStatusTask task = new ShareStatusTask();
                task.Status = result.RecognitionResult.Text;
                task.Show();
            }
        }


    }
}