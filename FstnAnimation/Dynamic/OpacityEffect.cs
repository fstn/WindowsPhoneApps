using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FstnAnimation.Dynamic
{
    public class OpacityEffect : DynamicEffect
    {
        public double From { get; set; }
        public double To { get; set; }
        public double Speed { get; set; }
        public int Duration { get; set; }
        public EasingMode Mode { get; set; }
        public EasingFunctionBase EasingFunction { get; set; }
        public OpacityEffect(double from, double to, double speed,int duration,EasingMode mode,EasingFunctionBase easingFunction)
        {
            this.From = from;
            this.To = to;
            this.Speed = speed;
            this.Duration = duration;
            this.Mode = mode;
            this.EasingFunction = easingFunction;
        }
        public OpacityEffect()
        { }
        protected override Storyboard CreateStoryboard(FrameworkElement target)
        {
            Storyboard sb = new Storyboard();
            
            DoubleAnimation animationY = new DoubleAnimation();
            animationY.BeginTime = TimeSpan.FromMilliseconds(0);
            animationY.Duration = TimeSpan.FromMilliseconds(Duration);
            animationY.From = From;
            animationY.To = To;
            EasingFunction.EasingMode = Mode;
            animationY.EasingFunction = EasingFunction;
            Storyboard.SetTarget(animationY, target);
            target.CacheMode = new BitmapCache();
            Storyboard.SetTargetProperty(animationY, new PropertyPath(SolidColorBrush.OpacityProperty));
            sb.Children.Add(animationY);
            return sb;
        }
    }
}
