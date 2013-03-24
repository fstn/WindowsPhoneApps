using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FstnAnimation.Dynamic
{
    public abstract class DynamicEffect
    {
        protected FrameworkElement target;
        Storyboard storyboard;

        public DynamicEffect()
        {
        }

        protected abstract Storyboard CreateStoryboard(FrameworkElement target);
        protected virtual void Add(FrameworkElement target)
        {
            this.target = target;
            storyboard = CreateStoryboard(target);
            storyboard.Completed += new EventHandler(OnCompleted);
            target.Resources.Remove("storyboard");
            target.Resources.Add("storyboard",storyboard);
        }
        protected virtual void Remove()
        {
            if (target == null)
                return;
            if (storyboard == null)
                return;
            target.Resources.Remove("storyboard");
            target = null;
            storyboard = null;
        }
        protected virtual void OnCompleted(object sender, EventArgs e)
        {
            if (Completed != null)
                Completed(target, null);
            Remove();
        }

        public virtual void Start(FrameworkElement target)
        {
            Add(target);
            storyboard.Begin();
        }
        public virtual void Stop()
        {
            if (target == null || storyboard == null)
                return;
            storyboard.Stop();
            Remove();
        }

        public event EventHandler Completed;
    }
}
