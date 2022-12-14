namespace VBarUtilities.Data
{
    using System;
    using SQLite;

    public class VuFlightDetail
    {
        [PrimaryKey]
        public int FlightNo { get; set; }

        public string BatteryName { get; set; }

        public int Capacity { get; set; }

        public int CapacityUsed { get; set; }

        public double VoltStart { get; set; }

        public double VoltMin { get; set; }

        public double VoltEnd { get; set; }

        public double AmpsMax { get; set; }

        public string EventLogs { get; set; }

        public string BatteryLogs { get; set; }

        public string CustomLogs { get; set; }

        public DateTime Date { get; set; }

        public double DurationS { get; set; }

        public string Model { get; set; }
    }
}
