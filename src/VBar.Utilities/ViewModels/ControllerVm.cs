namespace VBarUtilities.ViewModels
{
    using System;
    using Xamarin.Forms;

    internal class ControllerVm
    {
        public string Name { get; set; }

        public ImageSource ImageData { get; set; }

        public int Flights { get; set; }

        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }
    }
}