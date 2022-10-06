using VBarUtilities.Droid.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenshotsAndroid))]

namespace VBarUtilities.Droid.Models
{
    using System.Threading.Tasks;

    public class ScreenshotsAndroid : IScreenshots
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