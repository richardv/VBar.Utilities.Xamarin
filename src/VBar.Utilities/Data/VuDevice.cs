namespace VBarUtilities.Data
{
    using SQLite;

    public class VuDevice
    {
        [PrimaryKey]
        public string SerialNo { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public int Type { get; set; }

        public bool Update { get; set; }

        public string SId { get; set; }
    }
}