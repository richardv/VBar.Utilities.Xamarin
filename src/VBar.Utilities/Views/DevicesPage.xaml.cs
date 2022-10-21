namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage
    {
        public DevicesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var deviceVms = new List<DeviceViewModel>();

            var devices = await App.Database.Devices().OrderBy(b => b.SerialNo).ToListAsync();

            foreach (var device in devices)
            {
                deviceVms.Add(new DeviceViewModel
                {
                    Name = device.Name,
                    SerialNo = device.SerialNo,
                    Note = device.Note,
                    Update = device.Update,
                    Type = device.Type
                });
            }

            Devices.ItemsSource = deviceVms;

            DevicesCount.Text = $"{Fmt.No(deviceVms.Count)} device{Fmt.Plural(devices.Count)} found";
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void VStabiDevices_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/devices", BrowserLaunchMode.External);
        }
    }
}