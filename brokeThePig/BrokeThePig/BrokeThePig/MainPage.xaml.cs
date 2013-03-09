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
using BrokeThePig.UC;
using FstnAnimation.Dynamic;
using Microsoft.Phone.Controls;

namespace BrokeThePig
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

            //partOfPig
            //canvas
            //behavior
            //

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Pig.Updated += Pig_Updated;
            Pig.Exploded += Pig_Exploded;
        }


        void Pig_Updated(UC.PointNumber number)
        {
            Counter.Text = number.Number.ToString();
        }

        void Pig_Exploded()
        {
            Counter.Text ="You Win";
            Button b = new Button();
            b.Tap += b_Tap;
            PigLayout.Children.Add(b);
        }

        void b_Tap(object sender, GestureEventArgs e)
        {
            PigLayout.Children.Clear();
            Pig = new Pig();
            PigLayout.Children.Add(Pig);
        }


        public void UpdateCounter()
        {
            Counter.Text = Pig.CurrentNumber.Number.ToString();
        }
    }
}
