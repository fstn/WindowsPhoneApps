using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using BrokeThePig.Source.AI;
using BrokeThePig.UC.Weapons;
using FstnAnimation.Dynamic;
using FstnCommon.Util;
using FstnUserControl.ImageSprite;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Audio;

namespace BrokeThePig.UC
{
    public delegate void OnTapEventHandler();
    public class GraphicComponent:Canvas
    {
        private GCClassicPart ClassicPart;
        private GCFightPart FightPart;
        public event OnTapEventHandler OnTap;
        private Boolean ended = false;
        private DispatcherTimer dt;
     
        public Double EndNumber
        {
            get { return (Double)GetValue(EndNumberProperty); }
            set { SetValue(EndNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndNumberProperty =
            DependencyProperty.Register("EndNumber", typeof(Double), typeof(GraphicComponent), new PropertyMetadata(0.0));

        private SoundController sc;

        public GraphicComponent()
        {
            sc = new SoundController();
            this.Tap += GraphicPart_Tap;
            AI.Instance.CurrentNumber.ValueChanged += Update;
            this.Loaded += GraphicComponent_Loaded;
            dt = new DispatcherTimer();
            dt.Tick += dt_Tick;
        }


        void GraphicComponent_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in Children)
            {
                GCFightPart fightPart = element as GCFightPart;
                if (fightPart != null)
                    FightPart = fightPart;

                GCClassicPart classicPart = element as GCClassicPart;
                if (classicPart != null)
                    ClassicPart = classicPart;
            }
        }

        void GraphicPart_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
          
            AI.Instance.Fight();
            MoveEffect me = new MoveEffect();
            me.From = new Point(0, 0);
            me.To = new Point(0, 1);
            me.Duration = 120;
            me.Mode = EasingMode.EaseIn;
            me.EasingFunction = new CircleEase();
            me.Start((FrameworkElement)this);
            me.Completed += me_Completed;
           
        }
        void me_Completed(object sender, EventArgs e)
        {
            MoveEffect me = new MoveEffect();
           
                me.From = new Point(0, 1);
                me.To = new Point(0, 0);
                me.Duration = 120;
                me.Mode = EasingMode.EaseIn;
                me.EasingFunction = new CircleEase();
                me.Start((FrameworkElement)this);
                if (OnTap != null)
                {
                    OnTap();
                }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            ClassicPart.Visibility = Visibility.Visible;
            FightPart.Visibility = Visibility.Collapsed;
            dt.Stop();
        }
        public void Update()
        {
            if (!ended)
            {
                if (ClassicPart != null && FightPart != null)
                {
                    ClassicPart.Visibility = Visibility.Collapsed;
                    FightPart.Visibility = Visibility.Visible;
                    dt.Stop();
                    dt.Interval = TimeSpan.FromMilliseconds(200);
                    dt.Start();
                }
                StaticSprite sprite;
                foreach (UIElement elt in this.Children)
                {
                    if ((sprite = elt as StaticSprite) != null)
                    {
                        float n = MathUtil.Norm(AI.Instance.CurrentNumber.Number, AI.Instance.CurrentLevel.NumberOfTapToDo, 0);
                        int Number = (int)MathUtil.Lerp(0, sprite.Length-1, n);
                        sprite.MoveTo(Number);
                    }
                }
                if (AI.Instance.CurrentNumber.Number <= EndNumber * AI.Instance.CurrentLevel.NumberOfTapToDo)
                {

                    ended = true;
                    MoveEffect me = new MoveEffect();

                    sc.PlaySound("Sounds/tap6.wav");
                    me.From = new Point(0, 10);
                    me.To = new Point(0, 900);
                    me.Duration = 1200;
                    me.Mode = EasingMode.EaseIn;
                    me.EasingFunction = new CircleEase();
                    me.Start((FrameworkElement)this.Children.FirstOrDefault());
                    me.Completed += me_CompletedEnd;
                }
            }
        }

        private void me_CompletedEnd(object sender, EventArgs e)
        {
            this.Tap -= GraphicPart_Tap;
        }
    }
}
