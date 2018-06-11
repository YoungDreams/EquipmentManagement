using System;

namespace PPM.Web.Common
{
    public static class Formats
    {
        private const string DateStringFormat = "yyyy-MM-dd";
        private const string DateTimeStringFormat = "yyyy-MM-dd HH:mm";

        public static string DateString(this DateTime datetime)
        {
            return datetime.ToString(DateStringFormat);
        }

        public static string DateString(this DateTime? datetime)
        {
            return datetime?.ToString(DateStringFormat);
        }

        public static string DateTimeString(this DateTime datetime)
        {
            return datetime.ToString(DateTimeStringFormat);
        }

        public static string DatePickerString(this DateTime? datetime)
        {
            return datetime?.ToString("yyyy-MM-dd");
        }
    }
}