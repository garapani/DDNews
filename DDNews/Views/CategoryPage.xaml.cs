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

namespace DDNews.Views
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        public CategoryPage()
        {
            InitializeComponent();
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModelLocator viewModelLocator = App.Current.Resources["Locator"] as ViewModelLocator;
            viewModelLocator.Main.CurrentArticle = (Model.NewsItem)listOfArticles.SelectedItem;
            viewModelLocator.Main.ReadCurrentArticle();
            listOfArticles.SelectedItem = null;
        }
    }
}