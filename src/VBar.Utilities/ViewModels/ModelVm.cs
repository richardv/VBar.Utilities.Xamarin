namespace VBarUtilities.ViewModels
{
    using System;
    using Xamarin.Forms;

    public class ModelVm
    {
        public string Name { get; set; }

        public ImageSource ImageData { get; set; }

        public int TotalFlights { get; set; }

        public double TotalDuration { get; set; }

        public DateTime? LastFlight { get; set; }

        public double? LastDuration { get; set; }

        public string DeviceId { get; set; }
    }
}