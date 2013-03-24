using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FstnAnimation.Dynamic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

/**
 * SecreteAnimationControl
 * Animation to help user to pe patient ...
 * Anime rectangle use to create something fun
 * Stephen ZAMBAUX
 **/

namespace FstnUserControl
{
    public delegate void StartSecreteAnimationEventHandler();
    public delegate void StopSecreteAnimationEventHandler();
    public class SecreteAnimationControl : Canvas
    {
        //Publish when started sb is complete
        public event StartSecreteAnimationEventHandler Started;
        //Publish when stopped sb is complete
        public event StopSecreteAnimationEventHandler Stopped;

        //Animated rectangle
        private Rectangle foregroundRectangle;
        public Brush ForegroundColor { get; set; }
        //Rectangle use to mask background
        private Rectangle backgroundRectangle;
        public Brush BackgroundColor { get; set; }

        //if true we use backgroundrectangle
        public Boolean WithMask { get; set; }

        public List<DynamicEffect> startEffects { get; set; }
        public List<DynamicEffect> stopEffects { get; set; }

        private int nbStartComplete = 0;
        private int nbStopComplete = 0;

        public SecreteAnimationControl()
        {
            ForegroundColor = (Brush)Application.Current.Resources["PhoneAccentBrush"];
            BackgroundColor = (Brush)Application.Current.Resources["PhoneBackgroundBrush"];
        }

        public void load()
        {
            if (WithMask)
            {
                backgroundRectangle = new Rectangle();
                backgroundRectangle.Width = Width;
                backgroundRectangle.Height = Height;
                backgroundRectangle.Fill = BackgroundColor;

                Children.Add(backgroundRectangle);
            }
        }

        public void start()
        {
            foregroundRectangle = new Rectangle();
            foregroundRectangle.Width = Width + 10;
            foregroundRectangle.Height = Height + 10;
            foregroundRectangle.Fill = ForegroundColor;
            Children.Add(foregroundRectangle);

            if (startEffects != null)
            {
                foreach (DynamicEffect effect in startEffects)
                {
                    effect.Completed += OnStart_Completed;
                    effect.Start(foregroundRectangle);
                }
            }

        }

        void OnStart_Completed(object sender, EventArgs e)
        {
            if (nbStartComplete == startEffects.Count - 1)
            {
                if (WithMask)
                {
                    backgroundRectangle.Visibility = Visibility.Collapsed;
                }
                if (Started != null)
                    Started();
                nbStartComplete = 0;
            }
            else
            {
                nbStartComplete++;
            }
        }

        public void stop()
        {
            if (foregroundRectangle != null && stopEffects != null)
            {
                foreach (DynamicEffect effect in stopEffects)
                {
                    effect.Completed += OnStop_Completed;
                    effect.Start(foregroundRectangle);
                }
            }
        }

        void OnStop_Completed(object sender, EventArgs e)
        {
            if (nbStopComplete == stopEffects.Count - 1)
            {
                if (Stopped != null)
                    Stopped();
                Visibility = Visibility.Collapsed;
            }
            else
            {
                nbStopComplete++;
            }
        }

        public void ChangeColor(Brush brush)
        {
            foregroundRectangle.Fill = brush;
        }

        public void addStartEffect(DynamicEffect effect)
        {
            if (startEffects == null)
                startEffects = new List<DynamicEffect>();
            startEffects.Add(effect);
        }
        public void addStopEffect(DynamicEffect effect)
        {
            if (stopEffects == null)
                stopEffects = new List<DynamicEffect>();
            stopEffects.Add(effect);
        }
    }
}
