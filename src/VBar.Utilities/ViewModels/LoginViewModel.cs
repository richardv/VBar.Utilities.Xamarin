namespace VBarUtilities.ViewModels
{
    using AutoMapper;
    using Microsoft.AppCenter.Analytics;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using VBarUtilities.Data;
    using VBarUtilities.Views;
    using VStabiParser;
    using VStabiParser.Models;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        private string username;
        private string password;
        private string status;
        private double progress;
        private int startYear;
        private bool canSaveCredentials;

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool CanSaveCredentials
        {
            get => canSaveCredentials; set
            {
                canSaveCredentials = value;
                OnPropertyChanged(nameof(CanSaveCredentials));
            }
        }

        public Command LoginCommand { get; }

        public string Status
        {
            get
            {
                return status;
            }
            private set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public double Progress
        {
            get
            {
                return progress;
            }
            private set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public int StartYear
        {
            get
            {
                return startYear;
            }
            set
            {
                startYear = value;
                OnPropertyChanged(nameof(StartYear));
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            InitAsync();
        }

        public async void InitAsync()
        {
            await LoadCredentials();
        }

        private async void OnLoginClicked(object obj)
        {
            Analytics.TrackEvent("Login");

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Status = "Complete required fields";
                return;
            }

            Status = "Logging in";
            Progress = 0.1;

            var _mapper = CreateMapper();

            await SaveCredentials();

            var cookie = new Cookie
            {
                Name = username,
                Value = password,
                Path = "/",
                Domain = ".vstabi.info"
            };

            //var reader = new VStabiCloudReader.VStabiCloudReader(cookie);

            var reader = new VStabiCloudReader.VStabiCloudReader(username, Password);

            var parser = new VStabiParser(reader);

            try
            {
                // Controllers
                Status = "Loading Controllers";
                Progress = 0.2;

                var vstabiControllers = await parser.Controllers();

                await App.Database.ClearControllers();

                foreach (var vstabiController in vstabiControllers)
                {
                    await App.Database.InsertControllerAsync(_mapper.Map<VuController>(vstabiController));
                }

                // Models
                Status = "Loading Models";
                Progress = 0.3;

                var controller = vstabiControllers[0];

                var vstabiModels = await parser.Models(controller.SoftwareId);

                await App.Database.ClearModels();

                foreach (var vstabiModel in vstabiModels)
                {
                    await App.Database.InsertModelAsync(_mapper.Map<VuModel>(vstabiModel));
                }

                // Batteries
                Status = "Loading Batteries";
                Progress = 0.4;

                var vstabiBatteries = await parser.Batteries(controller.SoftwareId);

                await App.Database.ClearBatteries();

                foreach (var vstabiBattery in vstabiBatteries)
                {
                    var vuBattery = _mapper.Map<VuBattery>(vstabiBattery);

                    // Fix for vstabi site
                    vuBattery.StoreAging = vstabiBattery.FlightAging;
                    vuBattery.FlightAging = vstabiBattery.StoreAging;
                    // end fix

                    vuBattery.Flights = vstabiBattery.BatteryFlights.Count(bf => bf.DurationS >= 60);

                    var firstFlight = await App.Database.FlightDetails()
                        .Where(d => d.BatteryName == vstabiBattery.Name)
                        .OrderBy(d => d.Date)
                        .FirstOrDefaultAsync();

                    if (firstFlight != null)
                    {
                        vuBattery.FirstFlight = firstFlight.Date;
                    }

                    if (App.Platform.SaveFlightDetails)
                    {
                        foreach (var batteryFlight in vstabiBattery.BatteryFlights.Where(bf => bf.DurationS >= 60))
                        {
                            Status = "Loading Flight " + batteryFlight.FlightNo;

                            var vuFlightDetails = await App.Database.GetFlightDetail(batteryFlight.FlightNo);

                            if (vuFlightDetails == null)
                            {
                                vuFlightDetails = new VuFlightDetail
                                {
                                    AmpsMax = batteryFlight.AmpsMax,
                                    VoltEnd = batteryFlight.VoltEmpty,
                                    VoltMin = batteryFlight.VoltMin,
                                    FlightNo = batteryFlight.FlightNo,
                                    Capacity = batteryFlight.Capacity,
                                    CapacityUsed = batteryFlight.CapacityUsed,
                                    DurationS = batteryFlight.DurationS,
                                    Model = batteryFlight.Model,
                                    Date = batteryFlight.DateTime,
                                    BatteryName = vstabiBattery.Name
                                };

                                await App.Database.AddFlightDetail(vuFlightDetails);
                            }
                        }
                    }

                    await App.Database.InsertBatteryAsync(vuBattery);
                }

                // Devices
                Status = "Loading Devices";
                Progress = 0.5;

                var vstabiDevices = await parser.Devices();

                await App.Database.ClearDevices();

                foreach (var vstabiDevice in vstabiDevices)
                {
                    await App.Database.InsertDeviceAsync(_mapper.Map<VuDevice>(vstabiDevice));
                }

                // Setups
                Status = "Loading Setups";
                Progress = 0.6;

                var vstabiSetups = await parser.Setups();

                await App.Database.ClearSetups();

                foreach (var vstabiSetup in vstabiSetups)
                {
                    await App.Database.InsertSetupAsync(_mapper.Map<VuSetup>(vstabiSetup));
                }

                // Screenshots
                Status = "Loading Screenshots";
                Progress = 0.7;

                uint page = 0;
                var complete = false;

                if (App.Screenshots != null)
                {
                    do
                    {
                        Status = $"Loading Screenshots Page {page}";

                        var vstabiScreenshots = await parser.Screenshots(controller.SoftwareId, page);

                        if (vstabiScreenshots.Count == 0)
                        {
                            break;
                        }

                        foreach (var vstabiScreenshot in vstabiScreenshots)
                        {
                            if (vstabiScreenshot.Date.Year < StartYear)
                            {
                                complete = true;
                                break;
                            }

                            if (App.Screenshots.Exists(vstabiScreenshot.Name))
                            {
                                complete = true;
                                break;
                            }

                            App.Screenshots.Save(vstabiScreenshot.Name, vstabiScreenshot.ImageData);
                        }

                        page += 1;
                    } while (!complete);
                }

                // Flights
                Status = "Loading Flights";
                Progress = 0.8;

                var lastFlightNo = 0;
                page = 0;
                complete = false;

                var lastFlight = await App.Database.Flights().OrderByDescending(f => f.FlightNo).FirstOrDefaultAsync();

                if (lastFlight != null)
                {
                    lastFlightNo = lastFlight.FlightNo;
                }

                do
                {
                    Status = $"Loading Flights Page {page}";

                    var vstabiFlights = await parser.Flights(controller.SoftwareId, page);

                    if (vstabiFlights.Count == 0)
                    {
                        break;
                    }

                    foreach (var vstabiFlight in vstabiFlights.Where(f => f.DurationS >= 60))
                    {
                        if (vstabiFlight.FlightNo <= lastFlightNo || vstabiFlight.DateAndTime.Year < StartYear)
                        {
                            complete = true;
                            break;
                        }

                        try
                        {
                            await App.Database.InsertFlightAsync(_mapper.Map<VuFlight>(vstabiFlight));
                        }
                        catch (Exception ex)
                        {
                            //throw;
                        }
                    }

                    page += 1;
                } while (!complete);

            }
            catch (Exception ex)
            {
                Status = ex.Message;
                Progress = 100;
                return;
            }

            Progress = 100;
            Status = "Complete";

            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VStabiController, VuController>();
                cfg.CreateMap<VStabiSetup, VuSetup>();
                cfg.CreateMap<VStabiDevice, VuDevice>();
                cfg.CreateMap<VStabiBattery, VuBattery>();
                cfg.CreateMap<VStabiModel, VuModel>();
                cfg.CreateMap<VStabiFlight, VuFlight>();
            });

            return mapperConfiguration.CreateMapper();
        }

        private async Task LoadCredentials()
        {
            Username = await SecureStorage.GetAsync("Username");
            Password = await SecureStorage.GetAsync("Password");
            var CanSaveCredentialsString = await SecureStorage.GetAsync("CanSaveCredentials");

            if (bool.TryParse(CanSaveCredentialsString, out var CanSaveCredentialsBool))
            {
                CanSaveCredentials = CanSaveCredentialsBool;
            }

            if (int.TryParse(await SecureStorage.GetAsync("StartYear"), out int startYear))
            {
                StartYear = startYear;
            }
            else
            {
                StartYear = DateTime.Now.Year;
            }
        }

        private async Task SaveCredentials()
        {
            if (CanSaveCredentials)
            {
                await SecureStorage.SetAsync("Username", Username);
                await SecureStorage.SetAsync("Password", Password);
            }
            else
            {
                SecureStorage.Remove("Username");
                SecureStorage.Remove("Password");
            }

            await SecureStorage.SetAsync("CanSaveCredentials", CanSaveCredentials.ToString());
            await SecureStorage.SetAsync("StartYear", StartYear.ToString());
        }
    }
}
