namespace VBarUtilities.ViewModels
{
    using System;

    public class FlightSummary
    {
        public string Name { get; set; }

        public int TotalFlights { get; set; }

        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }

        public double Duration { get; set; }
    }
}