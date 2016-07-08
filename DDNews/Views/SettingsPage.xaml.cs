using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DDNews.Model;
using DDNews.ViewModel;

namespace DDNews.Views
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        ViewModelLocator viewModelLocator = App.Current.Resources["Locator"] as ViewModelLocator;
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (viewModelLocator != null && viewModelLocator.Main != null && viewModelLocator.Main.Settings != null)
            {
                if(viewModelLocator.Main.Settings.SelectedLanguage == Consts.CATEGORY_HINDI_STRING)
                {
                    SelectEnglishLanguage.IsChecked = false;
                    SelectHindiLanguage.IsChecked = true;
                }
                else
                {
                    SelectEnglishLanguage.IsChecked = true;
                    SelectHindiLanguage.IsChecked = false;
                }
            }
            else
            {
                SelectEnglishLanguage.IsChecked = true;
                SelectHindiLanguage.IsChecked = false;
            }
            base.OnNavigatedTo(e);
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);
            if (li.Content.ToString() == Consts.CATEGORY_HINDI_STRING)
            {
                if(viewModelLocator != null && viewModelLocator.Main != null && viewModelLocator.Main.Settings != null)
                {
                    viewModelLocator.Main.Settings.SelectedLanguage = Consts.CATEGORY_HINDI_STRING;
                    viewModelLocator.Main.SaveSettings();
                }
            }
            else
            {
                if (viewModelLocator != null && viewModelLocator.Main != null && viewModelLocator.Main.Settings != null)
                {
                    viewModelLocator.Main.Settings.SelectedLanguage = Consts.CATEGORY_ENGLISH_STRING;
                    viewModelLocator.Main.SaveSettings();
                }
            }
        }
    }
}