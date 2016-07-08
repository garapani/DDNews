using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using DDNews.Helper;

namespace DDNews.Views
{
    public partial class SharePage : PhoneApplicationPage
    {
        public SharePage()
        {
            InitializeComponent();
        }
        private void ShareViaSocialNetwork_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = "DDNEWS App";
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            shareLinkTask.LinkUri = new Uri(storeURI);
            shareLinkTask.Message = "\"DDNEWS App\" for Windows phone 8";
            shareLinkTask.Show();
        }

        private void ShareViaMail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            EmailComposeTask emailComposeTask = new EmailComposeTask()
            {
                Subject = "Try \" DDNews App\" for windows phone 8",
                Body = "\" DDNews App\" is a easy way to read english and hindi news from DD.Please click on this link " + storeURI,
            };

            emailComposeTask.Show();
        }

        private void ShareViaSMS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            SmsComposeTask smsComposeTask = new SmsComposeTask()
            {
                Body = "Try \"DD News App\" for windows phone 8. It's great!. " + storeURI,
            };

            smsComposeTask.Show();
        }
    }
}