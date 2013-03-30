using System;
using System.Linq;
using System.Windows;
using FstnDesign.FstnColor;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Settings.Resources;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.Recognition;
using FstnCommon.Util;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Phone.UI.Input;

namespace Settings
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SpeechSynthesizer ss;
        private SpeechRecognizerUI recoWithUI;
        private ConnectionSettingsTask connectionSettingsTask;
        private bool busy = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            connectionSettingsTask = new ConnectionSettingsTask();

            this.Tap += MainPage_Tap;
        }

        void MainPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ss = new SpeechSynthesizer();
            Listen();
        }


        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ss = new SpeechSynthesizer();
            Listen();
        }


        private async void Listen()
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
                    string[] settings = { "AirplaneMode", "Wifi", "Bluetooth", "Cellular" };

                    // Create a list grammar from the string array and add it to the grammar set.
                    recoWithUI.Recognizer.Grammars.AddGrammarFromList("englishNumbers", settings);

                    // Display text to prompt the user's input.
                    recoWithUI.Settings.ListenText = "Settings name:  AirplaneMode , Wifi, Bluetooth, Cellular";

                    // Give an example of ideal speech input.
                    recoWithUI.Settings.ExampleText = " 'AirplaneMode', 'Wifi', 'Bluetooth', 'Cellular' ";

                    // Load the grammar set and start recognition.
                    SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();
                    string action = recoResult.RecognitionResult.Text;
                    string toStay = "can't reconize settings";
                    SpeechSynthesizer ss = new SpeechSynthesizer();
                    VoiceInformation vi = InstalledVoices.All.Where(v => v.Language == "en-EN" && v.Gender == VoiceGender.Male).FirstOrDefault();
                    ss.SetVoice(vi);
                    switch (action)
                    {
                        case "AirplaneMode":
                            toStay = "let's go to airplane";
                            await ss.SpeakTextAsync(toStay);
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.AirplaneMode;
                            break;
                        case "Wifi":
                            toStay = "let's go to wifi";
                            await ss.SpeakTextAsync(toStay);
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.WiFi;
                            break;
                        case "Bluetooth":
                            toStay = "let's go to bluetooth";
                            await ss.SpeakTextAsync(toStay);
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
                            break;
                        case "Cellular":
                            toStay = "let's go to cellular";
                            await ss.SpeakTextAsync(toStay);
                            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Cellular;
                            break;
                    }
                    connectionSettingsTask.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something bad appends...");
                }
            }
            busy = false;

        }
    }
}