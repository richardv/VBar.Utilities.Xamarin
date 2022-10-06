using VBarUtilities.UWP;
using VBarUtilities.UWP.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Platform))]
namespace VBarUtilities.UWP
{
    using System;
    using System.Threading.Tasks;
    using VBarUtilities.Models;
    using Xamarin.Essentials;

    public class Platform : IPlatform
    {
        public bool SaveFlightDetails => true;

        public async Task OpenFolderLocalState()
        {
            await Windows.System.Launcher.LaunchFolderPathAsync(FileSystem.AppDataDirectory);
        }
    }
}
