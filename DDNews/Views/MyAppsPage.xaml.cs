using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace DDNews.Views
{
    public partial class MyAppsPage : PhoneApplicationPage
    {
        public MyAppsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationService.GoBack();
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //if(e.NavigationMode == NavigationMode.New)
            //{
            //    NavigationService.GoBack();
            //}
        }
    }
}