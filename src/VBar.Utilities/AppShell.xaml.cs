namespace VBarUtilities
{
    using VBarUtilities.Views;
    using Xamarin.Forms;

    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PartAddPage), typeof(PartAddPage));
            Routing.RegisterRoute(nameof(PartEditPage), typeof(PartEditPage));
            Routing.RegisterRoute(nameof(PartsPage), typeof(PartsPage));
            Routing.RegisterRoute(nameof(SetupPage), typeof(SetupPage));
            Routing.RegisterRoute(nameof(FlightsByDatePage), typeof(FlightsByDatePage));
            Routing.RegisterRoute(nameof(FlightPage), typeof(FlightPage));
            Routing.RegisterRoute(nameof(BatteryPage), typeof(BatteryPage));
            Routing.RegisterRoute(nameof(ModelPage), typeof(ModelPage));
        }
    }
}
