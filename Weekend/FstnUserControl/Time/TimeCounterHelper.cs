using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnUserControl
{
    public static class TimeCounterHelper
    {
        public static DateTime getNextWeekend(int day, DateTime hour)
        {
            DateTime today=DateTime.Now;
            int daysUntilWeekend = ((int) day - (int) today.DayOfWeek + 7) % 7;
            today=ChangeTime(today,hour.Hour,hour.Minute,hour.Second,hour.Millisecond);
            DateTime retour = today.AddDays(daysUntilWeekend);
            return retour;
        }

        public static Boolean IsWeekend(int day, DateTime hour, int weekendDuration)
        {
            DateTime today = DateTime.Now;
            DateTime nextWeekend = getNextWeekend(day, hour);
            DateTime actualWeekend = nextWeekend.AddDays(-7);
            DateTime endNextWeekend = nextWeekend.AddDays(weekendDuration);
            DateTime endActualWeekend = endNextWeekend.AddDays(-7);
            return (nextWeekend<today && today < endNextWeekend)|| (actualWeekend < today && today < endActualWeekend);
        }
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }
    }
}
