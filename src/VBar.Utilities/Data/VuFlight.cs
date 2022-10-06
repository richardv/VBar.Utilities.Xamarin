namespace VBarUtilities.Data
{
    using SQLite;
    using System;

    public class VuFlight
    {
        [PrimaryKey]
        public DateTime DateAndTime { get; set; }

        public int FlightNo { get; set; }

        public string ModelName { get; set; }

        public double DurationS { get; set; }

        public string ControllerId { get; set; }

        public string DeviceId { get; set; }
    }
}