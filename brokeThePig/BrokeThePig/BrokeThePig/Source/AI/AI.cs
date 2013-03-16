using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BrokeThePig.UC;
using Microsoft.Devices;

namespace BrokeThePig.Source.AI
{
    public delegate void LevelEndedEventHandler();
    public delegate void GameEndEventHandler();
    public delegate void FightEventHandler();

    public class AI : INotifyPropertyChanged
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

        public event LevelEndedEventHandler LevelEnded;
        public event GameEndEventHandler GameEnd;
        public event FightEventHandler OnFight;
        public int CurrentMoney { get; set; }
        public List<BrokeThePigWeapon> Weapons { get; set; }
        public PointNumber CurrentNumber { get; set; }
        public BrokeThePigWeapon CurrentWeapon { get; set; }
        public Level CurrentLevel
        {
            get
            {
               return this.Levels[CurrentNumber.LevelNumber];
            }
        }
        public Dictionary<int,Level> Levels { get; set; }
        #endregion

        private void Initialize()
        {
            Weapons = new List<BrokeThePigWeapon>();

            Levels = new Dictionary<int, Level>();
            Levels.Add(0, new Level(1000, 10, "../Images/Gift/dollar.png"));
            Levels.Add(1, new Level(10000, 100, "../Images/Gift/euro.png"));
            Levels.Add(2, new Level(100000, 1000, "../Images/Gift/pound.png"));
            Levels.Add(3, new Level(1000000, 10000, "../Images/Gift/gift.png"));


            CurrentNumber = new PointNumber(0);
            CurrentNumber.BecomeNull += CurrentNumber_BecomeNull;
            CurrentMoney = 0;
            CurrentNumber.LevelNumber = 0;
            CurrentNumber.Number = CurrentLevel.NumberOfTapToDo;

        }

        void CurrentNumber_BecomeNull()
        {
            if(LevelEnded!=null){
                LevelEnded();
            }
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(500));
            CurrentMoney +=this.Levels[CurrentNumber.LevelNumber].MoneyToWin;
            int newLevel= CurrentNumber.LevelNumber+1;
            if (newLevel < Levels.Count())
            {
                CurrentNumber.Number = this.Levels[newLevel].NumberOfTapToDo;
                CurrentNumber.LevelNumber = newLevel;
                OnPropertyChanged("CurrentMoney");
            }
            else
            {
                if (GameEnd != null)
                {
                    GameEnd();
                }
            }
        }

        public void AddWeapon(BrokeThePigWeapon Weapon)
        {
            Weapons.Add(Weapon);
            Weapon.SelectedEvent += Weapon_Selected;
            Weapon.FightEvent += Weapon_FightEvent;
        }

        public void Fight()
        {
            if (OnFight != null)
            {
                OnFight();
            }
            if (CurrentWeapon != null)
            CurrentWeapon.Fight();
        }
        void Weapon_FightEvent()
        {
            CurrentNumber.IsFighted(CurrentWeapon);            
        }

        void Weapon_Selected(BrokeThePigWeapon SelectedWeapon)
        {
            if(CurrentWeapon!=null)
                CurrentWeapon.UnSelect();
            SelectedWeapon.Select();
            CurrentWeapon = SelectedWeapon;
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
