using VBarUtilities.UWP.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveWindows))]
namespace VBarUtilities.UWP.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using VBarUtilities.Models;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Xamarin.Forms;

    class SaveWindows : ISave
    {
        public async Task SaveAndView(string filename, string contentType, MemoryStream stream)
        {
            if (Device.Idiom != TargetIdiom.Desktop)
            {
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile outFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (Stream outStream = await outFile.OpenStreamForWriteAsync())
                {
                    outStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }
                if (contentType != "application/html")
                    await Windows.System.Launcher.LaunchFileAsync(outFile);
            }
            else
            {
                StorageFile storageFile = null;
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Downloads;
                savePicker.SuggestedFileName = filename;
                switch (contentType)
                {
                    case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                        savePicker.FileTypeChoices.Add("PowerPoint Presentation", new List<string>() { ".pptx", });
                        break;

                    case "application/msexcel":
                        savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx", });
                        break;

                    case "application/msword":
                        savePicker.FileTypeChoices.Add("Word Document", new List<string>() { ".docx" });
                        break;

                    case "application/pdf":
                        savePicker.FileTypeChoices.Add("Adobe PDF Document", new List<string>() { ".pdf" });
                        break;
                    case "application/html":
                        savePicker.FileTypeChoices.Add("HTML Files", new List<string>() { ".html" });
                        break;
                    case "text/csv":
                        savePicker.FileTypeChoices.Add("CSV Files", new List<string>() { ".csv" });
                        break;
                }
                storageFile = await savePicker.PickSaveFileAsync();

                if (storageFile == null)
                {
                    return;
                }

                using (Stream outStream = await storageFile.OpenStreamForWriteAsync())
                {
                    outStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }

                //Invoke the saved file for Viewing.
                await Windows.System.Launcher.LaunchFileAsync(storageFile);
            }
        }

        public async Task Save(string filename, string contentType, MemoryStream stream)
        {
            if (Device.Idiom != TargetIdiom.Desktop)
            {
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile outFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (Stream outStream = await outFile.OpenStreamForWriteAsync())
                {
                    outStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }
                if (contentType != "application/html")
                    await Windows.System.Launcher.LaunchFileAsync(outFile);
            }
            else
            {
                StorageFile storageFile = null;
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Downloads;
                savePicker.SuggestedFileName = filename;
                switch (contentType)
                {
                    case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                        savePicker.FileTypeChoices.Add("PowerPoint Presentation", new List<string>() { ".pptx", });
                        break;

                    case "application/msexcel":
                        savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx", });
                        break;

                    case "application/msword":
                        savePicker.FileTypeChoices.Add("Word Document", new List<string>() { ".docx" });
                        break;

                    case "application/pdf":
                        savePicker.FileTypeChoices.Add("Adobe PDF Document", new List<string>() { ".pdf" });
                        break;
                    case "application/html":
                        savePicker.FileTypeChoices.Add("HTML Files", new List<string>() { ".html" });
                        break;
                    case "application/json":
                        savePicker.FileTypeChoices.Add("JSON Files", new List<string>() { ".json" });
                        break;
                }
                storageFile = await savePicker.PickSaveFileAsync();

                if (storageFile == null)
                {
                    return;
                }

                using (Stream outStream = await storageFile.OpenStreamForWriteAsync())
                {
                    outStream.SetLength(0);

                    outStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }
            }
        }

        public async Task<string> Open()
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".json");

            var file = await picker.PickSingleFileAsync();

            if (file == null)
            {
                return null;
            }

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
