namespace VBarUtilities.Views
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("id", "id")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartEditPage
    {
        public string id { get; set; }

        public PartEditPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var models = await App.Database.Models().ToListAsync();

            foreach (var model in models.OrderBy(m => m.Name))
            {
                ModelName.Items.Add(model.Name);
            }

            var partId = Guid.Parse(id);

            var part = await App.Database.GetPart(partId);

            ModelName.SelectedItem = part.ModelName;
            PartNo.Text = part.PartNo;
            Name.Text = part.Name;
            InstallDate.Date = part.InstallDate.Date;
            FlightLife.Text = part.FlightLife.ToString();

            if (part.RemovedDate != null)
            {
                Removed.IsChecked = true;
                RemovedDate.Date = part.RemovedDate.Value;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var partId = Guid.Parse(id);

            var part = await App.Database.GetPart(partId);

            part.ModelName = ModelName.SelectedItem.ToString();
            part.PartNo = PartNo.Text;
            part.Name = Name.Text;
            part.InstallDate = InstallDate.Date;
            part.FlightLife = int.Parse(FlightLife.Text);

            if (Removed.IsChecked)
            {
                part.RemovedDate = RemovedDate.Date;
            }
            else
            {
                part.RemovedDate = null;
            }

            await App.Database.UpdatePartAsync(part);

            await ReturnToParts();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await ReturnToParts();
        }

        private async Task ReturnToParts()
        {
            await Shell.Current.GoToAsync("//" + nameof(PartsPage) + "?model=" + ModelName.SelectedItem);
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private void Removed_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RemovedDate.IsEnabled = e.Value;
        }
    }
}