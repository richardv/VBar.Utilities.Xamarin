namespace VBarUtilities.ViewModels
{
    public class ClassicViewModel : BaseViewModel
    {
        private string path;
        private string status;
        private double progress;

        public string Path
        {
            get => path; set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public string Status
        {
            get => status; set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public double Progress
        {
            get => progress; set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
    }
}
