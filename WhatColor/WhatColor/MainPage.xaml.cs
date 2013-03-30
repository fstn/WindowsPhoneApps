using System;
using System.Linq;
using System.Windows;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using WhatColor.Resources;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.Recognition;
using System.Windows.Media;
using System.Collections.Generic;

namespace WhatColor
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SpeechSynthesizer ss;
        private ProgressIndicator indicator;
        private SpeechRecognizerUI recoWithUI;
        private GameColor currentColor;
        private List<GameColor> gameColors;
        private bool busy = false;

        // Constructor
        public MainPage()
        {
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
            initListOfCOlors();
            doNext();

            Speak("What is button's background color?");
        }

        private void initListOfCOlors()
        {
            gameColors = new List<GameColor>();
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Blue),
                ColorName = "Blue"
            });
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Green),
                ColorName = "Green"
            });
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Yellow),
                ColorName = "Yelloh"
            });
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Orange),
                ColorName = "Orange"
            });
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Purple),
                ColorName = "Purple"
            });
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Red),
                ColorName = "Red"
            });
            gameColors.Add(new GameColor()
            {
                ColorBrush = new SolidColorBrush(Colors.Brown),
                ColorName = "Brown"
            });
        }

        private async void Speak(string result = "")
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            VoiceInformation vi = InstalledVoices.All.Where(v => v.Language == "en-EN" && v.Gender == VoiceGender.Male).FirstOrDefault();
            ss.SetVoice(vi);
            string answer = "great job!";
            if (result != "")
            {
                answer = result;
            }
            AnswerText.Text = answer;
            await ss.SpeakTextAsync(answer);
        }

        private async void MicButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!busy)
            {
                try
                {
                    busy = true;
                    // Initialize the SpeechRecognizerUI object.
                    recoWithUI = new SpeechRecognizerUI();

                    // Query for a recognizer that recognizes French as spoken in France.
                    IEnumerable<SpeechRecognizerInformation> recognizers = from recognizerInfo in InstalledSpeechRecognizers.All
                                                                           select recognizerInfo;

                    // Set the recognizer to the top entry in the query result.
                    recoWithUI.Recognizer.SetRecognizer(recognizers.ElementAt(0));

                    // Create a string array of French numbers.
                    string[] settings = (from GameColor color in gameColors select color.ColorName).ToArray();

                    // Create a list grammar from the string array and add it to the grammar set.
                    recoWithUI.Recognizer.Grammars.AddGrammarFromList("englishColors", settings);

                    // Display text to prompt the user's input.
                    recoWithUI.Settings.ListenText = "Say the name of the button background color";
                    // Give an example of ideal speech input.
                    recoWithUI.Settings.ExampleText = " 'Blue','Green'... ";

                    // Load the grammar set and start recognition.
                    SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();
                    string action = recoResult.RecognitionResult.Text;
                    if (recoResult.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
                    {
                        string resultText = recoResult.RecognitionResult.Text;
                        resultText = ConfidenceText.Text = resultText;
                        if (resultText.Contains(currentColor.ColorName))
                        {
                            Speak();
                            doNext();
                        }
                        else
                        {
                            Speak("bad answer");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something bad appends...");
                }
            }
            busy = false;
        }

        private void doNext()
        {
            currentColor=gameColors[RandomService.Instance.getRand(0,gameColors.Count-1)];
            Button.Fill = currentColor.ColorBrush;
        }
    }
}