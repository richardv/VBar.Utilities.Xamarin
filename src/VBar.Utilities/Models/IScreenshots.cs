namespace VBarUtilities.Models
{
    using System.Threading.Tasks;

    public interface IScreenshots
    {
        bool Exists(string name);

        void Save(string name, string data);

        Task OpenFolder();
    }
}