using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BrokeThePig.UC
{
    public delegate void BecomeNullEventHandler();
    public delegate void ValueChangedEventHandler();
    public class PointNumber : INotifyPropertyChanged
 
    {
        public event BecomeNullEventHandler BecomeNull;
        public event ValueChangedEventHandler ValueChanged;
        public int LevelNumber { get; set; }
        public int Number { get; set; }
        public PointNumber(int Number)
        {
            this.Number = Number;
        }

        public void IsFighted(BrokeThePigWeapon Weapon)
        {
            if (ValueChanged != null)
            {
                ValueChanged();
            }
            if (Weapon != null)
                Number -= Weapon.Damage;
            if (Number <= 0)
            {
                Number = 0;
                if (BecomeNull != null) {
                    BecomeNull();
                }
            }

            OnPropertyChanged("Number");
        }

        private void OnPropertyChanged(string propertyName)
        {

            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

    }
}
