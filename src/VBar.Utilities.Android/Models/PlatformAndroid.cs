using VBarUtilities.Droid.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformAndroid))]

namespace VBarUtilities.Droid.Models
{
    using System.Threading.Tasks;

    public class PlatformAndroid : IPlatform
    {
        public bool SaveFlightDetails => false;

        public Task OpenFolderLocalState()
        {
            return Task.CompletedTask;
        }
    }
}