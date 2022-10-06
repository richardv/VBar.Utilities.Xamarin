namespace VBarUtilities.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    internal class DeviceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 20:
                    return "VBar Control";
                case 23:
                    return "VBar Control Touch";
                case 26:
                    return "VBar NEO";
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
