using System;
using System.Collections.Generic;

namespace HealthCare.DataManagment.Serialize
{
    public static class SerialUtil
    {
        public static T ParseEnum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static string ToString(IEnumerable<string> data, char separator = '|')
        {
            return string.Join(separator, data);
        }
    }
}