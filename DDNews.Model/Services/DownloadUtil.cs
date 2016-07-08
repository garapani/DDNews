using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DDNews.Model.Services
{
    public class DownloadUtil
    {
        public async Task<string> DownloadStringAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
            try
            {
                HttpClient client = new HttpClient(handler);
                var downloadedString = await client.GetStringAsync(url);
                return downloadedString;
            }
            catch (Exception ex)
            {

            }
            return "";
        }
    }
}
