namespace VBarUtilities.Views
{
    using System;
    using Microsoft.AppCenter.Analytics;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void OpenVStabi_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud", BrowserLaunchMode.External);
        }
    }
}