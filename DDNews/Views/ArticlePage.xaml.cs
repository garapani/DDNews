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
using DDNews.Model;
using DDNews.Model.Services;
using System.Windows.Media;

namespace DDNews.Views
{
    public partial class ArticlePage : PhoneApplicationPage
    {
        private readonly MainViewModel _mainViewModel;
        private static int previousIndex = 0;

        public ArticlePage()
        {
            InitializeComponent();
            ViewModelLocator viewModelLocator = App.Current.Resources["Locator"] as ViewModelLocator;
            _mainViewModel = viewModelLocator.Main;
        }

        private void UpdateFontStyleSection()
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                btnSmallFont.Background = new SolidColorBrush(Colors.Transparent);
                btnNormalFont.Background = new SolidColorBrush(Colors.Transparent);
                btnMediumFont.Background = new SolidColorBrush(Colors.Transparent);
                btnMediumLargeFont.Background = new SolidColorBrush(Colors.Transparent);
                btnLargeFont.Background = new SolidColorBrush(Colors.Transparent);

                switch (Utils.Instance.ConvertFontSize(_mainViewModel.Settings.FontSize))
                {
                    case EnumFontSize.SMALL:
                        {
                            btnSmallFont.Background = new SolidColorBrush(Colors.Gray);
                            break;
                        }
                    case EnumFontSize.NORMAL:
                        {
                            btnNormalFont.Background = new SolidColorBrush(Colors.Gray);
                            break;
                        }
                    case EnumFontSize.MEDIUM:
                        {
                            btnMediumFont.Background = new SolidColorBrush(Colors.Gray);
                            break;
                        }
                    case EnumFontSize.MEDIUMLARGE:
                        {
                            btnMediumLargeFont.Background = new SolidColorBrush(Colors.Gray);
                            break;
                        }
                    case EnumFontSize.LARGE:
                        {
                            btnLargeFont.Background = new SolidColorBrush(Colors.Gray);
                            break;
                        }
                    default:
                        break;
                }

                btnArbutusSlabFont.Background = new SolidColorBrush(Colors.Transparent);
                btnSegoeWPFont.Background = new SolidColorBrush(Colors.Transparent);
                btnRobotoFont.Background = new SolidColorBrush(Colors.Transparent);
                switch (Utils.Instance.ConvertFontFamily(_mainViewModel.Settings.FontFamily))
                {
                    case EnumFontFamily.ARBUTUSSLAB:
                        btnArbutusSlabFont.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case EnumFontFamily.ROBOTO:
                        btnRobotoFont.Background = new SolidColorBrush(Colors.Gray);
                        break;
                    case EnumFontFamily.SEGOE:
                        btnSegoeWPFont.Background = new SolidColorBrush(Colors.Gray);
                        break;
                }

                btnDark.Background = new SolidColorBrush(Colors.Transparent);
                btnLight.Background = new SolidColorBrush(Colors.Transparent);
              
                firstPivotStory.FontSize = _mainViewModel.Settings.FontSize;
                secondPivotStory.FontSize = _mainViewModel.Settings.FontSize;
                thirdPivotStory.FontSize = _mainViewModel.Settings.FontSize;
                firstPivotStory.FontFamily = new FontFamily(_mainViewModel.Settings.FontFamily);
                secondPivotStory.FontFamily = new FontFamily(_mainViewModel.Settings.FontFamily);
                thirdPivotStory.FontFamily = new FontFamily(_mainViewModel.Settings.FontFamily);

                if (_mainViewModel.Settings.Theme.ToLower() == "black")
                {
                    btnDark.Background = new SolidColorBrush(Colors.Gray);                    
                    firstPivotStory.Foreground = new SolidColorBrush(Colors.White);
                    secondPivotStory.Foreground = new SolidColorBrush(Colors.White);
                    thirdPivotStory.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    btnLight.Background = new SolidColorBrush(Colors.Gray);
                    firstPivotStory.Foreground = new SolidColorBrush(Colors.Black);
                    secondPivotStory.Foreground = new SolidColorBrush(Colors.Black);
                    thirdPivotStory.Foreground = new SolidColorBrush(Colors.Black);
                }

