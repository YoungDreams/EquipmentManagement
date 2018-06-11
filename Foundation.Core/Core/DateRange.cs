using System;
using System.Collections.Generic;

namespace Foundation.Core
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange(DateTime start,DateTime end)
        {
            Start = start;
            End = end;
        }

        public static DateRange OfMonth(int year, int month)
        {
            return new DateRange(new DateTime(year,month,1), new DateTime(year, month, 1).AddMonths(1).AddMilliseconds(-1));
        }

        public bool IsInclude(DateTime dateTime)
        {
            return dateTime >= Start && dateTime <= End;
        }

        public IDictionary<int, List<int>> GroupByYearForMonth()
        {
            var result = new Dictionary<int, List<int>>();

            for (var current = Start; current <= End; current = current.AddMonths(1))
            {
                if (!result.ContainsKey(current.Year))
                {
                    result.Add(current.Year,new List<int>());
                }

                result[current.Year].Add(current.Month);
            }

            return result;
        }
    }
}
