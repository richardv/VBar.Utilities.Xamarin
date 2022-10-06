using VBarUtilities.UWP.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClassicImportUwp))]
namespace VBarUtilities.UWP.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.Storage.Pickers;
    using VBarUtilities.Models;
    using VBarUtilities.UWP.Database;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;

    internal class ClassicImportUwp : IClassicImport
    {
        public async Task<IList<VbcFlight>> Logs()
        {
            var results = new List<VbcFlight>();

            var filePicker = new FileOpenPicker();

            filePicker.FileTypeFilter.Add("*");
            filePicker.SuggestedStartLocation = PickerLocationId.HomeGroup;
            var file = await filePicker.PickSingleFileAsync();

            if (file == null)
            {
                return null;
            }

            var targetFile = Path.Combine(FileSystem.CacheDirectory, ".vcontrol.db");
            var targetStream = File.Create(targetFile);

            new StreamReader(await file.OpenStreamForReadAsync()).BaseStream.CopyTo(targetStream);
            targetStream.Close();

            using (var db = new ClassicDb(targetFile))
            {
                var models = await db.Models().ToListAsync();
                var batteryLogs = await db.BatteryLogs().ToListAsync();

                foreach (var batteryLog in batteryLogs)
                {
                    var model = models.Single(m => m.id == batteryLog.modelid);

                    var vbcFlight = new VbcFlight
                    {
                        Model = model.name,
                        Duration = batteryLog.duration,
                        FlightNo = batteryLog.id.ToString(),
                        Date = DateTime.Parse(batteryLog.date),
                    };

                    results.Add(vbcFlight);
                }
            };

            return results;
        }
    }
}
