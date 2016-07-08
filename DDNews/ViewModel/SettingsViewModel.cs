using DDNews.Model;
using DDNews.Model.Services;
using GalaSoft.MvvmLight;

namespace DDNews.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly DataService _dataService;
        private Settings _settings;
        public Settings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                RaisePropertyChanged("Settings");
            }
        }

        public SettingsViewModel(DataService dataService)
        {
            if (Settings == null)
                Settings = new Settings();
            _dataService = dataService;
            Settings = _dataService.Settings;
        }
    }
}