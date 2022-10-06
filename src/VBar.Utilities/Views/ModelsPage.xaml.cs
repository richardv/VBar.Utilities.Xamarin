namespace VBarUtilities.Views
{
    using Microsoft.AppCenter.Analytics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModelsPage
    {
        public ModelsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var modelVms = new List<ModelVm>();

            var flights = await App.Database.Flights().ToListAsync();

            var models = await App.Database.Models().ToListAsync();

            foreach (var model in models.OrderByDescending(m => m.LastFlightTime))
            {
                var modelVm = new ModelVm
                {
                    Name = model.Name,
                    TotalFlights = flights.Count(f => f.ModelName == model.Name),
                    TotalDuration = flights.Where(f => f.ModelName == model.Name).Sum(f => f.DurationS),
                    LastFlight = model.LastFlightTime,
                    LastDuration = model.LastFlightDurationS,
                    DeviceId = model.DeviceId
                };
                try
                {
                    modelVm.ImageData = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(model.ImageData.Replace("data:image/png;base64,", ""))));
                }
                catch { }

                modelVms.Add(modelVm);
            }

            ModelsCount.Text = $"{Fmt.No(models.Count)} model{Fmt.Plural(models.Count)} found";

            Models.ItemsSource = modelVms;
        }

        private async void OpenVStabiModels_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud?action=&sort=New+first&start=0&type=&Aid=All+VBC-t&ftime=All+logs&vbar=All+VBar&action=modellist", BrowserLaunchMode.External);
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void View_OnClicked(object sender, EventArgs e)
        {
            var model = (ModelVm)((Button)sender).BindingContext;

            await Shell.Current.GoToAsync(nameof(ModelPage) + "?id=" + model.DeviceId);
        }
    }
}