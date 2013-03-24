using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Weekend.Resources;
using FstnUserControl;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
namespace Weekend.ViewModel
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public class MainViewModel : ViewModelBase
    {
        #region prop
        public int selectedPivot;
        public event ChangedEventHandler Changed;
        private String endOfWeekDay;
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private DateTime endOfWeekTime;
        public String ResultText { get; set; }
        private Int16 weekendDuration;
        public DateTime EventDate { get; set; }
        private List<String> listDay;
        private List<Int16> listDayNumber;


        public int SelectedPivot
        {
            get
            {
                return selectedPivot;
            }
            set
            {
                selectedPivot = value;
            }
        }
        public String EndOfWeekDay
        {
            get
            {
                if (endOfWeekDay == null)
                {
                    ReloadSettings();
                }
                return endOfWeekDay;
            }
            set
            {
                if (value != endOfWeekDay)
                {
                    endOfWeekDay = value;
                    SaveSettings();
                }
            }
        }

        public DateTime EndOfWeekTime
        {
            get { return endOfWeekTime; }
            set
            {
                if (value != endOfWeekTime)
                {
                    endOfWeekTime = value;
                    SaveSettings();
                }
            }
        }

        public Int16 WeekendDuration
        {
            get {
                if (weekendDuration == 0)
                {
                    ReloadSettings();
                }
                return weekendDuration; }
            set
            {
                if (value != weekendDuration)
                {
                    weekendDuration = value;
                    SaveSettings();
                }
            }
        }



        public List<String> ListDay
        {
            get { return listDay; }
            set
            {
                listDay = value;
                SaveSettings();
            }
        }

        public List<Int16> ListDayNumber
        {
            get { return listDayNumber; }
            set
            {
                listDayNumber = value;
                SaveSettings();
            }
        }
        #endregion

        public MainViewModel()
        {
            listDay = new List<string>();
            listDay.Add(Msg.Sunday);
            listDay.Add(Msg.Monday);
            listDay.Add(Msg.Tuesday);
            listDay.Add(Msg.Wednesday);
            listDay.Add(Msg.Thursday);
            listDay.Add(Msg.Friday);
            listDay.Add(Msg.Saturday);
            listDayNumber = new List<Int16>();
            listDayNumber.Add(1);
            listDayNumber.Add(2);
            listDayNumber.Add(3);
            listDayNumber.Add(4);
            listDayNumber.Add(5);
            listDayNumber.Add(6);
            listDayNumber.Add(7);
            ReloadSettings();

        }

        public void SaveSettings()
        {
            if (EndOfWeekTime != null)
            {
                settings.Remove("endTime");
                settings.Add("endTime", EndOfWeekTime);
            }
            if (listDay.IndexOf(EndOfWeekDay) != -1)
            {
                settings.Remove("lastDayOfWeek");
                settings.Add("lastDayOfWeek", listDay.IndexOf(EndOfWeekDay));
            }
            if (WeekendDuration != 0)
            {
                settings.Remove("weekendDuration");
                settings.Add("weekendDuration", Convert.ToInt32(WeekendDuration));
            }
            settings.Save();
            ReloadSettings();
            OnPropertyChanged("EventDate");
            if (Changed != null)
                Changed(this, null);
        }

        public Boolean IsWeekend()
        {
            return TimeCounterHelper.IsWeekend(listDay.IndexOf(EndOfWeekDay), EndOfWeekTime, Convert.ToInt32(weekendDuration));
        }

        public void ReloadSettings()
        {
            if (settings.Contains("endTime") && settings["endTime"] != null)
            {
                EndOfWeekTime = ((DateTime)settings["endTime"]);
            }
            else
            {
                EndOfWeekTime = new DateTime(2012, 12, 12, 18, 0, 0);
            }
            if (settings.Contains("lastDayOfWeek") && settings["lastDayOfWeek"] != null && (int)settings["lastDayOfWeek"] != -1)
            {
                try
                {
                    EndOfWeekDay = listDay[((int)settings["lastDayOfWeek"])];
                }
                catch (InvalidCastException e)
                {
                    EndOfWeekDay = listDay[5];
                    Debugger.Log(3, "error", "bad lastDayOfWeek " + settings["lastDayOfWeek"]);
                }
            }
            else
            {
                EndOfWeekDay = listDay[0];

            }

            if (settings.Contains("weekendDuration") && (int)settings["weekendDuration"] != 0)
            {
                WeekendDuration = (Int16.Parse(settings["weekendDuration"].ToString()));
            }
            else
            {
                WeekendDuration = 2;
            }
            OnPropertyChanged("WeekendDuration");
            if (listDay.Contains(EndOfWeekDay))
            {
                EventDate = TimeCounterHelper.getNextWeekend(listDay.IndexOf(EndOfWeekDay), EndOfWeekTime);
            }
            else
            {
                Debugger.Log(3, "error", "bad day of week" + EndOfWeekDay);
            }
        }
    }
}
