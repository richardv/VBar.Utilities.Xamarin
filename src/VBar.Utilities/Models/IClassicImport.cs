namespace VBarUtilities.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VBarUtilities.ViewModels;

    public interface IClassicImport
    {
        Task<IList<VbcFlight>> Logs();
    }
}