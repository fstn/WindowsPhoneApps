using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using AppsRoulet.Resources;
using FstnUserControl;
using GalaSoft.MvvmLight.Command;

namespace AppsRoulet.ViewModel
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public class MainViewModel : ViewModelBase
    {
        public event ChangedEventHandler Changed;
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public String EndOfWeek { get; set; }
        private DateTime endOfWeekTime;

        public String ResultText { get; set; }

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

        private String weekendDuration;

        public String WeekendDuration
        {
            get { return weekendDuration; }
            set
            {
                if (value != weekendDuration)
                {
                    weekendDuration = value;
                    SaveSettings();
                }
            }
        }


        public DateTime EventDate { get; set; }
        private DispatcherTimer dt;
        private List<String> listDay;


        public List<String> ListDay
        {
            get { return listDay; }
            set { listDay = value; }
        }

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
            ReloadSettings();

        }

        public void SaveSettings()
        {
            if (EndOfWeekTime != null)
            {
                settings.Remove("endTime");
                settings.Add("endTime", EndOfWeekTime);
            }
            if (listDay.IndexOf(EndOfWeek) != -1)
            {
                settings.Remove("lastDayOfWeek");
                settings.Add("lastDayOfWeek", listDay.IndexOf(EndOfWeek));
            }
            if (WeekendDuration != null)
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
            return TimeCounterHelper.IsWeekend(listDay.IndexOf(EndOfWeek), EndOfWeekTime,Convert.ToInt32(weekendDuration));
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
                    EndOfWeek = listDay[((int)settings["lastDayOfWeek"])];
                }
                catch (InvalidCastException e)
                {
                    Debugger.Log(3, "error", "bad lastDayOfWeek " + settings["lastDayOfWeek"]);
                }
            }

            if (settings.Contains("weekendDuration") && settings["weekendDuration"] != null)
            {
                WeekendDuration = (settings["weekendDuration"].ToString());
            }
            else
            {
                WeekendDuration = "2";
            }
            OnPropertyChanged("WeekendDuration");
            if (listDay.Contains(EndOfWeek))
            {
                EventDate = TimeCounterHelper.getNextWeekend(listDay.IndexOf(EndOfWeek), EndOfWeekTime);
            }
            else
            {
                Debugger.Log(3, "error", "bad day of week" + EndOfWeek);
            }
        }
    }
}
