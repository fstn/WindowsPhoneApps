using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BrokeThePig.Source.AI;
using Microsoft.Devices;

namespace BrokeThePig.UC
{
    public delegate void SelectedEventHandler(BrokeThePigWeapon SelectedWeapon);
    public delegate void FigutEventHandler();
    public abstract class BrokeThePigWeapon : StackPanel
    {

        public event SelectedEventHandler SelectedEvent;
        public event FigutEventHandler FightEvent;
        private SoundController sc;
        private Boolean enabled=false;

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

        // Using a DependencyProperty as the backing store for ShootSound.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoneyNeededProperty =
            DependencyProperty.Register("MoneyNeeded", typeof(int), typeof(BrokeThePigWeapon), new PropertyMetadata(0));

        public int MoneyNeeded
        {
            get { return (int)GetValue(MoneyNeededProperty); }
            set { SetValue(MoneyNeededProperty, value); }
        }
        #endregion
      
        public virtual void Fight()
        {
            if (FightEvent != null)
            {
                FightEvent();
            }
            if (ShootSound != "")
                sc.PlaySound(ShootSound);
        }

        public BrokeThePigWeapon()
        {
            this.Tap += BrokeThePigWeapon_Tap;
            this.Loaded += BrokeThePigWeapon_Loaded;
            AI.Instance.AddWeapon(this);
            sc = new SoundController();
            AI.Instance.LevelEnded += Instance_LevelEnded;           
            
        }

        void BrokeThePigWeapon_Loaded(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Red);
            TextBlock amount = new TextBlock();
            amount.Text = MoneyNeeded + "$";
            amount.TextAlignment = TextAlignment.Center;
            this.Children.Add(amount);
            if (AI.Instance.CurrentMoney >= MoneyNeeded)
            {
                Enable();
            }
        }

        void Instance_LevelEnded()
        {
            if (AI.Instance.CurrentMoney >= MoneyNeeded)
            {
                Enable();
            }
        }

        void Enable()
        {
            this.enabled = true;
            this.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BrokeThePigWeapon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (enabled)
            {
                if (SelectedEvent != null)
                {
                    SelectedEvent(this);
                }
                Select();
            }
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
