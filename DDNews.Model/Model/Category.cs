using SQLite;
using System;
using System.Collections.Generic;

namespace DDNews.Model
{
    public class Category
    {
        public Category(string categoryName, string categoryDecorativeName, string categoryUrl, string language)
        {
            CategoryName = categoryName;
            CategoryDecorativeName = categoryDecorativeName;
            CategoryUrl = categoryUrl;
            Language = language;
        }

        public Category()
        {
        }

        
        public string CategoryName
        {
            get; set;
        }

        [PrimaryKey]
        public string CategoryDecorativeName
        {
            get;set;
        }

        public string CategoryUrl
        {
            get; set;
        }

        public string Language
        {
            get; set;
        }

        public DateTime LastFetchedTime
        {
            get; set;
        }
        public int Order { get; set; }
    }

    public class ListOfCategories
    {
        public List<Category> categories { get; set; }
    }
}
