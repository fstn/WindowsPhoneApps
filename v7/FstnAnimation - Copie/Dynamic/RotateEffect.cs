using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FstnAnimation.Dynamic
{
    public class RotateEffect : DynamicEffect
    {
        public double From { get; set; }
        public double To { get; set; }
        public double Speed { get; set; } 
        public int Duration { get; set; }
        public EasingMode Mode { get; set; }
        public EasingFunctionBase EasingFunction { get; set; }
        public Point RotationCenter { get; set; }
        public DependencyProperty RotationAxe { get; set; }
        public RotateEffect(double from, double to, double speed, EasingMode mode, EasingFunctionBase easingFunction)
        {
            this.From = from;
            this.To = to;
            this.Speed = speed;
            this.Mode = mode;
            this.EasingFunction = easingFunction;
        }
        public RotateEffect(double from, double to, double speed,int duration, EasingMode mode, EasingFunctionBase easingFunction)
        {
            this.From = from;
            this.To = to;
            this.Speed = speed;
            this.Mode = mode;
            this.EasingFunction = easingFunction;
            this.Duration = duration;
        }
        public RotateEffect()
        {
            Speed = -1;
            Duration = -1;
        }
        protected override Storyboard CreateStoryboard(FrameworkElement target)
        {
            Storyboard result = new Storyboard();
            if (target != null)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = From;
                animation.To = To;
                if (Speed != -1)
                    animation.SpeedRatio = Speed;
                if (Duration != -1)
                {
                    animation.SpeedRatio = 1;
                    animation.Duration = TimeSpan.FromMilliseconds(Duration);
                }
                EasingFunction.EasingMode = Mode;
                PlaneProjection projection = new PlaneProjection();
                target.Projection = projection;
                animation.EasingFunction = EasingFunction;
                Storyboard.SetTarget(animation, projection);
                if(RotationCenter!=null){
                    projection.CenterOfRotationX=RotationCenter.X;
                    projection.CenterOfRotationY=RotationCenter.Y;
                }

                if (RotationAxe == null)
                {
                    RotationAxe = PlaneProjection.RotationYProperty;
                }
                Storyboard.SetTargetProperty(animation, new PropertyPath(RotationAxe));

                result.Children.Add(animation);
            }
            else
            {
                Debugger.Log(1, "animation", "target is null for RotateEffect");
            }
            return result;
        }
    }
}
