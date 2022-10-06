namespace VBarUtilities.UWP
{
    using SQLite;

    public class model
    {
        [PrimaryKey]
        [AutoIncrement]
        public int id { get; set; }

        [MaxLength(20)]
        public string type { get; set; }

        public string name { get; set; }

        public byte[] image { get; set; }

        public byte[] thumb { get; set; }

        public int info { get; set; }
    }
}