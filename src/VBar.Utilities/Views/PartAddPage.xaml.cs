namespace VBarUtilities.Views
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AppCenter.Analytics;
    using VBarUtilities.Data;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("model", "model")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartAddPage : ContentPage
    {
        public string model { get; set; }

        public PartAddPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var models = await App.Database.Models().ToListAsync();

            Models.Items.Clear();

            foreach (var model in models.OrderBy(m => m.Name))
            {
                Models.Items.Add(model.Name);
            }

            Models.SelectedItem = model;

            FlightLife.Text = "500";
            InstallDate.Date = DateTime.Today;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var part = new VuPart
            {
                Id = Guid.NewGuid(),
                ModelName = Models.SelectedItem.ToString(),
                PartNo = PartNo.Text,
                Name = Name.Text,
                FlightLife = int.Parse(FlightLife.Text),
                InstallDate = InstallDate.Date
            };

           await App.Database.InsertPartAsync(part);

           await ReturnToParts();
        }

        private async Task ReturnToParts()
        {
            await Shell.Current.GoToAsync("//" + nameof(PartsPage) + "?model=" + model);
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await ReturnToParts();
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }
    }
}