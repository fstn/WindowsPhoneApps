using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrokeThePig.UC;

namespace BrokeThePig.Source.AI
{
    public class AI
    {

        #region Singleton

        private static AI _instance = null;

        private AI() { }

        public static AI Instance
        {
            get
            {
                if (_instance == null)
                {
                    AI instance = new AI();
                    instance.Initialize();
                    _instance = instance;
                }
                return _instance;
            }
        }

        #endregion


        #region Fields
        public List<BrokeThePigWeapon> Weapons { get; set; }

        public BrokeThePigWeapon CurrentWeapon { get; set; }
        #endregion

        private void Initialize()
        {
            Weapons = new List<BrokeThePigWeapon>();
        }

        public void AddWeapon(BrokeThePigWeapon Weapon)
        {
            Weapons.Add(Weapon);
            Weapon.Selected += Weapon_Selected;
        }

        void Weapon_Selected(BrokeThePigWeapon SelectedWeapon)
        {
            if(CurrentWeapon!=null)
                CurrentWeapon.UnSelect();
            SelectedWeapon.Select();
            CurrentWeapon = SelectedWeapon;
        }

        
	
    }
}
