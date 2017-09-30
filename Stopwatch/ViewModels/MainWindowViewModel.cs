using Prism.Mvvm;

namespace Stopwatch.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Autofac Stopwatch Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
