using DDNews.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DDNews.Converter
{
    public class CanFontVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is string && (string)value == Consts.CATEGORY_ENGLISH_STRING) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            //return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
