namespace VBarUtilities.Data
{
    using SQLite;
    using System;

    public class VuSetup
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string ControllerId { get; set; }

        public DateTime DateAndTime { get; set; }

        public string FileData { get; set; }
    }
}