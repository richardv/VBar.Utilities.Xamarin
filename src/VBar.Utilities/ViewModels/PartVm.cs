namespace VBarUtilities.ViewModels
{
    using System;
    using Xamarin.Forms;

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

        public string FlightsRemaining
        {
            get
            {
                if (RemovedDate != null)
                {
                    return $"Removed {Fmt.ShortDay(RemovedDate.Value)}";
                }

                return FlightLife == 0 ? "" : Fmt.No(FlightLife - Flights) + " flights remaining";
            }
        }

        public string InstallDateFmt => Fmt.ShortDay(InstallDate);

        public string Days
        {
            get
            {
                if (RemovedDate != null)
                {
                    return Math.Round((RemovedDate.Value - InstallDate).TotalDays) + " days";
                }

                return Math.Round((DateTime.Now - InstallDate).TotalDays) + " days";
            }
        }

        public string FlightLifeFmt => FlightLife < 1 ? "" : "Life: " + Fmt.Flights(FlightLife);

        public string FlightsFmt => Fmt.Flights(Flights);

        public Color UsedColour
        {
            get
            {
                if (RemovedDate != null)
                    return Color.Black;

                if (FlightLife <= 0)
                    return Color.Transparent;

                if (100 * Flights / FlightLife > 100)
                    return Color.Red;

                if (100 * Flights / FlightLife > 90)
                    return Color.Orange;

                if (100 * Flights / FlightLife > 75)
                    return Color.Yellow;

                return Color.Green;
            }
        }

        public DateTime? RemovedDate { get; set; }
    }
}