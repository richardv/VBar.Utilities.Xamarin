namespace VBarUtilities
{
    using Models;
    using System;
    using System.IO;
    using VBarUtilities.Data;
    using Xamarin.Forms;

    public partial class App
    {
        static VuDatabase database;

        public static VuDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new VuDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VBarUtilities.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI5MDUwQDMxMzkyZTMzMmUzME9ITVNlQTNHZ3hIckdudVlENXEwRWpPbEhBT1pUK0tjZThoUEVjTEVyTlE9");

            ClassicImport = DependencyService.Get<IClassicImport>();
            Screenshots = DependencyService.Get<IScreenshots>();
            Platform = DependencyService.Get<IPlatform>();
            Save = DependencyService.Get<ISave>();
        }
        public static IClassicImport ClassicImport { get; set; }

        public static IScreenshots Screenshots { get; set; }

        public static IPlatform Platform { get; set; }

        public static ISave Save { get; set; }
    }
}
