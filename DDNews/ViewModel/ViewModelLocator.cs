using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using DDNews.Model.Services;
using DDNews.Services;

namespace DDNews.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<NavigationService>();            
            SimpleIoc.Default.Register<MainViewModel>();
        }
        
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public DataService DataService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DataService>();
            }
        }

        public NavigationService NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<NavigationService>(); }
        }
        
        public void Cleanup()
        {
            Main.SaveAgent();
            _isSaved = true;
        }

        private bool _isSaved = false;

        public bool IsSaved()
        {
            return _isSaved;
        }
    }
}