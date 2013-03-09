using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BrokeThePig.Source.AI;

namespace BrokeThePig.UC
{
    public delegate void SelectedEventHandler(BrokeThePigWeapon SelectedWeapon);
    public abstract class BrokeThePigWeapon:Canvas
    {

        public event SelectedEventHandler Selected;
        private SoundController sc;

        #region dependency property
        public String ShootSound
        {
            get { return (String)GetValue(ShootSoundProperty); }
            set { SetValue(ShootSoundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShootSound.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShootSoundProperty =
            DependencyProperty.Register("ShootSound", typeof(String), typeof(BrokeThePigWeapon), new PropertyMetadata(""));

        // Using a DependencyProperty as the backing store for ShootSound.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionSoundProperty =
            DependencyProperty.Register("SelectionSound", typeof(String), typeof(BrokeThePigWeapon), new PropertyMetadata(""));

        public String SelectionSound
        {
            get { return (String)GetValue(SelectionSoundProperty); }
            set { SetValue(SelectionSoundProperty, value); }
        }



        public int Damage
        {
            get { return (int)GetValue(DamageProperty); }
            set { SetValue(DamageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Damage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DamageProperty =
            DependencyProperty.Register("Damage", typeof(int), typeof(BrokeThePigWeapon), new PropertyMetadata(0));


        #endregion
        public virtual int Fight(int Number)
        {
            if (ShootSound != "")
                sc.PlaySound(ShootSound);
            return Number - Damage;
        }
        public BrokeThePigWeapon()
        {
            this.Tap+=BrokeThePigWeapon_Tap;
            AI.Instance.AddWeapon(this);
            sc = new SoundController();
        }

        private void BrokeThePigWeapon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Selected != null)
            {
                Selected(this);
            }
            Select();
        }

        public virtual void Enable()
        {
        }
        public virtual void Disable()
        {
        }
        public virtual void UnSelect()
        {
            this.Background = new SolidColorBrush(Colors.Transparent);
        }
        public virtual void Select()
        {
            this.Background = (Brush)Application.Current.Resources["PhoneAccentBrush"];
            sc.PlaySound(SelectionSound);
        }
    }
}
