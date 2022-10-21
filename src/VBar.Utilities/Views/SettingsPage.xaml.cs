namespace VBarUtilities.Views
{
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ResetDatabaseClick(object sender, EventArgs e)
        {
            var resetDatabase = await DisplayAlert("Reset Database", "Are you sure you want to reset the database?", "OK", "Cancel");

            if (resetDatabase)
            {
                await App.Database.ResetDatabase();
            }

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void DataFolder_Clicked(object sender, EventArgs e)
        {
           await App.Platform.OpenFolderLocalState();
        }
    }
}