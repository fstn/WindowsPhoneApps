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
using System.Windows.Threading;
using BrokeThePig.Source.AI;
using BrokeThePig.UC;
using FstnAnimation.Dynamic;
using FstnCommon.Util.Settings;
using Microsoft.Phone.Controls;

namespace BrokeThePig
{
    public partial class MainPage : PhoneApplicationPage
    {
        public AI GameAI { get; set; }
        public MainPage()
        {
            GameAI = AI.Instance;
            this.DataContext = this;
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Pig.Exploded += Pig_Exploded;
        }

        void Pig_Exploded()
        {
            PigLayout.Tap += b_Tap;
        }


        void b_Tap(object sender, GestureEventArgs e)
        {
            PigLayout.Tap -= b_Tap;
            Pig.Exploded -= Pig_Exploded;
            double left = Canvas.GetLeft(Pig);
            double top = Canvas.GetTop(Pig);
            PigLayout.Children.Clear();
            Pig = new Pig();
            Canvas.SetLeft(Pig,left);
            Canvas.SetTop(Pig,top);
            Pig.Exploded += Pig_Exploded;
            PigLayout.Children.Add(Pig);
        }

        private void Button_Tap_1(object sender, GestureEventArgs e)
        {
            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentCount, 0);
            AI.Instance.CurrentNumber.LevelNumber = 0;

            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentCount, 0);
            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentLevel, 0);
            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentMoney, 0);
            AI.Instance.CurrentNumber.Number = 0;
            AI.Instance.CurrentNumber.LevelNumber = 0;
            AI.Instance.CurrentMoney = 0;
        }


    }
}
