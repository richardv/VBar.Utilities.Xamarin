namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AppCenter.Analytics;
    using VBarUtilities.Data;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using CheckedChangedEventArgs = Syncfusion.XForms.Buttons.CheckedChangedEventArgs;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightsPage
    {
        private List<VuFlight> flights;

        private readonly FlightsViewModel viewModel;

        private Breakdown Breakdown { get; set; }

        public FlightsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new FlightsViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            flights = await App.Database.Flights().ToListAsync();

            var models = await App.Database.Models().ToListAsync();
            models.Insert(0, new VuModel());

            viewModel.Models.Clear();

            foreach (var model in models.OrderBy(m => m.Name))
            {
                viewModel.Models.Add(model.Name);
            }

            Month_Clicked();
        }

        private async void OpenVStabiFlights_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud?action=flightlist", BrowserLaunchMode.External);
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private void Year_Clicked()
        {
            Breakdown = Breakdown.Year;

            var flightsByYear = from f in flights
                                where f.ModelName == viewModel.ModelName || viewModel.ModelName == null
                                group f by f.DateAndTime.Year into g
                                select new
                                {
                                    Date = new DateTime(g.Key, 1, 1),
                                    Flights = g.Count(),
                                    FirstDate = g.Min(f => f.DateAndTime),
                                    LastDate = g.Max(f => f.DateAndTime),
                                    Duration = g.Sum(f => f.DurationS)
                                } into fs
                                orderby fs.Date descending
                                select fs;

            viewModel.Flights.Clear();

            foreach (var year in flightsByYear)
            {
                viewModel.Flights.Add(new FlightSummary
                {
                    Name = year.Date.ToString("yyyy"),
                    TotalFlights = year.Flights,
                    FirstDate = year.FirstDate,
                    LastDate = year.LastDate,
                    Duration = year.Duration
                });
            }
        }

        private void Month_Clicked()
        {
            Breakdown = Breakdown.Month;

            var flightsByMonth = from f in flights
                                 where f.ModelName == viewModel.ModelName || viewModel.ModelName == null
                                 group f by new { f.DateAndTime.Year, f.DateAndTime.Month } into g
                                 select new
                                 {
                                     Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                                     Flights = g.Count(),
                                     FirstDate = g.Min(f => f.DateAndTime),
                                     LastDate = g.Max(f => f.DateAndTime),
                                     Duration = g.Sum(f => f.DurationS)
                                 } into fs
                                 orderby fs.Date descending
                                 select fs;

            viewModel.Flights.Clear();

            foreach (var month in flightsByMonth)
            {
                viewModel.Flights.Add(new FlightSummary
                {
                    Name = month.Date.ToString("MMM yyyy"),
                    TotalFlights = month.Flights,
                    FirstDate = month.FirstDate,
                    LastDate = month.LastDate,
                    Duration = month.Duration
                });
            }
        }

        private void Week_Clicked()
        {
            Breakdown = Breakdown.Week;

            var flightsByWeek = from f in flights
                                where f.ModelName == viewModel.ModelName || viewModel.ModelName == null
                                group f by LastDayOfWeek(f.DateAndTime) into g
                                select new
                                {
                                    Date = g.Key,
                                    Flights = g.Count(),
                                    FirstDate = g.Min(f => f.DateAndTime),
                                    LastDate = g.Max(f => f.DateAndTime),
                                    Duration = g.Sum(f => f.DurationS)
                                } into fs
                                orderby fs.Date descending
                                select fs;

            viewModel.Flights.Clear();

            foreach (var day in flightsByWeek)
            {
                viewModel.Flights.Add(new FlightSummary
                {
                    Name = day.Date.ToString("d MMM yyyy"),
                    TotalFlights = day.Flights,
                    FirstDate = day.FirstDate,
                    LastDate = day.LastDate,
                    Duration = day.Duration
                });
            }
        }

        public static DateTime LastDayOfWeek(DateTime date)
        {
            return date.Date.AddDays(6 - DayOfMondayWeek(date));
        }

        public static int DayOfMondayWeek(DateTime date)
        {
            var dow = (int)date.DayOfWeek - 1;

            if (dow < 0)
            {
                dow += 7;
            }

            return dow;
        }

        private void Day_Clicked()
        {
            Breakdown = Breakdown.Day;

            var flightsByDay = from f in flights
                               where f.ModelName == viewModel.ModelName || viewModel.ModelName == null
                               group f by f.DateAndTime.Date into g
                               select new
                               {
                                   Date = g.Key,
                                   Flights = g.Count(),
                                   FirstDate = g.Min(f => f.DateAndTime),
                                   LastDate = g.Max(f => f.DateAndTime),
                                   Duration = g.Sum(f => f.DurationS)
                               } into fs
                               orderby fs.Date descending
                               select fs;

            viewModel.Flights.Clear();

            foreach (var day in flightsByDay)
            {
                viewModel.Flights.Add(new FlightSummary
                {
                    Name = day.Date.ToString("d M yyyy"),
                    TotalFlights = day.Flights,
                    FirstDate = day.FirstDate,
                    LastDate = day.LastDate,
                    Duration = day.Duration
                });
            }
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Breakdown)
            {
                case Breakdown.Year:
                    Year_Clicked();
                    break;
                case Breakdown.Month:
                    Month_Clicked();
                    break;
                case Breakdown.Week:
                    Week_Clicked();
                    break;
                case Breakdown.Day:
                    Day_Clicked();
                    break;
            }
        }

        private async void Flights_OnClicked(object sender, EventArgs e)
        {
            var flightSummary = (FlightSummary)((Button)sender).BindingContext;

            await Shell.Current.GoToAsync($"{nameof(FlightsByDatePage)}?FromDate={flightSummary.FirstDate}&ToDate={flightSummary.LastDate}");
        }

        private void GroupBy_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (viewModel == null)
            {
                return;
            }

            switch (GroupBy.CheckedItem.Text)
            {
                case "Day":
                    Day_Clicked();
                    break;
                case "Week":
                    Week_Clicked();
                    break;
                case "Month":
                    Month_Clicked();
                    break;
                case "Year":
                    Year_Clicked();
                    break;
            }
        }
    }

    internal enum Breakdown
    {
        Day,
        Week,
        Month,
        Year,
    }
}