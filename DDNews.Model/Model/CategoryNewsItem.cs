using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace DDNews.Model
{
    public class CategoryWithNewsItems : ViewModelBase
    {
        private Category _category;
        public Category category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                RaisePropertyChanged("category");
            }
        }

        private ObservableCollection<NewsItem> _newsItems;
        public ObservableCollection<NewsItem> NewsItems
        {
            get
            {
                return _newsItems;
            }
            set
            {
                _newsItems = value;
                RaisePropertyChanged("NewsItems");
            }
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
    }
}
