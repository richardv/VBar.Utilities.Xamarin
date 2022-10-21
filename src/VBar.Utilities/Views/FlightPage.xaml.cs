namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VStabiCloudReader;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("DateAndTime", "DateAndTime")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightPage
    {
        public string DateAndTime { get; set; }

        public FlightPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var dateAndTime = DateTime.Parse(DateAndTime);

            var flight = await App.Database.GetFlight(dateAndTime);

            Date.Text = flight.DateAndTime.ToString("d MMMM yyyy HH:mm");

            Model.Text = flight.ModelName;

            Duration.Text = Fmt.TimeS(flight.DurationS);

            var controller = await App.Database.GetController(flight.ControllerId);

            if (controller != null)
            {
                Controller.Text = controller.Name;
                ControllerImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(controller.ImageData.Replace("data:image/png;base64,", ""))));
            }

            var device = await App.Database.GetDevice(flight.DeviceId);

            if (device != null)
            {
                Device.Text = device.Name;
            }

            var model = await App.Database.GetModel(flight.DeviceId);

            if (model != null)
            {
                ModelImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(model.ImageData.Replace("data:image/png;base64,", ""))));
            }

            var flightDetail = await App.Database.GetFlightDetail(flight.FlightNo);

            if (flightDetail != null)
            {
                if (flightDetail.EventLogs == null)
                {
                    var username = await SecureStorage.GetAsync("Username");
                    var password = await SecureStorage.GetAsync("Password");

                    var reader = new VStabiCloudReader(username, password);

                    var parser = new VStabiParser.VStabiParser(reader);

                    var devices = await parser.Devices();

                    var vstabiFlightDetail = await parser.FlightDetail(devices.First().SId, flight.FlightNo);

                    flightDetail.EventLogs = vstabiFlightDetail.EventLogs;
                    flightDetail.BatteryLogs = vstabiFlightDetail.BatteryLogs;
                    flightDetail.CustomLogs = vstabiFlightDetail.CustomLogs;
                    flightDetail.VoltStart = vstabiFlightDetail.VoltStart;

                    await App.Database.UpdateFlightDetail(flightDetail);
                }

                BatteryName.Text = flightDetail.BatteryName;
                Capacity.Text = Fmt.MAh(flightDetail.Capacity);
                Used.Text = Fmt.MAh(flightDetail.CapacityUsed);
                StartV.Text = Fmt.Voltage(flightDetail.VoltStart);
                MinV.Text = Fmt.Voltage(flightDetail.VoltMin);
                EndV.Text = Fmt.Voltage(flightDetail.VoltEnd);
                AmpsMax.Text = Fmt.Amps(flightDetail.AmpsMax);
                EventLogs.Text = flightDetail.EventLogs;

                var series1 = new List<NameValue>();
                var series2 = new List<NameValue>();
                var series3 = new List<NameValue>();
                var series4 = new List<NameValue>();
                var series5 = new List<NameValue>();
                var series6 = new List<NameValue>();

                var sr = new StringReader(flightDetail.BatteryLogs);

                var line = sr.ReadLine();
                line = sr.ReadLine();

                while (!string.IsNullOrWhiteSpace(line))
                {
                    var values = line.Split(';');

                    var time = values[0].Substring(11, 8);

                    series1.Add(new NameValue(time, values[1]));
                    series2.Add(new NameValue(time, values[2]));
                    series3.Add(new NameValue(time, values[3]));
                    series4.Add(new NameValue(time, values[4]));
                    series5.Add(new NameValue(time, values[5]));
                    series6.Add(new NameValue(time, values[6]));

                    line = sr.ReadLine();
                }

                Series1.ItemsSource = series1;
                Series2.ItemsSource = series2;
                Series3.ItemsSource = series3;
                Series4.ItemsSource = series4;
                Series5.ItemsSource = series5;
                Series6.ItemsSource = series6;
            }
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }
    }

    public class NameValue
    {
        public NameValue(string name, string value)
        {
            Name = name;
            Value = double.Parse(value);
        }

        public string Name { get; set; }

        public double Value { get; set; }
    }
}