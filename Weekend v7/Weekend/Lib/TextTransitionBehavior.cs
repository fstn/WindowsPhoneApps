using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace Weekend.Lib
{
    public class TextTransitionBehavior : Behavior<TextBlock>
    {
        private int nbIncMax = 20;
        private int nbIncCurrent = 0;
        private String intitialTxt;
        private DispatcherTimer dt;
        public String IntitialTxt
        {
            get { return intitialTxt; }
            set { intitialTxt = value; }
        }
        public TextBlock text { get; set; }
        public String InitialTxt { get; set; }
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        /// <summary>
        /// DependencyProperty to allow binding of our DataObject
        /// </summary>
        public static readonly DependencyProperty DataObjectProperty = DependencyProperty.Register(
            "DataObject", typeof(TextBlock), typeof(TextTransitionBehavior), null);

        public TextBlock DataObject
        {
            get { return (TextBlock)GetValue(DataObjectProperty); }
            set { SetValue(DataObjectProperty, value); }
        }

        public void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            text = (TextBlock)sender;
            intitialTxt = text.Text;
             dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 500); // 500 Milliseconds
            dt.Tick += dt_Tick;
            dt.Start();
        }
        void dt_Tick(object sender, EventArgs e)
        {
            if (nbIncCurrent < nbIncMax)
            {
                text.Text = Melanger(text.Text);
                nbIncCurrent++;
            }
            else
            {
                dt.Stop();
                text.Text = InitialTxt;
            }

        }
        private string Melanger(string texte)
        {
            string melange = string.Empty;

            do
            {
                Random r = new Random();
                int index = r.Next(texte.Length);// tire l'index d'un caractère de la chaine

                melange += texte[index].ToString();//mets ce caractère dans le mélange

                texte = texte.Remove(index, 1);//enlève le caractère de la chaine d'origine pour ne pas les réutiliser

            } while (texte.Length > 0);

            return melange;
        }

    }
}
