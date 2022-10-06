namespace VBarUtilities.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    internal class LongDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Fmt.Day((DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
