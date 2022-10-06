namespace VBarUtilities.Models
{
    using System.Threading.Tasks;

    public interface IPlatform
    {
        bool SaveFlightDetails { get; }

        Task OpenFolderLocalState();
    }
}
