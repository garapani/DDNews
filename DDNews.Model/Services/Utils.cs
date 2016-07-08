using System;
using System.Windows.Media;

namespace DDNews.Model.Services
{
    public sealed class Utils
    {
        private static Utils _instance;
        public static Utils Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Utils();
                return _instance;
            }
            private set { }
        }

        private Utils()
        {
        }

        public EnumFontSize ConvertFontSize(double fontSize)
        {
            fontSize = Math.Truncate(fontSize * 1000) / 1000;
            EnumFontSize fontSizeEnum = EnumFontSize.NORMAL;
            if (fontSize == 18.666)
            {
                fontSizeEnum = EnumFontSize.SMALL;
            }
            else if (fontSize == 20)
            {
                fontSizeEnum = EnumFontSize.NORMAL;
            }
            else if (fontSize == 22.666)
            {
                fontSizeEnum = EnumFontSize.MEDIUM;
            }
            else if (fontSize == 25.333)
            {
                fontSizeEnum = EnumFontSize.MEDIUMLARGE;
            }
            else if (fontSize == 32)
            {
                fontSizeEnum = EnumFontSize.LARGE;
            }
            return fontSizeEnum;
        }

        public EnumFontFamily ConvertFontFamily(string fontFamily)
        {
            EnumFontFamily fontFamilyEnum = EnumFontFamily.SEGOE;
            if(fontFamily.ToLower().Contains("arbutus"))
            {
                fontFamilyEnum = EnumFontFamily.ARBUTUSSLAB;
            }
            else if(fontFamily.ToLower().Contains("segoe"))
            {
                fontFamilyEnum = EnumFontFamily.SEGOE;
            }
            else if(fontFamily.ToLower().Contains("roboto"))
            {
                fontFamilyEnum = EnumFontFamily.ROBOTO;
            }
            return fontFamilyEnum;
        }

        public EnumTheme ConvertTheme(string theme)
        {
            EnumTheme themeEnum = EnumTheme.LIGHT;
            if (theme.ToLower() == "Black")
                themeEnum = EnumTheme.DARK;
            return themeEnum;
        }
    }
}
