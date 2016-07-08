using GalaSoft.MvvmLight;
using DDNews.Model.Services;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using DDNews.Model;
using DDNews.Services;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Linq;
using Microsoft.Phone.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Tasks;

namespace DDNews.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const string AgentName = "DDNEWS.Agent";
        private readonly DataService _dataService;
        private ObservableCollection<NewsItem> _categoryItems;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private bool _inArticlePage = false;
        private ObservableCollection<NewsItem> _selectedCategoryItems;
        private ObservableCollection<MainCategory> _mainCategories;
        private MainCategory _selectedMainCategory;
        private NavigationService _navigationService;
        private NewsItem _currentArticle;
        private NewsItem _previousArticle;
        private NewsItem _nextArticle;
        private bool _isLoading;
        private bool _showStyling;
        private Settings _settings;
        private bool _isMyAppsLoading;
        public bool IsMyAppsLoading
        {
            get
            {
                return _isMyAppsLoading;
            }
            set
            {
                _isMyAppsLoading = value;
                RaisePropertyChanged("IsMyAppsLoading");
            }
        }

        #region Properties

        private ObservableCollection<MyApp> _listMyApps;
        public ObservableCollection<MyApp> ListOfMyApps
        {
            get
            {
                return _listMyApps;
            }
            set
            {
                _listMyApps = value;
                RaisePropertyChanged("ListOfMyApps");
            }
        }

        public MyApp MySelectedApp { get; set; }

        public ObservableCollection<NewsItem> CategoryItems
        {
            get { return _categoryItems; }
            set { _categoryItems = value; RaisePropertyChanged("CategoryItems"); }
        }

        public Settings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                RaisePropertyChanged("Settings");
            }
        }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged("Categories"); }
        }

        public bool InArticlePage
        {
            get { return _inArticlePage; }
            set { _inArticlePage = value; RaisePropertyChanged("InArticlePage"); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged("IsLoading"); }
        }
        public bool ShowStyling
        {
            get { return _showStyling; }
            set
            {
                _showStyling = value;
                RaisePropertyChanged("ShowStyling");
            }
        }

        private string _categoriesText;
        public string CategoriesText
        {
            get
            {
                return _categoriesText;
            }
            set
            {
                _categoriesText = value;
                RaisePropertyChanged("CategoriesText");
            }
        }

        private string _headLinesText;
        public string HeadLinesText
        {
            get
            {
                return _headLinesText;
            }
            set
            {
                _headLinesText = value;
                RaisePropertyChanged("HeadLinesText");
            }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged("SelectedCategory"); }
        }

        public ObservableCollection<NewsItem> SelectedCategoryItems
        {
            get { return _selectedCategoryItems; }
            set { _selectedCategoryItems = value; RaisePropertyChanged("SelectedCategoryItems"); }
        }

        public ObservableCollection<MainCategory> MainCategories
        {
            get
            {
                return _mainCategories;
            }
            set
            {
                _mainCategories = value;
                RaisePropertyChanged("MainCategories");
            }
        }

        public MainCategory SelectedMainCategory
        {
            get
            {
                return _selectedMainCategory;
            }
            set
            {
                _selectedMainCategory = value;
                RaisePropertyChanged("SelectedMainCategory");
            }
        }

        public NewsItem CurrentArticle
        {
            get
            {
                return _currentArticle;
            }
            set
            {
                _currentArticle = value;
                RaisePropertyChanged("CurrentArticle");
            }
        }
        public NewsItem PreviousArticle
        {
            get
            {
                return _previousArticle;
            }
            set
            {
                _previousArticle = value;
                RaisePropertyChanged("PreviousArticle");
            }
        }
        public NewsItem NextArticle
        {
            get
            {
                return _nextArticle;
            }
            set
            {
                _nextArticle = value;
                RaisePropertyChanged("NextArticle");
            }
        }

        public RelayCommand ReadCurrentArticleCommand { get; private set; }
        public RelayCommand<NewsItem> ReadSelectionChangedCommand { get; private set; }
        public RelayCommand HomePagedLoadedCommand { get; private set; }

        public RelayCommand ShowMyAppsCommand { get; private set; }
        public RelayCommand ReadMyAppCommand { get; private set; }
        public RelayCommand MyAppsLoadedCommand { get; private set; }
        #endregion


        #region RelayCommands

        private RelayCommand _backButtonCommand;
        public RelayCommand BackButtonCommand
        {
            get
            {
                return _backButtonCommand ?? (_backButtonCommand = new RelayCommand(() =>
                {
                    InArticlePage = false;
                    _navigationService.NavigateTo(new System.Uri("HomePage"));
                    //MyFrame.Navigate(typeof(Home_Page));
                }));
            }
        }

        private RelayCommand _settingsButtonCommand;
        public RelayCommand SettingsButtonCommand
        {
            get
            {
                return _settingsButtonCommand ?? (_settingsButtonCommand = new RelayCommand(() => { ShowSettings(); }));
            }
        }

        private RelayCommand _homeButtonCommand;
        public RelayCommand HomeButtonCommand
        {
            get
            {
                return _homeButtonCommand ?? (_homeButtonCommand = new RelayCommand(() => { ShowCategory(); }));
            }
        }
        private RelayCommand _showAboutPageCommand;
        public RelayCommand ShowAboutPageCommand
        {
            get
            {
                return _showAboutPageCommand ?? (_showAboutPageCommand = new RelayCommand(() => { ShowAboutPage(); }));
            }
        }

        private void ShowAboutPage()
        {
            App.RootFrame.Navigate(new Uri(string.Format("/Views/AboutPage.xaml"), UriKind.RelativeOrAbsolute));
        }

        public RelayCommand GoToSelectedCategoryCommand { get; private set; }
        private RelayCommand _shareEmailArticleCommand;
        public RelayCommand ShareEmailArticleCommand
        {
            get
            {
                return _shareEmailArticleCommand ?? (_shareEmailArticleCommand = new RelayCommand(() => { ShareEmailArticle(); }));
            }
        }

        private RelayCommand _shareArticleCommand;
        public RelayCommand ShareArticleCommand
        {
            get
            {
                return _shareArticleCommand ?? (_shareArticleCommand = new RelayCommand(() => { ShareArticle(); }));
            }
        }

        private RelayCommand _showSettingsCommand;
        public RelayCommand ShowSettingsCommand
        {
            get
            {
                return _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(() => { ShowSettings(); }));
            }
        }

        private RelayCommand _stylingCommand;
        public RelayCommand StylingCommand
        {
            get
            {
                return _stylingCommand ?? (_stylingCommand = new RelayCommand(() => { Styling(); }));
            }
        }
        private RelayCommand<double> _setFontSizeCommand;
        public RelayCommand<double> SetFontSizeCommand
        {
            get
            {
                return _setFontSizeCommand ?? (_setFontSizeCommand = new RelayCommand<double>((fontsize) =>
                {
                    if (Settings != null)
                    {
                        Settings.FontSize = fontsize;
                        ShowStyling = false;
                    }
                }));
            }
        }

        private RelayCommand<FontFamily> _setFontFamilyCommand;
        public RelayCommand<FontFamily> SetFontFamilyCommand
        {
            get
            {
                return _setFontFamilyCommand ?? (_setFontFamilyCommand = new RelayCommand<FontFamily>((fontFamily) =>
                {
                    if (Settings != null)
                    {
                        Settings.FontFamily = fontFamily.ToString();
                        ShowStyling = false;
                    }
                }));
            }
        }

        private RelayCommand<string> _setThemeCommand;
        public RelayCommand<string> SetThemeCommand
        {
            get
            {
                return _setThemeCommand ?? (_setThemeCommand = new RelayCommand<string>((theme) =>
                {
                    if (Settings != null)
                    {
                        if (theme.ToLower() == "dark")
                        {
                            Settings.Theme = "Black";
                        }
                        else
                        {
                            Settings.Theme = "White";
                        }
                        ShowStyling = false;
                    }
                }));
            }
        }

        private RelayCommand _articleTapCommand;
        public RelayCommand ArticleTapCommand
        {
            get
            {
                return _articleTapCommand ?? (_articleTapCommand = new RelayCommand(() =>
                {
                    if (ShowStyling == true)
                    {
                        ShowStyling = false;
                    }
                }));
            }
        }

        #endregion

        public MainViewModel(DataService dataService, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            Settings = dataService.Settings;
            ReadCurrentArticleCommand = new RelayCommand(ReadCurrentArticle);
            ReadSelectionChangedCommand = new RelayCommand<NewsItem>((args) => ReadSelectionChanged(args));
            HomePagedLoadedCommand = new RelayCommand(HomePageLoaded);
            GoToSelectedCategoryCommand = new RelayCommand(GoToSelectedCategoryPage);
            ShowMyAppsCommand = new RelayCommand(ShowMyApps);
            MyAppsLoadedCommand = new RelayCommand(GetMyApps);
            ReadMyAppCommand = new RelayCommand(OpenMyApp);

            _categoryItems = new ObservableCollection<NewsItem>();
            Categories = new ObservableCollection<Category>();
            SelectedCategoryItems = new ObservableCollection<NewsItem>();
            CurrentArticle = new NewsItem();
            NextArticle = new NewsItem();
            PreviousArticle = new NewsItem();
            IsMyAppsLoading = true;
            HeadLinesText = Settings.SelectedLanguage.ToLower() == "hindi" ? "प्रमुख समाचार" : "HeadLines";
            CategoriesText = Settings.SelectedLanguage.ToLower() == "hindi" ? "अन्य समाचार" : "Categories";
        }

        private void OpenMyApp()
        {
            if (MySelectedApp != null)
            {
                var wbt = new WebBrowserTask();
                wbt.Uri = new Uri(MySelectedApp.appUrl, UriKind.Absolute);
                wbt.Show();
            }
        }

        private void GetMyApps()
        {
            if (_dataService.GetIsNetWorkAvailable())
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(async () =>
                {
                    IsMyAppsLoading = true;
                    if (ListOfMyApps == null)
                        ListOfMyApps = new ObservableCollection<MyApp>();
                    if (ListOfMyApps.Count == 0)
                    {
                        var myApps = await _dataService.GetMyAppsAsync();
                        if (myApps != null)
                        {
                            foreach (var myapp in myApps)
                            {
                                ListOfMyApps.Add(myapp);
                            }
                        }
                    }
                    IsMyAppsLoading = false;
                });
            }
            else
            {
                MessageBox.Show("Sorry, There is some problem with internet connectivity");
            }
        }

        private void ShowMyApps()
        {
            App.RootFrame.Navigate(new Uri("/Views/MyAppsPage.xaml", UriKind.Relative));
        }

        private static bool _isNetworkNotAvailableMsgDisplayed = false;
        private void HomePageLoaded()
        {
            var headLinesTask = Task.Factory.StartNew(async () =>
            {
                var isNetworkAvailable = GetIsNetWorkAvailable();
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                {
                    HeadLinesText = Settings.SelectedLanguage.ToLower() == "hindi" ? "प्रमुख समाचार" : "HeadLines";
                    CategoriesText = Settings.SelectedLanguage.ToLower() == "hindi" ? "अन्य समाचार" : "Categories";
                    IsLoading = true;
                    if (isNetworkAvailable == false && _isNetworkNotAvailableMsgDisplayed == false)
                    {
                        MessageBox.Show("Sorry, There is some problem with internet connectivity");
                        _isNetworkNotAvailableMsgDisplayed = true;
                    }
                });
                var articles = await _dataService.LoadHeadLinesAsync();
                if (articles != null && articles.Count > 0)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        SelectedCategoryItems.Clear();
                        articles.OrderByDescending(o =>
                        {
                            var date = new DateTime();
                            DateTime.TryParse(o.publish_date, out date);
                            return date;
                        }).ToList().ForEach(i => SelectedCategoryItems.Add(i));
                        IsLoading = false;
                    });
                }
            });

            Task.Factory.StartNew(async () =>
            {
                var categories = await _dataService.GetCategoriesAsync();
                if (categories != null && categories.Count > 0)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        Categories.Clear();
                        categories.ForEach(o =>
                        {
                            if (o.Language == Settings.SelectedLanguage)
                                Categories.Add(o);
                        });
                    });
                }
            });

        }

        private void ReadSelectionChanged(NewsItem newsitem)
        {
            CurrentArticle = newsitem;
            ReadCurrentArticle();
        }

        public async Task LoadHeadLinesAsync()
        {
            var articles = await _dataService.LoadHeadLinesAsync();
            if (articles != null && articles.Count > 0)
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                {
                    SelectedCategoryItems.Clear();
                    articles.OrderByDescending(o => o.PublishDate).ToList().ForEach(i => SelectedCategoryItems.Add(i));
                    IsLoading = false;
                });
            }
        }

        public void SetCurrentPreviousandNextArticle(int id, string language)
        {
            int index = 0;
            foreach (var article in SelectedCategoryItems)
            {
                if (article.id == id && article.language == language)
                {
                    break;
                }
                index++;
            }
            CurrentArticle = SelectedCategoryItems[index];
            if (index == 0)
            {
                PreviousArticle = SelectedCategoryItems[SelectedCategoryItems.Count - 1];
                NextArticle = SelectedCategoryItems[index + 1];
            }
            else if (index >= SelectedCategoryItems.Count - 1)
            {
                NextArticle = SelectedCategoryItems[0];
                PreviousArticle = SelectedCategoryItems[index - 1];
            }
            else
            {
                NextArticle = SelectedCategoryItems[index + 1];
                PreviousArticle = SelectedCategoryItems[index - 1];
            }
        }

        public void SetCurrentPreviousandNextArticle(NewsItem currentArticle)
        {
            if (currentArticle != null)
            {
                CurrentArticle = currentArticle;
                int index = 0;
                foreach (var article in SelectedCategoryItems)
                {
                    if (article.id == currentArticle.id)
                    {
                        break;
                    }
                    index++;
                }
                if (index == 0)
                {
                    PreviousArticle = SelectedCategoryItems[SelectedCategoryItems.Count - 1];
                    NextArticle = SelectedCategoryItems[index + 1];
                }
                else if (index >= SelectedCategoryItems.Count - 1)
                {
                    NextArticle = SelectedCategoryItems[0];
                    PreviousArticle = SelectedCategoryItems[index - 1];
                }
                else
                {
                    NextArticle = SelectedCategoryItems[index + 1];
                    PreviousArticle = SelectedCategoryItems[index - 1];
                }
            }
        }

        private void GoToSelectedCategoryPage()
        {
            IsLoading = true;
            SelectedCategoryItems.Clear();
            _navigationService.NavigateTo(new Uri("/Views/CategoryPage.xaml", UriKind.RelativeOrAbsolute));
            Task.Factory.StartNew(async () =>
            {
                if (GetIsNetWorkAvailable() == false)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Sorry, There is some problem with internet connectivity");
                    });
                }
                var categoryArticles = await _dataService.GetCategoryNewsItemsAsync(SelectedCategory);
                if (categoryArticles != null && categoryArticles.Count > 0)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        SelectedCategoryItems.Clear();
                        categoryArticles.OrderByDescending(o => o.PublishDate).ToList().ForEach(i => SelectedCategoryItems.Add(i));
                        IsLoading = false;
                    });
                }
                else
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        IsLoading = false;
                    });
                }
            });
        }

        private bool GetIsNetWorkAvailable()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return true;
            }
            return false;
        }

        private int currentArticleIndex = 0;
        public void ReadCurrentArticle()
        {
            try
            {
                currentArticleIndex = 0;
                if (CurrentArticle != null && !string.IsNullOrEmpty(CurrentArticle.news_title))
                {
                    var index = 0;
                    foreach (var article in CategoryItems)
                    {
                        if (article.news_title == CurrentArticle.news_title && CurrentArticle.category == article.category)
                        {
                            if (index == 0)
                            {
                                PreviousArticle = CategoryItems[CategoryItems.Count - 1];
                                NextArticle = CategoryItems[index + 1];
                            }
                            else if (index >= CategoryItems.Count - 1)
                            {
                                NextArticle = CategoryItems[0];
                                PreviousArticle = CategoryItems[index - 1];
                            }
                            else
                            {
                                PreviousArticle = CategoryItems[index - 1];
                                NextArticle = CategoryItems[index + 1];
                            }
                            break;
                        }
                        index++;
                        currentArticleIndex++;
                    }
                    App.RootFrame.Navigate(new Uri(string.Format("/Views/ArticlePage.xaml"), UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception)
            {
            }
        }

        private void HomeButtonAction()
        {
        }

        private void BackButtonAction()
        {
        }

        private void ShowSettings()
        {
            App.RootFrame.Navigate(new Uri(string.Format("/Views/SettingsPage.xaml"), UriKind.RelativeOrAbsolute));
        }

        private void ShowCategory()
        {
            App.RootFrame.Navigate(new Uri(string.Format("/Views/HomePage.xaml"), UriKind.RelativeOrAbsolute));
            //_navigationService.NavigateTo(new System.Uri("HomePage"));
        }

        private void Styling()
        {
            ShowStyling = !ShowStyling;
        }

        private void ShareArticle()
        {
            //try
            //{
            //    ShareLinkTask shareLinkTask = new ShareLinkTask
            //    {
            //        Title = "Read this article!",
            //        LinkUri = new Uri(CurrentArticle., UriKind.Absolute),
            //        Message = Article.HeadLine
            //    };
            //    shareLinkTask.Show();
            //}
            //catch (Exception)
            //{
            //}
        }

        private void ShareEmailArticle()
        {
        }

        public void SaveSettings()
        {
            if (_dataService != null && _settings != null)
            {
                _dataService.SaveSettings(_settings);
            }
        }
        private void UpdateAgent()
        {
            if (Settings.IsDownloadingArticlesOffline)
            {
                StartAgent();
            }
            else
            {
                StopAgentIfStarted();
            }
        }

        private static void StartAgent()
        {
            StopAgentIfStarted();
            try
            {
                ScheduledActionService.Add(new PeriodicTask(AgentName) { Description = "Periodically download articles in the background if the Internet connection is Wi-Fi or Ethernet." });
#if DEBUG
                ScheduledActionService.LaunchForTest(AgentName, new TimeSpan(0, 0, 0, 3));
#endif
            }
            catch (Exception)
            {
            }
        }

        private static void StopAgentIfStarted()
        {
            if (ScheduledActionService.Find(AgentName) != null)
            {
                ScheduledActionService.Remove(AgentName);
            }
        }

        public void SaveAgent()
        {
            try
            {
                UpdateAgent();
            }
            catch (Exception ex)
            {
            }
        }
    }
}