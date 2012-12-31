using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public static class DateTimeExtensions
    {
        public static DateTime Interval(this DateTime date, DateTimePart? intervalType, int? intervalVal)
        {
            if (intervalType.HasValue && intervalVal.HasValue)
            {
                switch (intervalType.Value)
                {
                    case DateTimePart.Year:
                        return date.AddYears(intervalVal.Value);
                    case DateTimePart.Month:
                        return date.AddMonths(intervalVal.Value);
                    case DateTimePart.Day:
                        return date.AddDays((double)intervalVal.Value);
                    case DateTimePart.Hour:
                        return date.AddHours((double)intervalVal.Value);
                    case DateTimePart.Munite:
                        return date.AddMinutes((double)intervalVal.Value);
                    case DateTimePart.Second:
                        return date.AddSeconds((double)intervalVal.Value);
                    case DateTimePart.Week:
                        return date.AddDays((double)intervalVal.Value * 7);
                    case DateTimePart.Quarter:
                        return date.AddMonths(intervalVal.Value * 3);
                }
            }
            return date;
        }
    }
}