                NewsItem firstNews = firstPivotStory.DataContext as NewsItem;
                if (firstNews != null)
                {
                    firstPivotStory.Html = "";
                    firstPivotStory.Html = firstNews.full_description;
                }
                NewsItem secondNews = firstPivotStory.DataContext as NewsItem;
                if (secondNews != null)
                {
                    secondPivotStory.Html = "";
                    secondPivotStory.Html = secondNews.full_description;
                }
                NewsItem thirdNews = firstPivotStory.DataContext as NewsItem;
                if (thirdNews != null)
                {
                    thirdPivotStory.Html = "";
                    thirdPivotStory.Html = thirdNews.full_description;
                }
                _mainViewModel.ShowStyling = false;
                _mainViewModel.SaveSettings();
            }
        }

        private void pivotControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Pivot pivotcontrol = sender as Pivot;
            switch (pivotcontrol.SelectedIndex)
            {
                case 0:
                    {
                        if (_mainViewModel != null)
                        {
                            secondScrollBar.Visibility = Visibility.Collapsed;
                            thirdScrollBar.Visibility = Visibility.Collapsed;


                            scrollViewer.Visibility = Visibility.Visible;
                            if (previousIndex == 2)
                            {
                                if (_mainViewModel.NextArticle != null)
                                {
                                    onePivot.DataContext = _mainViewModel.NextArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.NextArticle);
                                }
                            }
                            else if (previousIndex == 1)
                            {
                                if (_mainViewModel.PreviousArticle != null)
                                {
                                    onePivot.DataContext = _mainViewModel.PreviousArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.PreviousArticle);
                                }
                            }
                            else
                            {
                                if (_mainViewModel.CurrentArticle != null)
                                {
                                    onePivot.DataContext = _mainViewModel.CurrentArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.CurrentArticle);
                                }
                            }
                        }
                        previousIndex = 0;
                    }
                    break;

                case 1:
                    {
                        if (_mainViewModel != null)
                        {
                            scrollViewer.Visibility = Visibility.Collapsed;
                            secondScrollBar.Visibility = Visibility.Visible;
                            thirdScrollBar.Visibility = Visibility.Collapsed;

                            secondPivot.DataContext = null;
                            if (previousIndex == 0)
                            {
                                if (_mainViewModel.NextArticle != null)
                                {
                                    secondPivot.DataContext = _mainViewModel.NextArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.NextArticle);
                                }
                            }
                            else if (previousIndex == 2)
                            {
                                if (_mainViewModel.PreviousArticle != null)
                                {
                                    secondPivot.DataContext = _mainViewModel.PreviousArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.PreviousArticle);
                                }
                            }
                        }
                        previousIndex = 1;
                    }
                    break;

                case 2:
                    {
                        if (_mainViewModel != null)
                        {
                            scrollViewer.Visibility = Visibility.Collapsed;
                            secondScrollBar.Visibility = Visibility.Collapsed;
                            thirdScrollBar.Visibility = Visibility.Visible;

                            thirdPivot.DataContext = null;
                            if (previousIndex == 1)
                            {
                                if (_mainViewModel.NextArticle != null)
                                {
                                    thirdPivot.DataContext = _mainViewModel.NextArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.NextArticle);
                                }
                            }
                            else if (previousIndex == 0)
                            {
                                if (_mainViewModel.PreviousArticle != null)
                                {
                                    thirdPivot.DataContext = _mainViewModel.PreviousArticle;
                                    _mainViewModel.SetCurrentPreviousandNextArticle(_mainViewModel.PreviousArticle);
                                }
                            }
                        }
                        previousIndex = 2;
                    }
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateFontStyleSection();
            base.OnNavigatedTo(e);
        }

        private void btnDark_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.Theme = "Black";
            }
            UpdateFontStyleSection();
        }
        private void btnLight_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.Theme = "White";
            }
            UpdateFontStyleSection();
        }

        private void btnArbutusSlabFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontFamily = btnArbutusSlabFont.FontFamily.ToString();
            }
            UpdateFontStyleSection();
        }

        private void btnSegoeWPFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontFamily = btnSegoeWPFont.FontFamily.ToString();
            }
            UpdateFontStyleSection();
        }

        private void btnRobotoFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontFamily = btnRobotoFont.FontFamily.ToString();
            }
            UpdateFontStyleSection();
        }

        private void btnSmallFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontSize = btnSmallFont.FontSize;
            }
            UpdateFontStyleSection();
        }

        private void btnNormalFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontSize = btnNormalFont.FontSize;
            }
            UpdateFontStyleSection();
        }

        private void btnMediumFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontSize = btnMediumFont.FontSize;
            }
            UpdateFontStyleSection();
        }

        private void btnMediumLargeFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontSize = btnMediumLargeFont.FontSize;
            }
            UpdateFontStyleSection();
        }

        private void btnLargeFont_Click(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel != null && _mainViewModel.Settings != null)
            {
                _mainViewModel.Settings.FontSize = btnLargeFont.FontSize;
            }
            UpdateFontStyleSection();
        }
    }
}