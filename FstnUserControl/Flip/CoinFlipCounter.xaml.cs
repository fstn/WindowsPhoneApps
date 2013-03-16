using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FstnAnimation.Dynamic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl
{
    public partial class CoinFlipCounter : UserControl
    {
        public event CompletedEventHandler Completed;

        private int NumberOfRotation = 0;
        private Queue<FrameworkElement> q;
        private RotateEffect effect;
        public static readonly DependencyProperty Image1SourceProperty;
        public static readonly DependencyProperty Image2SourceProperty;
        public static readonly DependencyProperty TossProperty;
        private Boolean ended = true;
        public TimeCounter TimeCounterTextBox
        {
            get { return timeCounterTextBox; }
            set { timeCounterTextBox = value; }
        }
        
        public CoinFlipCounter()
        {
            this.Visibility = Visibility.Collapsed;
            InitializeComponent(); 
            this.Loaded += CoinFlipCounter_Loaded;

        }

        void CoinFlipCounter_Loaded(object sender, RoutedEventArgs e)
        {
            timeCounterTextBox.StartCount();
        }
        static CoinFlipCounter()
        {
            TossProperty = DependencyProperty.Register("Toss", typeof(Boolean), typeof(CoinFlipCounter), new PropertyMetadata(true));
            Image1SourceProperty = DependencyProperty.Register("Image1Source", typeof(String), typeof(CoinFlipCounter), new PropertyMetadata(""));
            Image2SourceProperty = DependencyProperty.Register("Image1Source", typeof(String), typeof(CoinFlipCounter), new PropertyMetadata(""));
           
        }
        public Boolean Toss
        {
            get { return (Boolean)GetValue(TossProperty); }
            set { SetValue(TossProperty, value); }
        }
        public String Image1Source
        {
            get { return (String)GetValue(Image1SourceProperty); }
            set
            {
                SetValue(Image1SourceProperty, value);
                Uri uri = new Uri(Image1Source, UriKind.Relative);
                if (uri != null)
                {
                    finalImage.Source = new BitmapImage(uri);
                }
            }
        }
        public String Image2Source
        {
            get { return (String)GetValue(Image2SourceProperty); }
            set
            {
                SetValue(Image2SourceProperty, value);
                Uri uri = new Uri(Image2Source, UriKind.Relative);
                if (uri != null)
                {
                    nonFinalImage.Source = new BitmapImage(uri);
                }
            }
        }

        private void finalResponseImg_Loaded(object sender, EventArgs e)
        {
        }

        public void Start()
        {
            if (ended)
            {
                NumberOfRotation=0;
                this.Visibility = Visibility.Visible;
                ended = false;
                nonFinalCanvas.Visibility = Visibility.Visible;
                finalCanvas.Visibility = Visibility.Collapsed;
                EasingFunctionBase easing = new SineEase();
                effect = new RotateEffect(-90, 90, 100, EasingMode.EaseInOut, easing);
                effect.RotationCenter = new Point(0.5, 0.5);
                effect.Duration = 500;
                effect.Completed += effect_Completed;
                q = new Queue<FrameworkElement>();
                if (Toss)
                {
                    effect.Start(nonFinalCanvas);
                    q.Enqueue(finalCanvas);
                    q.Enqueue(nonFinalCanvas);
                }
                else
                {
                    effect.Start(finalCanvas);
                    q.Enqueue(nonFinalCanvas);
                    q.Enqueue(finalCanvas);
                }
            }
        }
        public void effect_Completed(object sender, EventArgs e)
        {
            FrameworkElement first = q.Dequeue();
            FrameworkElement second = q.Dequeue();
            effect.Duration += effect.Duration/4;
            first.Visibility = Visibility.Visible;
            second.Visibility = Visibility.Collapsed;
            if (effect.Duration > 2000 && NumberOfRotation %2==0)
            {
                effect.Duration += 2000;
                effect.To = 0;
                effect.Completed -= effect_Completed;
                ended = true;

                if (this.Completed != null)
                {
                    Completed(this, null);
                }
            }
            else
            {
                q.Enqueue(second);
                q.Enqueue(first);
            }
            NumberOfRotation++;
           /* if(effect.Speed <1){
                cote.Visibility = Visibility.Visible;
                collapseEffect.Start(cote);
            }*/
            effect.Start(first);
        }
     
    }
}
