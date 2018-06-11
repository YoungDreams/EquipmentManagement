using System;

namespace Foundation.Core
{
    public static class DateTimeExtensions
    {
        public static bool IsInSameMonth(this DateTime @this, DateTime month)
        {
            return @this.Year == month.Year && @this.Month == month.Month;
        }

        public static bool IsInSameMonth(this DateTime? @this, DateTime month)
        {
            return @this.HasValue && @this.Value.IsInSameMonth(month);
        }

        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }

        public static DateTime ToFullDay(this DateTime @this)
        {
            return @this.AddDays(1).AddSeconds(-1);
        }

        public static int ToAge(this DateTime? @this)
        {
            if (!@this.HasValue)
            {
                return 0;
            }

            DateTime now = DateTime.Today;
            int age = now.Year - @this.Value.Year;
            if (now < @this.Value.AddYears(age)) age--;
            return age;
        }

        public static bool IsOverFisrtDayOfTwelve(this DateTime @this)
        {
            var now = DateTime.Now;
            if (now.Hour < 12 && now.Day == 1)
            {
                return false;
            }
            return true;
        }

        public static bool IsOverNineHour(this DateTime @this)
        {
            var now = DateTime.Now;
            if (now.Hour < 9)
            {
                return false;
            }
            return true;
        }

        public static bool IsOverTwelveHour(this DateTime @this)
        {
            var now = DateTime.Now;
            if (now.Hour < 12)
            {
                return false;
            }
            return true;
        }

        public static bool IsRangePreviousMonthFromNow(this DateTime @this)
        {
            var now = DateTime.Now.Date;
            var firstDayOfMonth = now.AddDays(1 - DateTime.Now.Day);
            var previousMonth = firstDayOfMonth.AddMonths(-1);
            return @this >= previousMonth && @this <= now;
        }

        public static bool IsRangeCurrentMonthFromNow(this DateTime @this)
        {
            var now = DateTime.Now.Date;
            var firstDayOfMonth = now.AddDays(1 - DateTime.Now.Day);
            return @this >= firstDayOfMonth && @this <= now;
        }

        public static bool IsRangePreviousMonthFirstDay(this DateTime @this)
        {
            var now = DateTime.Now.Date;
            var firstDayOfMonth = now.AddDays(1 - DateTime.Now.Day);
            var previousMonth = firstDayOfMonth.AddMonths(-1);
            return @this >= previousMonth;
        }

        public static bool IsRangeCurrentMonthFirstDay(this DateTime @this)
        {
            var now = DateTime.Now.Date;
            var firstDayOfMonth = now.AddDays(1 - DateTime.Now.Day);
            return @this >= firstDayOfMonth;
        }


        public static int DateDiff(this DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            return Math.Abs(ts.Days);
        }


    }
}
