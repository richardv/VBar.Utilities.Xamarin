namespace VBarUtilities.UWP
{
    using Models;

    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            VBarUtilities.App.ClassicImport = new ClassicImportUwp();

            VBarUtilities.App.Screenshots = new Screenshots();

            VBarUtilities.App.Platform = new Platform();

            LoadApplication(new VBarUtilities.App());
        }
    }
}
