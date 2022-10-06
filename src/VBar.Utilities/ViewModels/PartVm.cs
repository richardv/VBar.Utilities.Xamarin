namespace VBarUtilities.ViewModels
{
    using System;

    internal class PartVm
    {
        public Guid Id { get; set; }

        public string ModelName { get; set; }

        public string PartNo { get; set; }

        public string Name { get; set; }

        public int FlightLife { get; set; }

        public DateTime InstallDate { get; set; }

        public int Flights { get; set; }

        public double Duration { get; set; }

        public string UsedPerc => FlightLife == 0 ? "" : Fmt.Perc((double)Flights / FlightLife) + " used";

        public string FlightsRemaining => FlightLife == 0 ? "" : Fmt.No(FlightLife - Flights) + " flights remaining";

        public string InstallDateFmt => Fmt.ShortDay(InstallDate);

        public string Days => Math.Round((DateTime.Now - InstallDate).TotalDays) + " days";

        public string FlightLifeFmt => FlightLife < 1 ? "" : "Life: " + Fmt.Flights(FlightLife);

        public string FlightsFmt => Fmt.Flights(Flights);
    }
}