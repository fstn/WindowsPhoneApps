using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FstnAnimation.Dynamic
{
    public class MoveEffect : DynamicEffect
    {
        public Point From { get; set; }
        public Point To { get; set; }
        public double Speed { get; set; }
        public int Duration { get; set; }
        public EasingMode Mode { get; set; }
        public EasingFunctionBase EasingFunction { get; set; }
        public MoveEffect(Point from, Point to, double speed,int duration,EasingMode mode,EasingFunctionBase easingFunction)
        {
            this.From = from;
            this.To = to;
            this.Speed = speed;
            this.Duration = duration;
            this.Mode = mode;
            this.EasingFunction = easingFunction;
        }
        public MoveEffect()
        { }
        protected override Storyboard CreateStoryboard(FrameworkElement target)
        {
            Storyboard sb = new Storyboard();
            
            DoubleAnimation animationY = new DoubleAnimation();
            animationY.BeginTime = TimeSpan.FromMilliseconds(0);
            animationY.Duration = TimeSpan.FromMilliseconds(Duration);
            animationY.From = From.Y;
            animationY.To = To.Y;
            EasingFunction.EasingMode = Mode;
            animationY.EasingFunction = EasingFunction;

            DoubleAnimation animationX = new DoubleAnimation();
            animationX.BeginTime=TimeSpan.FromMilliseconds(0);
            animationX.Duration= TimeSpan.FromMilliseconds(Duration);
            animationX.From=From.X;
            animationX.To = To.X;
            animationX.EasingFunction = EasingFunction;
            Transform transform = new TranslateTransform();
            if (target.RenderTransform == null)
            {
                target.RenderTransform = transform;
            }
            else
            {
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(target.RenderTransform);
                tg.Children.Add(transform);
                target.RenderTransform = tg;
            }

            Storyboard.SetTarget(animationX, transform);
            Storyboard.SetTarget(animationY, transform);
            target.CacheMode = new BitmapCache();
            Storyboard.SetTargetProperty(animationX, new PropertyPath(TranslateTransform.XProperty));
            Storyboard.SetTargetProperty(animationY, new PropertyPath(TranslateTransform.YProperty));
            sb.Children.Add(animationX);
            sb.Children.Add(animationY);
            return sb;
        }

    }
}
