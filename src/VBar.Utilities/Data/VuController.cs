namespace VBarUtilities.Data
{
    using SQLite;

    public class VuController
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageData { get; set; }

        public string SoftwareId { get; set; }
    }
}