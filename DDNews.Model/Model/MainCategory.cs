using System.Collections.ObjectModel;

namespace DDNews.Model
{
    public class MainCategory
    {
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public MainCategory(string categoryName, string categoryImage)
        {
            CategoryImage = categoryImage;
            CategoryName = categoryName;
        }
        
        public MainCategory()
        { }
    }
}
