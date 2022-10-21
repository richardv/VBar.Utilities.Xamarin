using VBarUtilities.iOS.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClassicImportIos))]

namespace VBarUtilities.iOS.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VBarUtilities.Models;
    using ViewModels;

    public class ClassicImportIos : IClassicImport
    {
        public async Task<IList<VbcFlight>> Logs()
        {
            return new List<VbcFlight>();
        }
    }
}