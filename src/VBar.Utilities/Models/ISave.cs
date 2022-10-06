namespace VBarUtilities.Models
{
    using System.IO;
    using System.Threading.Tasks;

    public interface ISave
    {
        Task SaveAndView(string filename, string contentType, MemoryStream stream);

        Task Save(string filename, string contentType, MemoryStream stream);

        Task<string> Open();
    }
}
