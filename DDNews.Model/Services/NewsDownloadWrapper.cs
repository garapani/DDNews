using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DDNews.Model.Services
{
    public class NewsDownloadWrapper
    {
        private static NewsDownloadWrapper _instance;
        public static NewsDownloadWrapper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NewsDownloadWrapper();
                return _instance;
            }
            private set { }
        }

        private NewsDownloadWrapper() { }

        public async Task<List<NewsItem>> GetCategoryNewsAsync(string categoryUrl)
        {
            List<NewsItem> newsItems = new List<NewsItem>();
            DownloadUtil downloadUtil = new DownloadUtil();
            var downloadedString = await downloadUtil.DownloadStringAsync(categoryUrl);
            if (!String.IsNullOrEmpty(downloadedString) && downloadedString.Trim() != "{}")
            {
                newsItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NewsItem>>(downloadedString);
            }
            return newsItems;
        }

        public async Task<List<MyApp>> GetMyAppsAsync()
        {
            List<MyApp> listOfApps = new List<MyApp>();
            var myAppsUrl = string.Format("https://thehinduadrotator.blob.core.windows.net/myapps/myapps.json");
            DownloadUtil downloadUtil = new DownloadUtil();
            var downloadedString = await downloadUtil.DownloadStringAsync(myAppsUrl);
            var myApps = JsonConvert.DeserializeObject<MyApps>(downloadedString);
            if (myApps != null)
                listOfApps = myApps.myapps;
            return listOfApps;
        }
    }
}
