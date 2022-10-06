namespace VBarUtilities.ViewModels
{
    using System.Collections.ObjectModel;

    public class FlightsViewModel : BaseViewModel
    {
        public ObservableCollection<string> Models { get; set; } = new ObservableCollection<string>();

        public string ModelName { get; set; }

        public ObservableCollection<FlightSummary> Flights { get; set; } = new ObservableCollection<FlightSummary>();
    }
}
