using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;

namespace Share
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SpeechSynthesizer ss;
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