using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AppsRoulet.Lib
{
    public class TextTransitionBehavior : Behavior<TextBlock>
    {
        public static readonly DependencyProperty SetFocusOnLoadedProperty =
     DependencyProperty.Register("SetFocusOnLoaded", typeof(bool), typeof(TextTransitionBehavior), new PropertyMetadata(true));
 
        /// <summary>
        /// Set focus to <see cref="AssociatedObject"/> on loaded event. By default <value>true</value>.
        /// </summary>
        public bool SetFocusOnLoaded
        {
            get { return (bool)GetValue(SetFocusOnLoadedProperty); }
            set { SetValue(SetFocusOnLoadedProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;

            if (SetFocusOnLoaded)
            {
                //AssociatedObject.Text();
            }
        }
    }
}
