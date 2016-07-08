using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DDNews.ViewModel;

namespace DDNews
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                string strId, language;
                NavigationContext.QueryString.TryGetValue("Id", out strId);
                NavigationContext.QueryString.TryGetValue("Language", out language);
                if (!string.IsNullOrEmpty(strId) && !string.IsNullOrEmpty(language))
                {
                    ViewModelLocator viewModelLocator = App.Current.Resources["Locator"] as ViewModelLocator;
                    await viewModelLocator.Main.LoadHeadLinesAsync();
                    viewModelLocator.Main.SetCurrentPreviousandNextArticle(int.Parse(strId), language);
                    NavigationService.Navigate(new Uri(string.Format("/Views/ArticlePage.xaml?Id={0}&Language={1}", int.Parse(strId), language), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
}