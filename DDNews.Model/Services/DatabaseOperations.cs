using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DDNews.Model.Services
{
    public class DatabaseOperations
    {
        private static DatabaseOperations _instance = null;

        private SQLite.SQLiteAsyncConnection _dbConnection;

        private DatabaseOperations()
        {
            _dbConnection = new SQLite.SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "articlesDb.sqlite"));
        }

        public static DatabaseOperations Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseOperations();
                    TaskFactory factory = new TaskFactory();
                    factory.StartNew(async () =>
                    {
                        await _instance.CreateDatabaseIfNotCreated();
                    });
                }
                return _instance;
            }
            private set { }
        }

        private async Task CreateDatabaseIfNotCreated()
        {
            try
            {
                await _dbConnection.CreateTablesAsync<NewsItem, Category>();
            }
            catch (Exception)
            {
            }
        }
        public bool IsCategoriesDbAlreadyCreated()
        {
            if (_dbConnection != null)
            {
                var temp = _dbConnection.Table<Category>();
                if (temp != null)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task AddCategoryAsync(Category categoryDetails)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryDetails.CategoryName.ToLower() && category.Language.ToLower() == categoryDetails.Language.ToLower() select category;
                    if (allCategories != null && await allCategories.CountAsync() >= 1)
                    {
                        await _dbConnection.UpdateAsync(categoryDetails);
                    }
                    else
                    {
                        await _dbConnection.InsertAsync(categoryDetails);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }
        public async Task AddOrUpdateArticleAsync(NewsItem articleToAddorUpdate)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<NewsItem>() where article.id == articleToAddorUpdate.id select article;
                    if (allArticles != null && await allArticles.CountAsync() >= 1)
                    {
                        await _dbConnection.UpdateAsync(articleToAddorUpdate);
                    }
                    else
                    {
                        await _dbConnection.InsertAsync(articleToAddorUpdate);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteAllArticlesAsync()
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = await _dbConnection.Table<NewsItem>().ToListAsync();
                    foreach (var article in allArticles)
                    {
                        await _dbConnection.DeleteAsync(article);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteArticleAsync(int Id, string category)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<NewsItem>() where article.id == Id select article;
                    if (allArticles != null && await allArticles.CountAsync() >= 1)
                    {
                        var temp = await allArticles.FirstOrDefaultAsync();
                        await _dbConnection.DeleteAsync(temp);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteArticlesAsync(string category, string language)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<NewsItem>() where article.category.ToLower() == category.ToLower() && language.ToLower() == article.language.ToLower() select article;
                    foreach (var article in await allArticles.ToListAsync())
                    {
                        await _dbConnection.DeleteAsync(article);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task DeleteExpiredArticlesAsync()
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = await _dbConnection.Table<NewsItem>().ToListAsync();

                    foreach (var article in allArticles)
                    {
                        if (article.publish_date != null)
                        {
                            var diff = DateTime.Now - article.PublishDate;
                            if (diff.TotalHours > 240)
                            {
                                await _dbConnection.DeleteAsync(article);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
        }

        public async Task<NewsItem> GetArticleAsync(int Id, string category = null, string language = null)
        {
            NewsItem tempArticle = null;
            if (_dbConnection != null)
            {
                try
                {
                    var allArticles = from article in _dbConnection.Table<NewsItem>() where article.id == Id && article.category.ToLower() == category.ToLower() && article.language.ToLower() == language.ToLower() select article;
                    tempArticle = await allArticles.FirstOrDefaultAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return tempArticle;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            List<Category> list = null;
            if (_dbConnection != null)
            {
                try
                {
                    list = await _dbConnection.Table<Category>().ToListAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return list;
        }

        public async Task<List<NewsItem>> GetCategoryArticlesAsync(string category, string language)
        {
            List<NewsItem> list = null;
            if (_dbConnection != null)
            {
                try
                {
                    var articles = from article in _dbConnection.Table<NewsItem>() where article.category != null select article;
                    var temp = await articles.ToListAsync();
                    var allArticles = from article in _dbConnection.Table<NewsItem>() where article.category.ToLower() == category.ToLower() && language.ToLower() == article.language.ToLower() select article;
                    if (allArticles != null)
                    {
                        list = await allArticles.ToListAsync();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return list;
        }


        public async Task<List<NewsItem>> GetAllArticlesAsync()
        {
            List<NewsItem> list = null;
            if (_dbConnection != null)
            {
                try
                {
                    list = await _dbConnection.Table<NewsItem>().ToListAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return list;
        }

        public async Task<bool> UpdateArticlesAsync(List<NewsItem> items)
        {
            try
            {
                var allArticles = await _dbConnection.Table<NewsItem>().ToListAsync();
                if (allArticles != null)
                {
                    foreach (var item in items)
                    {
                        if (allArticles.Find(o => o.id == item.id) == null)
                            await _dbConnection.InsertAsync(item);
                        else
                            await _dbConnection.UpdateAsync(item);
                    }
                }
                else
                {
                    await _dbConnection.InsertAllAsync(items);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<Category> GetCategoryDetailAsync(string language, string categoryName)
        {
            Category categoryObj = null;
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryName.ToLower() && category.Language.ToLower() == language.ToLower() select category;
                    if (allCategories != null)
                    {
                        categoryObj = await allCategories.FirstOrDefaultAsync();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get sqlite connection");
            }
            return categoryObj;
        }

        public async Task UpdateCategoryAsync(Category categoryModel)
        {
            if (_dbConnection != null)
            {
                try
                {
                    var allCategories = from category in _dbConnection.Table<Category>() where category.CategoryName.ToLower() == categoryModel.CategoryName.ToLower() && category.Language.ToLower() == categoryModel.Language.ToLower() select category;
                    if (allCategories != null)
                    {
                        Category tempCategory = await allCategories.FirstOrDefaultAsync();
                        if (tempCategory != null)
                        {
                            tempCategory.CategoryName = categoryModel.CategoryName;
                            tempCategory.CategoryDecorativeName = categoryModel.CategoryDecorativeName;
                            tempCategory.CategoryUrl = categoryModel.CategoryUrl;
                            tempCategory.Language = categoryModel.Language;
                            tempCategory.Order = categoryModel.Order;
                            if (categoryModel.LastFetchedTime != null)
                                tempCategory.LastFetchedTime = categoryModel.LastFetchedTime;
                            await _dbConnection.UpdateAsync(tempCategory);
                        }
                        else
                        {
                            await _dbConnection.InsertAsync(categoryModel);
                        }
                    }
                    else
                    {
                        await _dbConnection.InsertAsync(categoryModel);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("failed to get db connection");
            }
        }

        public Settings GetSettings()
        {
            var settings = new Settings();
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Settings"))
            {
                settings = (Settings)IsolatedStorageSettings.ApplicationSettings["Settings"];
            }
            else
            {
                settings = new Settings();
            }
            return settings;
        }

        public void SaveSettings(Settings settings)
        {
            if (settings != null)
            {
                IsolatedStorageSettings.ApplicationSettings["Settings"] = settings;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
    }
}
