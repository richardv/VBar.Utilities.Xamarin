using VBarUtilities.UWP.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Screenshots))]
namespace VBarUtilities.UWP.Models
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using VBarUtilities.Models;
    using Xamarin.Essentials;

    public class Screenshots : IScreenshots
    {
        private string Folder()
        {
            var folder = Path.Combine(FileSystem.CacheDirectory, "Screenshots");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        public bool Exists(string name)
        {
            var path = Path.Combine(Folder(), name);

            return File.Exists(path);
        }

        public void Save(string name, string data)
        {
            var imageData = new MemoryStream(Convert.FromBase64String(data.Replace("data:image/png;base64,", "")));

            var path = Path.Combine(Folder(), name);

            File.WriteAllBytes(path, imageData.ToArray());
        }

        public async Task OpenFolder()
        {
            await Windows.System.Launcher.LaunchFolderPathAsync(Folder());
        }
    }
}
