using VBarUtilities.iOS.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveIos))]

namespace VBarUtilities.iOS.Models
{
    using System.IO;
    using System.Threading.Tasks;
    using VBarUtilities.Models;

    public class SaveIos : ISave
    {
        public Task SaveAndView(string filename, string contentType, MemoryStream stream)
        {
            return Task.CompletedTask;
        }

        public Task Save(string filename, string contentType, MemoryStream stream)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Open()
        {
            throw new System.NotImplementedException();
        }
    }
}