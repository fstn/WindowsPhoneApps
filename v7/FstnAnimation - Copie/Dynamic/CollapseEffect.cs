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
    public class CollapseEffect : DynamicEffect
    {
        public double Speed { get; set; }

        public CollapseEffect(double speed)
        {
            this.Speed = speed;
        }
        public CollapseEffect()
        { }
        protected override Storyboard CreateStoryboard(FrameworkElement target)
        {
            Storyboard storyboard = new Storyboard();
            ObjectAnimationUsingKeyFrames animation = new ObjectAnimationUsingKeyFrames();
            Debugger.Log(1,"",animation.KeyFrames.ToString());
            DiscreteObjectKeyFrame keyFrame =  new DiscreteObjectKeyFrame();
            KeyTime keyTime = KeyTime.FromTimeSpan(new TimeSpan(0));
            keyFrame.KeyTime = keyTime;
            keyFrame.Value=Visibility.Visible;

            DiscreteObjectKeyFrame keyFrame2 = new DiscreteObjectKeyFrame();
            KeyTime keyTime2 = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(250));
            keyFrame2.KeyTime = keyTime2;
            keyFrame2.Value=Visibility.Collapsed;

            animation.KeyFrames.Add(keyFrame);
            animation.KeyFrames.Add(keyFrame2);
            animation.SpeedRatio = Speed;
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Image.VisibilityProperty));
            return storyboard;
        }
    }
}