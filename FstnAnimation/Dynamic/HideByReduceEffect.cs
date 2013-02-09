using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnAnimation.Dynamic
{
    public class HideByReduceEffect : DynamicEffect
    {
        public double Speed { get; set; }
        public double TargetWidth { get; set; }
        public FrameworkElement Target { get; set; }

        public HideByReduceEffect(double speed)
        {
            this.Speed = speed;
        }
        public HideByReduceEffect()
        { }
        protected override Storyboard CreateStoryboard(FrameworkElement target)
        {
            Target = target;
            Target.Visibility = Visibility.Visible;
            TargetWidth = target.Width;
            Storyboard result = new Storyboard();
            result.Completed += result_Completed;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = target.Width;
            animation.To = 0;
            animation.SpeedRatio = Speed;
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath("FrameworkElement.Width"));

            result.Children.Add(animation);

            return result;
        }

        void result_Completed(object sender, EventArgs e)
        {
            Target.Visibility = Visibility.Collapsed;
            Target.Width = TargetWidth;
        }
    }
}