﻿namespace VBarUtilities.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    internal class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Fmt.TimeS((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
