namespace VBarUtilities
{
    using System;

    public static class Fmt
    {
        public static string No(int value)
        {
            return value.ToString("#,##0");
        }

        public static string MAh(int value)
        {
            return value.ToString("#,##0") + " mAh";
        }

        public static string No(double value)
        {
            return value.ToString("#,##0");
        }

        public static string DateTime(in DateTime? date)
        {
            if (date == null) return "";

            return date.Value.ToString("d MMM yyyy HH:mm");
        }

        public static string TimeS(in double? seconds)
        {
            if (seconds == null)
            {
                return string.Empty;
            }

            var timeSpan = TimeSpan.FromSeconds(Math.Floor(seconds.Value));

            var response = string.Empty;

            if (timeSpan.TotalHours > 1)
            {
                response += $"{Math.Floor(timeSpan.TotalHours)}h ";
            }

            if (timeSpan.TotalMinutes > 1)
            {
                response += $"{timeSpan.Minutes}m ";
            }

            response += $"{timeSpan.Seconds}s";

            return response;
        }

        public static string LastUsed(DateTime dateTime)
        {
            var days = (int)Math.Round((System.DateTime.Now - dateTime).TotalDays);

            return "last used " + days + " day" + Plural(days) + " ago";
        }

        public static string FirstUsed(DateTime dateTime)
        {
            if (dateTime == System.DateTime.MinValue)
            {
                return string.Empty;
            }

            var days = (int)Math.Round((System.DateTime.Now - dateTime).TotalDays);

            return "First used " + Fmt.No(days) + " day" + Plural(days) + " ago";
        }

        public static string Plural(int qty)
        {
            return qty == 1 ? string.Empty : "s";
        }

        internal static string Day(DateTime day)
        {
            return day.ToString("ddd, d MMMM yyyy");
        }

        internal static string ShortDay(DateTime day)
        {
            return day.ToString("d MMM yyyy");
        }

        internal static string DayOfWeek(int day)
        {
            switch (day)
            {
                case 0:
                case 7:
                    return "Sunday";
                case 1:
                    return "Monday";
                case 2:
                    return "Tuesday";
                case 3:
                    return "Wednesday";
                case 4:
                    return "Thursday";
                case 5:
                    return "Friday";
                case 6:
                    return "Saturday";
                default:
                    return "Unknown";
            };
        }

        public static string Voltage(double voltage)
        {
            return voltage.ToString("0.0") + " V";
        }

        public static string Watts(int watts)
        {
            return No(watts) + " W";
        }

        public static string Amps(double amps)
        {
            return amps.ToString("0.0") + " A";
        }

        public static string Cells(int cells)
        {
            return No(cells) + "S";
        }

        public static string Flights(int flights)
        {
            return No(flights) + " flights";
        }

        public static string Year(DateTime date)
        {
            return date.Year.ToString();
        }

        public static string Month(DateTime date)
        {
            return date.ToString("MMM yyyy");
        }

        public static string Days(int days)
        {
            return days + " days";
        }

        public static string Perc(double value)
        {
            return Math.Round(value * 100) + "%";
        }
    }
}
