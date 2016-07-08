using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDNews.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Scheduler;

namespace DDNews.Model.Services
{
    public class DataService
    {
        private List<NewsItem> _headLines;
        public List<NewsItem> HeadLines
        {
            get { return _headLines; }
            set { _headLines = value; }
        }
        private List<Category> _categories;
        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
            }
        }

        private Settings _settings;
        public Settings Settings
        {
            get { return _settings; }
            private set { }
        }
        
        public DataService()
        {
            _settings = DatabaseOperations.Instance.GetSettings();
            _headLines = new List<NewsItem>();
            _categories = new List<Category>();
            Task.Factory.StartNew(async () =>
            {
                _categories = await LoadCategoriesAsync();
                var tempHeadLines = await LoadHeadLinesAsync(_settings.SelectedLanguage);
                if (tempHeadLines != null && tempHeadLines.Count > 0)
                {
                    HeadLines.Clear();
                    tempHeadLines.ForEach(o => HeadLines.Add(o));
                }
            });
        }

        public ObservableCollection<MainCategory> LoadMainCategories()
        {
            ObservableCollection<MainCategory> mainCategories = new ObservableCollection<MainCategory>();
            mainCategories.Add(new MainCategory(Consts.CATEGORY_ENGLISH_STRING, ""));
            mainCategories.Add(new MainCategory(Consts.CATEGORY_HINDI_STRING, ""));
            return mainCategories;
        }

        private async Task<List<Category>> LoadCategoriesAsync()
        {
            List<Category> categories = new List<Category>();
            DownloadUtil downloadUtils = new DownloadUtil();
            var categoriesString = await downloadUtils.DownloadStringAsync(Consts.DDNEWS_URL);
            var listOfCategories = Newtonsoft.Json.JsonConvert.DeserializeObject<ListOfCategories>(categoriesString);
            if (listOfCategories != null)
            {
                categories = listOfCategories.categories;
                categories.ForEach(async o => { await DatabaseOperations.Instance.UpdateCategoryAsync(o); });
            }

            if (categories == null || categories.Count == 0)
            {
                categories.Add(new Category(Consts.HEADLINES_ENG_STRING, Consts.HEADLINES_ENG_STRING, Consts.HEADLINES_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.SCIENCE_TECH_ENG_STRING, Consts.SCIENCE_TECH_ENG_STRING, Consts.SCIENCE_TECH_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.SOCIAL_ENG_STRING, Consts.SOCIAL_ENG_STRING, Consts.SOCIAL_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.NATIONAL_ENG_STRING, Consts.NATIONAL_ENG_STRING, Consts.NATIONAL_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.INTERNATIONAL_ENG_STRING, Consts.INTERNATIONAL_ENG_STRING, Consts.INTERNATIONAL_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.BUSINESS_ENG_STRING, Consts.BUSINESS_ENG_STRING, Consts.BUSINESS_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.HEALTH_ENG_STRING, Consts.HEALTH_ENG_STRING, Consts.HEALTH_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.CURRENT_ENG_STRING, Consts.CURRENT_ENG_DECORATIVE_STRING, Consts.CURRENT_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.SPORTS_ENG_STRING, Consts.SPORTS_ENG_STRING, Consts.SPORTS_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));
                categories.Add(new Category(Consts.ENTERTAINMENT_ENG_STRING, Consts.ENTERTAINMENT_ENG_STRING, Consts.ENTERTAINMENT_ENGLISH_URL, Consts.CATEGORY_ENGLISH_STRING));

                categories.Add(new Category(Consts.HEADLINES_ENG_STRING, Consts.HEADLINES_HINDI_STRING, Consts.HEADLINES_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
                categories.Add(new Category(Consts.NATIONAL_ENG_STRING, Consts.NATIONAL_HINDI_STRING, Consts.NATIONAL_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
                categories.Add(new Category(Consts.INTERNATIONAL_ENG_STRING, Consts.INTERNATIONAL_HINDI_STRING, Consts.INTERNATIONAL_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
                categories.Add(new Category(Consts.BUSINESS_ENG_STRING, Consts.BUSINESS_HINDI_STRING, Consts.BUSINESS_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
                categories.Add(new Category(Consts.CURRENT_ENG_STRING, Consts.CURRENT_HINDI_STRING, Consts.CURRENT_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
                categories.Add(new Category(Consts.SPORTS_ENG_STRING, Consts.SPORTS_HINDI_STRING, Consts.SPORTS_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
                categories.Add(new Category(Consts.ENTERTAINMENT_ENG_STRING, Consts.ENTERTAINMENT_HINDI_STRING, Consts.ENTERTAINMENT_HINDI_URL, Consts.CATEGORY_HINDI_STRING));
            }
            return categories;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await DatabaseOperations.Instance.GetCategoriesAsync();
            if (categories == null || categories.Count == 0)
            {
                categories = await LoadCategoriesAsync();
            }
            categories = categories.Where(item => item.Language == _settings.SelectedLanguage).OrderBy(o=>o.Order).ToList();
            return categories;
        }

        public async Task<List<MyApp>> GetMyAppsAsync()
        {
            return await NewsDownloadWrapper.Instance.GetMyAppsAsync();
        }

        public async Task<List<NewsItem>> LoadHeadLinesAsync(string lanaguage)
        {
            List<NewsItem> headLines = new List<NewsItem>();
            if (Categories != null && Categories.Count > 0 && Categories.Find(o => { return o.CategoryName == Consts.HEADLINES_ENG_STRING && o.Language == lanaguage; }) != null)
            {
                headLines = await DatabaseOperations.Instance.GetCategoryArticlesAsync(Categories.Find(o => { return o.CategoryName == Consts.HEADLINES_ENG_STRING && o.Language == lanaguage; }).CategoryName, lanaguage);
            }
            else
            {
                headLines = await DatabaseOperations.Instance.GetCategoryArticlesAsync(Consts.HEADLINES_HINDI_STRING, lanaguage);
            }
            return headLines;
        }

        public bool GetIsNetWorkAvailable()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return true;
            }
            return false;
        }

        public async Task<List<NewsItem>> LoadHeadLinesAsync()
        {
            var articles = new List<NewsItem>();
            if (_categories == null || _categories.Count == 0)
            {
                _categories = await LoadCategoriesAsync();
            }
            bool bfoundCategory = false;
            string language, categoryUrl, categoryName, categoryDecoratedName;
            language = _settings.SelectedLanguage;
            categoryName = Consts.HEADLINES_ENG_STRING;
            categoryDecoratedName = Consts.HEADLINES_ENG_STRING;
            categoryUrl = Consts.HEADLINES_ENGLISH_URL;
            if (_categories != null && _categories.Count > 0)
            {
                var categoryObj = _categories.Find(o => o.CategoryName == Consts.HEADLINES_ENG_STRING && o.Language == _settings.SelectedLanguage);
                if (categoryObj != null)
                {
                    categoryUrl = categoryObj.CategoryUrl;
                    categoryDecoratedName = categoryObj.CategoryDecorativeName;
                    bfoundCategory = true;
                }
            }

            if (bfoundCategory == false)
            {
                categoryUrl = _settings.SelectedLanguage == Consts.CATEGORY_HINDI_STRING ? Consts.HEADLINES_HINDI_URL : Consts.HEADLINES_ENGLISH_URL;
            }

            var fetecheDetails = await GetLastFetchedDateTimeOfCategory(language, categoryName);
            bool bForceFetch = true;
            if (fetecheDetails != null && fetecheDetails.LastFetchedTime != null)
            {
                if ((DateTime.UtcNow - fetecheDetails.LastFetchedTime).TotalMinutes < 5)
                {
                    bForceFetch = false;
                }
            }

            var newArticles = new List<NewsItem>();
            articles = await LoadHeadLinesAsync(_settings.SelectedLanguage);
            if (GetIsNetWorkAvailable() == true)
            {
                if (bForceFetch == true || articles == null || articles.Count == 0)
                {
                    newArticles = await NewsDownloadWrapper.Instance.GetCategoryNewsAsync(categoryUrl);
                    if (articles == null) articles = new List<NewsItem>();
                    if (newArticles != null && newArticles.Count > 0)
                    {
                        await DatabaseOperations.Instance.UpdateArticlesAsync(newArticles);
                        Category category = new Category(categoryName, categoryDecoratedName, categoryUrl, language);
                        category.LastFetchedTime = DateTime.UtcNow;
                        await DatabaseOperations.Instance.UpdateCategoryAsync(category);
                        newArticles.ForEach(o =>
                        {
                            if (articles.Find(x => x.id == o.id) == null)
                                articles.Add(o);
                        });
                    }
                }
            }

            if (articles != null && articles.Count > 0)
                articles = articles.OrderByDescending(o => o.PublishDate).ToList();
            return articles;
        }

        public async Task<List<NewsItem>> GetCategoryNewsItemsAsync(Category category)
        {
            List<NewsItem> newsItems = new List<NewsItem>();
            var fetecheDetails = await GetLastFetchedDateTimeOfCategory(category.Language, category.CategoryName);
            bool bForceFetech = true;
            if (fetecheDetails != null)
            {
                var diff = DateTime.UtcNow - fetecheDetails.LastFetchedTime;
                if (diff.TotalMinutes < 5)
                {
                    bForceFetech = false;
                }
            }
            if (GetIsNetWorkAvailable() == true)
            {
                if (bForceFetech == true)
                {
                    newsItems = await NewsDownloadWrapper.Instance.GetCategoryNewsAsync(category.CategoryUrl);
                    if (newsItems != null && newsItems.Count > 0)
                    {
                        await DatabaseOperations.Instance.UpdateArticlesAsync(newsItems);
                        category.LastFetchedTime = DateTime.UtcNow;
                        await DatabaseOperations.Instance.UpdateCategoryAsync(category);
                    }
                }
            }
            newsItems.Clear();
            newsItems = await DatabaseOperations.Instance.GetCategoryArticlesAsync(category.CategoryName, category.Language);
            return newsItems;
        }

        private async Task<Category> GetLastFetchedDateTimeOfCategory(string language, string categoryName)
        {
            return await DatabaseOperations.Instance.GetCategoryDetailAsync(language, categoryName);
        }

        public void SaveSettings(Settings settings = null)
        {
            if (settings != null)
            {
                Settings = settings;
            }
            DatabaseOperations.Instance.SaveSettings(Settings);
        }
    }
}