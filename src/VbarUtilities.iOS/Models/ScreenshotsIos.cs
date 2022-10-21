using VBarUtilities.iOS.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenshotsIos))]

namespace VBarUtilities.iOS.Models
{
    using System.Threading.Tasks;
    using VBarUtilities.Models;

    public class ScreenshotsIos : IScreenshots
    {
        public bool Exists(string name)
        {
            return false;
        }

        public void Save(string name, string data)
        {
            
        }

        public Task OpenFolder()
        {
            return Task.CompletedTask;
        }
    }
}