using VBarUtilities.Droid.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformAndroid))]

namespace VBarUtilities.Droid.Models
{
    using System.Threading.Tasks;
    using VBarUtilities.Models;

    public class PlatformAndroid : IPlatform
    {
        public bool SaveFlightDetails => false;

        public Task OpenFolderLocalState()
        {
            return Task.CompletedTask;
        }
    }
}