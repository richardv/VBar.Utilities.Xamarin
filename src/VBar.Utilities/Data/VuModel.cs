namespace VBarUtilities.Data
{
    using SQLite;
    using System;

    public class VuModel
    {
        public string Name { get; set; }

        public string ImageData { get; set; }

        public string LastFlightNo { get; set; }

        public DateTime? LastFlightTime { get; set; }

        public double? LastFlightDurationS { get; set; }

        public string ControllerId { get; set; }

        [PrimaryKey]
        public string DeviceId { get; set; }
    }
}