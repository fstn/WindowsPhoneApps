using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using BrokeThePig.Source.AI;
using BrokeThePig.UC.Weapons;
using FstnAnimation.Dynamic;
using Microsoft.Xna.Framework.Audio;

namespace BrokeThePig.UC
{
    public delegate void OnTapEventHandler(PointNumber newNumber);
    public class GraphicComponent:Canvas
    {
        public event OnTapEventHandler OnTap;
        private Boolean ended = false;

        public PointNumber CurrentNumber
        {
            get { return (PointNumber)GetValue(CurrentNumberProperty); }
            set { SetValue(CurrentNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentNumberProperty =
            DependencyProperty.Register("CurrentNumber", typeof(PointNumber), typeof(GraphicComponent), new PropertyMetadata(new PointNumber(0)));

        public int Inc
        {
            get { return (int)GetValue(IncProperty); }
            set { SetValue(IncProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Inc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IncProperty =
            DependencyProperty.Register("Inc", typeof(int), typeof(GraphicComponent), new PropertyMetadata(0));




        public int StartNumber
        {
            get { return (int)GetValue(StartNumberProperty); }
            set { SetValue(StartNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartNumberProperty =
            DependencyProperty.Register("StartNumber", typeof(int), typeof(GraphicComponent), new PropertyMetadata(0));



        public int EndNumber
        {
            get { return (int)GetValue(EndNumberProperty); }
            set { SetValue(EndNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndNumberProperty =
            DependencyProperty.Register("EndNumber", typeof(int), typeof(GraphicComponent), new PropertyMetadata(0));

        private SoundController sc;

        public GraphicComponent()
        {
            sc = new SoundController();
            this.Tap += GraphicPart_Tap;
                
        }

        void GraphicPart_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CurrentNumber.FightNumber(AI.Instance.CurrentWeapon);
            MoveEffect me = new MoveEffect();
            me.From = new Point(0, 0);
            me.To = new Point(0, 1);
            me.Duration = 120;
            me.Mode = EasingMode.EaseIn;
            me.EasingFunction = new CircleEase();
            me.Start((FrameworkElement)this.Children.FirstOrDefault());
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
                me.Start((FrameworkElement)this.Children.FirstOrDefault());
                if (OnTap != null)
                {
                    OnTap(CurrentNumber);
                }
           
            
        }
        public void Update(int Number)
        {
            CurrentNumber.Number = Number;
            if (!ended)
            {
                if (CurrentNumber.Number <= EndNumber)
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
