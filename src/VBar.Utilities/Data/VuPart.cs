namespace VBarUtilities.Data
{
    using SQLite;
    using System;

    public class VuPart
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string ModelName { get; set; }

        public string PartNo { get; set; }

        public string Name { get; set; }

        public DateTime InstallDate { get; set; }

        public int FlightLife { get; set; }

        public DateTime? RemovedDate { get; set; }
    }
}
