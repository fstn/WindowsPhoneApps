using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace FstnAnimation.Dynamic
{
    public class RotateEffect : DynamicEffect
    {
        public double From { get; set; }
        public double To { get; set; }
        public double Speed { get; set; }
        public EasingMode Mode { get; set; }
        public EasingFunctionBase EasingFunction { get; set; }
        public RotateEffect(double from, double to, double speed, EasingMode mode, EasingFunctionBase easingFunction)
        {
            this.From = from;
            this.To = to;
            this.Speed = speed;
            this.Mode = mode;
            this.EasingFunction = easingFunction;
        }
        public RotateEffect()
        { }
        protected override Storyboard CreateStoryboard(FrameworkElement target)
        {
            Storyboard result = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = From;
            animation.To = To;
            animation.SpeedRatio = Speed;
            EasingFunction.EasingMode = Mode;
            animation.EasingFunction = EasingFunction;
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationY)"));

            result.Children.Add(animation);

            return result;
        }
    }
}
