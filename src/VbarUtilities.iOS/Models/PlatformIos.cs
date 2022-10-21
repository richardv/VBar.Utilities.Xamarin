using VBarUtilities.iOS.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformIos))]

namespace VBarUtilities.iOS.Models
{
    using System.Threading.Tasks;
    using VBarUtilities.Models;

    public class PlatformIos : IPlatform
    {
        public bool SaveFlightDetails => false;

        public Task OpenFolderLocalState()
        {
            return Task.CompletedTask;
        }
    }
}