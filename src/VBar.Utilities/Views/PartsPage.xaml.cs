namespace VBarUtilities.Views
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VBarUtilities.Models;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("model", "model")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartsPage
    {
        private object model;

        private List<VuFlight> flights;

        public PartsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var parts = await App.Database.Parts().ToListAsync();

            var models = (from p in parts
                          group p by p.ModelName into g
                          orderby g.Key
                          select g.Key).ToList();

            models.Insert(0, "");

            Models.Items.Clear();

            foreach (var model in models)
            {
                Models.Items.Add(model);
            }

            Models.SelectedItem = model;

            flights = await App.Database.Flights().ToListAsync();

            await LoadParts();
        }

        private async Task LoadParts()
        {
            var allParts = await App.Database.Parts().ToListAsync();

            var parts = from p in allParts
                        where (model == null || model.ToString() == p.ModelName) 
                              && (ShowRemoved.IsChecked || p.RemovedDate == null)
                        orderby p.ModelName, p.PartNo, p.Name
                        select p;

            var partsList = new List<PartVm>();

            foreach (var part in parts)
            {
                var partFlights = flights
                    .Where(f => f.ModelName == part.ModelName && f.DateAndTime >= part.InstallDate && (part.RemovedDate == null || f.DateAndTime <= part.RemovedDate))
                    .ToList();

                var partVm = new PartVm
                {
                    Id = part.Id,
                    ModelName = part.ModelName,
                    PartNo = part.PartNo,
                    Name = part.Name,
                    FlightLife = part.FlightLife,
                    InstallDate = part.InstallDate,
                    RemovedDate = part.RemovedDate,
                    Flights = partFlights.Count,
                    Duration = partFlights.Sum(f => f.DurationS)
                };

                partsList.Add(partVm);
            }

            Parts.ItemsSource = partsList;
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void AddPart_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PartAddPage) + "?model=" + model);
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var id = ((Button)sender).BindingContext;

            await Shell.Current.GoToAsync(nameof(PartEditPage) + "?id=" + id);
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var id = ((Button)sender).BindingContext.ToString();

            if (!Guid.TryParse(id, out Guid partId))
            {
                return;
            }

            var part = await App.Database.GetPart(partId);

            var delete = await DisplayAlert("Remove " + part.Name, "Are you sure you want to remove this part?", "OK", "Cancel");

            if (delete)
            {
                await App.Database.DeletePart(partId);
            }

            await LoadParts();
        }

        private async void BackupParts_Clicked(object sender, EventArgs e)
        {
            using (var stream = new MemoryStream())
            {
                var parts = await App.Database.Parts().ToListAsync();

                var sw = new StreamWriter(stream, Encoding.Unicode);

                await sw.WriteLineAsync(JsonConvert.SerializeObject(parts, Formatting.Indented));

                await sw.FlushAsync();

                await DependencyService.Get<ISave>().Save(
                    "Parts.json",
                    "application/json",
                    stream);

                stream.Close();
            }
        }

        private async void RestoreParts_Clicked(object sender, EventArgs e)
        {
            var json = await DependencyService.Get<ISave>().Open();

            if (json == null)
            {
                return;
            }

            var parts = JsonConvert.DeserializeObject<List<VuPart>>(json);

            if (parts == null)
            {
                return;
            }

            foreach (var part in parts)
            {
                try
                {
                    await App.Database.GetPart(part.Id);

                    await App.Database.UpdatePartAsync(part);
                }
                catch
                {
                    await App.Database.InsertPartAsync(part);
                }
            }

            await LoadParts();
        }

        private async void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            model = Models.SelectedItem;

            await LoadParts();
        }

        private async void RetiredCheckBox_Changed(object sender, CheckedChangedEventArgs e)
        {
           await LoadParts();
        }
    }
}