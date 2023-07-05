using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LibraryCirculation.DataManagement.Serialize
{
    public static class SerialUtil
    {
        public static T Parse<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static string Join<T>(IEnumerable<T> items, string separator = "/")
        {
            return string.Join(separator, items);
        }

        public static List<T> Split<T>(string str, string separator = "/")
        {
            var conv = TypeDescriptor.GetConverter(typeof(T));
            return str.Split(separator)
                .Select(s => conv.ConvertFromInvariantString(s))
                .Where(o => o != null).Cast<T>().ToList();
        }

        public static List<T> GetRange<T>(IEnumerable<T> items, int start, int end = -1)
        {
            var sliced = items.Skip(start);
            if (end != -1) sliced = sliced.Take(end);
            return sliced.ToList();
        }
    }
}