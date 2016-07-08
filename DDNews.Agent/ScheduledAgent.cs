using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using DDNews.Model.Services;
using System;
using System.Threading.Tasks;
using DDNews.Model;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace DDNews.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        DataService dataService = new DataService();
        static ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += UnhandledException;
                });
            }
        }

        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        protected override async void OnInvoke(ScheduledTask task)
        {
            if (IsUpdateRequired())
            {
                await UpdateAsync();
            }
            NotifyComplete();
        }

        private bool IsUpdateRequired()
        {
            if (dataService.Settings != null && dataService.Settings.LastNotificationTime != null && dataService.Settings.IsDownloadingArticlesOffline)
            {
                return (DateTime.Now - dataService.Settings.LastNotificationTime).TotalHours >= 1;
            }
            else
            {
                return false;
            }
        }
        private async Task UpdateAsync()
        {
            try
            {
                var headLines = await dataService.LoadHeadLinesAsync();
                if (headLines != null && headLines.Count > 0)
                {
                    UpdateNotification(headLines.Count, headLines[0]);
                }
            }
            catch (Exception)
            {
            }
        }
        private void UpdateNotification(int articleCount, NewsItem article)
        {
            if (article.id != dataService.Settings.LastNotifiedArticleId)
            {
                dataService.Settings.LastNotifiedArticleId = article.id;
                dataService.Settings.LastNotificationTime = DateTime.Now;
                dataService.SaveSettings();
                UpdateLiveTile(article);

                if (dataService.Settings.IsToastNotificationUsed)
                {
                    try
                    {
                        ShellToast shellToast = new ShellToast();
                        shellToast.Content = article.news_title;
                        shellToast.Title = "DD NEWS";
                        shellToast.NavigationUri = new Uri(string.Format("/Views/HomePage.xaml?Id={0}&Language={1}&Toast={2}", article.id, article.language, true), UriKind.Relative);
                        shellToast.Show();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void UpdateLiveTile(NewsItem article)
        {
            try
            {
                if (article != null)
                {
                    ShellTile appTile = ShellTile.ActiveTiles.First();
                    if (appTile != null)
                    {
                        if (string.IsNullOrEmpty(article.image_link))
                        {
                            appTile.Update(new FlipTileData
                            {
                                BackContent = article.news_title,
                                WideBackContent = article.news_title,
                                BackTitle = "DD NEWS"
                            });
                        }
                        else
                        {
                            appTile.Update(new FlipTileData
                            {
                                BackContent = article.news_title,
                                WideBackContent = article.news_title,
                                BackgroundImage = new Uri(article.image_link),
                                WideBackgroundImage = new Uri(article.image_link),
                                BackTitle = "DD NEWS"
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}