using System;
using System.Globalization;
using System.Windows.Data;

namespace HealthCare.Application.Common.Converter
{
    public class RadioBoolToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int integer = (int)value;
            return integer == int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}