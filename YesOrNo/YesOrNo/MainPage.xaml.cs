using System;
using System.Linq;
using System.Windows;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using YesOrNo.Resources;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.Recognition;

namespace YesOrNo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SpeechSynthesizer ss;
        private ProgressIndicator indicator;
        // Constructor
        public MainPage()
        {
            InitAppBar();
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            indicator = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true
            };
            SystemTray.SetProgressIndicator(this, indicator);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            indicator.IsVisible = false;
            MicButton.Tap += MicButton_Tap;
            ss = new SpeechSynthesizer();
        }

        private async void Speak()
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            VoiceInformation vi = InstalledVoices.All.Where(v => v.Language == "en-EN" && v.Gender == VoiceGender.Male).FirstOrDefault();
            ss.SetVoice(vi);
            int rand=(int)RandomService.Instance.getRand(0, 3);
            string answer="";
            switch (rand)
            {
                case 0:
                    answer = "yes";
                    break;
                case 1:
                    answer = "no";
                    break;
                case 2:
                    answer = "maybe";
                    break;
                case 3:
                    answer = "I don't know";
                    break;

            }
            AnswerText.Text = answer;
            await ss.SpeakTextAsync(answer);
        }

        private async void MicButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            SpeechRecognizerUI sr = new SpeechRecognizerUI();
            sr.Settings.ListenText = "Will I be rich?";
            sr.Settings.ReadoutEnabled = false;
            sr.Settings.ShowConfirmation = true;
            SpeechRecognitionUIResult result = await sr.RecognizeWithUIAsync();
            if (result.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                string resultText = result.RecognitionResult.Text;
                resultText.Replace('.', '?');
                resultText=ConfidenceText.Text = resultText;
                Speak();
            }
        }


        private void AskToShare(object sender, EventArgs e)
        {
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(resultPanel).GetPath();
            task.Show();
        }

        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.share.png", Msg.Save, AskToShare);
        }
    }
}