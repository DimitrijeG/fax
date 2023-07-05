using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LibraryCirculation.Application.Common
{
    public static class Util
    {
        public static DateTime ParseDateTime(string str)
        {
            return DateTime.ParseExact(str, Formats.DateTime, CultureInfo.InvariantCulture);
        }

        public static DateOnly ParseDate(string str)
        {
            return DateOnly.ParseExact(str, Formats.Date, CultureInfo.InvariantCulture);
        }

        public static TimeSpan ParseTimeSpan(string str)
        {
            return TimeSpan.ParseExact(str, Formats.TimeSpan, CultureInfo.InvariantCulture);
        }

        public static TimeOnly ParseTime(string str)
        {
            return TimeOnly.ParseExact(str, Formats.Time, CultureInfo.InvariantCulture);
        }

        public static string ToString(DateTime dt, string format = Formats.DateTime)
        {
            return dt.ToString(format);
        }

        public static string ToString(DateOnly d)
        {
            return d.ToString(Formats.Date);
        }

        public static string ToString(TimeSpan ts)
        {
            return ts.ToString(Formats.TimeSpan);
        }

        public static string ToString(TimeOnly t)
        {
            return t.ToString(Formats.Time);
        }

        public static string ToString(double d)
        {
            return d.ToString(CultureInfo.InvariantCulture);
        }

        public static IEnumerable<(T item, int i)> WithIndex<T>(IEnumerable<T> data)
        {
            return data.Select((item, i) => (item, i));
        }
    }
}