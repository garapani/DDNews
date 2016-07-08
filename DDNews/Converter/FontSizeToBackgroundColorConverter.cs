using DDNews.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DDNews.Converter
{
    public sealed class FontSizeToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ViewModelLocator viewModelLocator = App.Current.Resources["Locator"] as ViewModelLocator;
            if (viewModelLocator != null && viewModelLocator.Main != null && viewModelLocator.Main.Settings != null)
            {
                if (viewModelLocator.Main.Settings.FontSize == (double)value)
                {
                    return new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
            else
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
