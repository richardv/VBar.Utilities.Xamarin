namespace VBarUtilities.Views
{
    using Syncfusion.SfChart.XForms;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VBarUtilities.ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var flights = await App.Database.Flights().ToListAsync();

            TodayFlights.Text = Fmt.Flights(flights.Count(f => f.DateAndTime.Date == DateTime.Today));
            TodayDuration.Text = Fmt.TimeS(flights.Where(f => f.DateAndTime.Date == DateTime.Today).Sum(f => f.DurationS));

            YesterdayFlights.Text = Fmt.Flights(flights.Count(f => f.DateAndTime.Date == DateTime.Today.AddDays(-1)));
            YesterdayDuration.Text = Fmt.TimeS(flights.Where(f => f.DateAndTime.Date == DateTime.Today.AddDays(-1)).Sum(f => f.DurationS));

            TotalFlights.Text = Fmt.Flights(flights.Count);
            TotalDuration.Text = Fmt.TimeS(flights.Sum(f => f.DurationS));
            if (flights.Any())
            {
                AverageDuration.Text = Fmt.TimeS(flights.Sum(f => f.DurationS) / flights.Count());
            }

            MonthFlights.Text = Fmt.Flights(flights.Count(f => f.DateAndTime.Year == DateTime.Today.Year && f.DateAndTime.Month == DateTime.Today.Month));
            MonthDuration.Text = Fmt.TimeS(flights.Where(f => f.DateAndTime.Year == DateTime.Today.Year && f.DateAndTime.Month == DateTime.Today.Month).Sum(f => f.DurationS));

            LastMonthFlights.Text = Fmt.No(flights.Count(f => f.DateAndTime.Year == DateTime.Today.AddMonths(-1).Year && f.DateAndTime.Month == DateTime.Today.AddMonths(-1).Month));
            LastMonthDuration.Text = Fmt.TimeS(flights.Where(f => f.DateAndTime.Year == DateTime.Today.AddMonths(-1).Year && f.DateAndTime.Month == DateTime.Today.AddMonths(-1).Month).Sum(f => f.DurationS));

            YearFlights.Text = Fmt.Flights(flights.Count(f => f.DateAndTime.Year == DateTime.Today.Year));
            YearDuration.Text = Fmt.TimeS(flights.Where(f => f.DateAndTime.Year == DateTime.Today.Year).Sum(f => f.DurationS));

            LastYearFlights.Text = Fmt.Flights(flights.Count(f => f.DateAndTime.Year == DateTime.Today.AddYears(-1).Year));
            LastYearDuration.Text = Fmt.TimeS(flights.Where(f => f.DateAndTime.Year == DateTime.Today.AddYears(-1).Year).Sum(f => f.DurationS));

            // Best Day
            var days = from f in flights
                       group f by f.DateAndTime.Date into g
                       select new
                       {
                           Day = g.Key,
                           Flights = g.Count(),
                           Duration = g.Sum(f => f.DurationS)
                       };

            var bestDay = days.OrderByDescending(d => d.Flights).FirstOrDefault();

            if (bestDay != null)
            {
                BestDay.Text = Fmt.Day(bestDay.Day);
                BestDayFlights.Text = Fmt.Flights(bestDay.Flights);
                BestDayDuration.Text = Fmt.TimeS(bestDay.Duration);
            }

            var flightsByMonths = from f in flights
                                  where f.DateAndTime.Year >= DateTime.Today.Year
                                  group f by new { f.DateAndTime.Year, f.DateAndTime.Month } into g
                                  select new
                                  {
                                      Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                                      Flights = g.Count(),
                                      Duration = g.Sum(x => x.DurationS)
                                  };

            var bestMonth = flightsByMonths.OrderByDescending(f => f.Flights).FirstOrDefault();

            if (bestMonth != null)
            {
                BestMonth.Text = Fmt.Month(bestMonth.Date);
                BestMonthFlights.Text = Fmt.Flights(bestMonth.Flights);
                BestMonthDuration.Text = Fmt.TimeS(bestMonth.Duration);
            }

            var years = from f in flights
                        group f by f.DateAndTime.Year into g
                        select new
                        {
                            Date = new DateTime(g.Key, 1, 1),
                            Flights = g.Count(),
                            Duration = g.Sum(x => x.DurationS)
                        };

            var bestYear = years.OrderByDescending(d => d.Flights).FirstOrDefault();

            if (bestYear != null)
            {
                BestYear.Text = Fmt.Year(bestYear.Date);
                BestYearFlights.Text = Fmt.Flights(bestYear.Flights);
                BestYearDuration.Text = Fmt.TimeS(bestYear.Duration);
            }

            // Chart
            Chart.Series.Clear();

            var firstFlight = flights.OrderBy(f => f.DateAndTime).FirstOrDefault();
            var lastFlight = flights.OrderByDescending(f => f.DateAndTime).FirstOrDefault();

            if (firstFlight != null && lastFlight != null)
            {
                for (int year = firstFlight.DateAndTime.Year; year <= lastFlight.DateAndTime.Year; year++)
                {
                    // Series
                    var flightsByMonth = new List<FlightsByMonth> {
                        new FlightsByMonth
                        {
                            Month = "",
                            Flights = 0
                        }
                    };

                    var startValue = 0;

                    for (int i = 1; i <= 12; i++)
                    {
                        var month = new DateTime(year, i, 1);

                        if (month > DateTime.Now)
                        {
                            break;
                        }

                        startValue += flights.Where(f => f.DateAndTime.Year == year && f.DateAndTime.Month == i).Count();

                        flightsByMonth.Add(new FlightsByMonth
                        {
                            Month = month.ToString("MMM"),
                            Flights = startValue
                        });
                    }

                    Chart.Series.Insert(
                        0,
                        new SplineSeries
                        {
                            XBindingPath = "Month",
                            YBindingPath = "Flights",
                            Label = year.ToString(),
                            EnableTooltip = true,
                            SplineType = SplineType.Monotonic,
                            DataMarker = new ChartDataMarker
                            {
                                ShowMarker = true,
                                ShowLabel = false,
                                MarkerType = DataMarkerType.Diamond,
                                MarkerHeight = 10,
                                MarkerWidth = 10
                            },
                            ItemsSource = flightsByMonth
                        });


                }

                // Predicted
                if (DateTime.Now.Month != 12)
                {
                    var predicted = new SplineSeries
                    {
                        XBindingPath = "Month",
                        YBindingPath = "Flights",
                        Label = "Predicted",
                        EnableTooltip = true,
                        SplineType = SplineType.Monotonic
                    };

                    var predictedFlightsByMonth = new List<FlightsByMonth> {
                        new FlightsByMonth
                        {
                            Month = "",
                            Flights = 10
                        }
                    };

                    var predictedFlights = (decimal)flights.Where(f => f.DateAndTime.Year == DateTime.Today.Year).Count() / DateTime.Today.DayOfYear * 365;

                    for (int i = 1; i < 13; i++)
                    {
                        predictedFlightsByMonth.Add(new FlightsByMonth
                        {
                            Month = new DateTime(DateTime.Today.Year, i, 1).ToString("MMM"),
                            Flights = (int)(predictedFlights * i / 12)
                        });
                    }

                    predicted.ItemsSource = predictedFlightsByMonth;
                    Chart.Series.Insert(0, predicted);
                }

                // Streaks
                var days1 = from f in flights
                            group f by f.DateAndTime.Date
                    into g
                            orderby g.Key
                            select new { Date = g.Key, flights = g.Count() };

                var streakDays = 0;
                var streakFlights = 0;
                var streakDate = DateTime.MinValue;

                var bestStreakDays = 0;
                var bestStreakFlights = 0;
                var bestStreakDate = DateTime.Today;

                foreach (var day in days1)
                {
                    if (day.Date == streakDate.AddDays(1))
                    {
                        streakDays += 1;
                        streakFlights += day.flights;
                        streakDate = day.Date;
                    }
                    else
                    {
                        if (streakDays >= bestStreakDays)
                        {
                            bestStreakDays = streakDays;
                            bestStreakDate = streakDate;
                            bestStreakFlights += day.flights;
                        }

                        streakDays = 1;
                        streakDate = day.Date;
                    }
                }

                if (streakDate >= DateTime.UtcNow.Date.AddDays(-1))
                {
                    StreakDays.Text = Fmt.No(streakDays) + " days";
                    StreakFlights.Text = Fmt.Flights(streakFlights);
                }

                BestStreakDays.Text = Fmt.Days(bestStreakDays);
                BestStreakDate.Text = Fmt.Day(bestStreakDate);
                BestStreakFlights.Text = Fmt.Flights(bestStreakFlights);

                // Updates
                var updates = await App.Database.Devices().CountAsync(d => d.Update);

                if (updates > 0)
                {
                    Updates.Text = $"{updates} device update{Fmt.Plural(updates)} available";
                }
            }
        }

        private async void Screenshots_Clicked(object sender, EventArgs e)
        {
            if (App.Screenshots != null)
            {
                await App.Screenshots.OpenFolder();
            }
        }

        private async void OpenVStabi_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud", BrowserLaunchMode.External);
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }
    }
}