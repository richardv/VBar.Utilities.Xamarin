namespace VBarUtilities.UWP
{
    using SQLite;
    using System;

    public class batterylog
    {
        [PrimaryKey]
        [AutoIncrement]
        public int id { get; set; }

        public string date { get; set; }

        public int batteryid { get; set; }

        public int modelid { get; set; }

        public int duration { get; set; }

        public int capacity { get; set; }

        public int used { get; set; }

        public double minvoltage { get; set; }

        public double maxampere { get; set; }

        public double uid { get; set; }
    }
}