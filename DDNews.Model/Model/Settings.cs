using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNews.Model
{
    public enum EnumFontSize
    {
        SMALL,
        NORMAL,
        MEDIUM,
        MEDIUMLARGE,
        LARGE
    }
    public enum EnumFontFamily
    {
        ARBUTUSSLAB,
        SEGOE,
        ROBOTO
    }

    public enum EnumTheme
    {
        DARK,
        LIGHT
    }

    public class Settings : ViewModelBase
    {
        private string _selectedLanguage = Consts.CATEGORY_ENGLISH_STRING;
        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged("SelectedLanguage");
            }
        }
        private string _theme = "White";
        public string Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                RaisePropertyChanged("Theme");
            }
        }
        private double _fontSize = 20;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                RaisePropertyChanged("FontSize");
            }
        }

        private string _fontFamily = "Segoe WP";
        public string FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                _fontFamily = value;
                RaisePropertyChanged("FontFamily");
            }
        }

        private bool _isDownloadingArticlesOffline = true;
        public bool IsDownloadingArticlesOffline
        {
            get
            {
                return _isDownloadingArticlesOffline;
            }
            set
            {
                _isDownloadingArticlesOffline = value;
                RaisePropertyChanged("IsDownloadingArticlesOffline");
            }
        }

        private bool _isToastNotificationUsed = true;
        public bool IsToastNotificationUsed
        {
            get
            {
                return _isToastNotificationUsed;
            }
            set
            {
                _isToastNotificationUsed = value;
                RaisePropertyChanged("IsToastNotificationUsed");
            }
        }
        private DateTime _lastNotificationTime = DateTime.Now.AddHours(-1);
        public DateTime LastNotificationTime
        {
            get
            {
                return _lastNotificationTime;
            }
            set
            {
                _lastNotificationTime = value;
                RaisePropertyChanged("LastNotificationTime");
            }
        }

        private int _lastNotifiedArticleId = 0;
        public int LastNotifiedArticleId
        {
            get
            {
                return _lastNotifiedArticleId;
            }
            set
            {
                _lastNotifiedArticleId = value;
                RaisePropertyChanged("LastNotifiedArticleId");
            }
        }

        //private string _lastNotifiedArticle = "";
        //public string LastNotifiedArticle
        //{
        //    get
        //    {
        //        return _lastNotifiedArticle;
        //    }
        //    set
        //    {
        //        _lastNotifiedArticle = value;
        //        RaisePropertyChanged("LastNotifiedArticle");
        //    }
        //}
    }
}