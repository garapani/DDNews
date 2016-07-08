using SQLite;
using System;

namespace DDNews.Model
{
    public class NewsItem
    {
        public string news_title { get; set; }
        public string thumbnail_link { get; set; }
        public string obj_type { get; set; }
        public string full_description { get; set; }
        public string language { get; set; }
        public string short_description { get; set; }
        public string publish_date { get; set; }
        public DateTime PublishDate
        {
            get
            {
                DateTime date = DateTime.Now;
                DateTime.TryParse(publish_date, out date);
                return date;
            }
            private set { }
        }

        public string image_link { get; set; }
        public string category { get; set; }
        public string decorateCategory
        {
            get
            {
                if (language == Consts.CATEGORY_HINDI_STRING)
                {
                    switch (category)
                    {
                        case Consts.BUSINESS_ENG_STRING:
                            return Consts.BUSINESS_HINDI_STRING;
                        case Consts.CURRENT_ENG_STRING:
                            return Consts.CURRENT_HINDI_STRING;
                        case Consts.ENTERTAINMENT_ENG_STRING:
                            return Consts.ENTERTAINMENT_HINDI_STRING;
                        case Consts.HEADLINES_ENG_STRING:
                            return Consts.HEADLINES_HINDI_STRING;
                        case Consts.INTERNATIONAL_ENG_STRING:
                            return Consts.INTERNATIONAL_HINDI_STRING;
                        case Consts.NATIONAL_ENG_STRING:
                            return Consts.NATIONAL_HINDI_STRING;
                        case Consts.SPORTS_ENG_STRING:
                            return Consts.SPORTS_HINDI_STRING;
                        default:
                            return Consts.HEADLINES_HINDI_STRING;
                    }
                }
                else
                {
                    return language;
                }
            }
            private set { }
        }

        [PrimaryKey]
        public int id { get; set; }
    }
}