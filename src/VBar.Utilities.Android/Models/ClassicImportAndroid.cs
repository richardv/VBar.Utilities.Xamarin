using VBarUtilities.Droid.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClassicImportAndroid))]

namespace VBarUtilities.Droid.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Views;

    public class ClassicImportAndroid : IClassicImport
    {
        public async Task<IList<VbcFlight>> Logs()
        {
            return new List<VbcFlight>();
        }
    }
}