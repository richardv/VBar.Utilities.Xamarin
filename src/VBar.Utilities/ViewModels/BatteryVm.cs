namespace VBarUtilities.ViewModels
{
    using System;

    public class BatteryVm
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public int TotalFlights { get; set; }

        public double TotalDuration { get; set; }

        public DateTime? LastFlight { get; set; }

        public double LastDuration { get; set; }

        public DateTime FirstFlight { get; set; }
    }
}