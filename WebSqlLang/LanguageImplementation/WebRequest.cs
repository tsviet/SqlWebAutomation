/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace WebSqlLang.LanguageImplementation
{
    public class WebRequest
    {
        private string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1";
        private readonly InputContainer container = null;
        private readonly Uri inputUrl;
        public string html { get; private set; }

        public WebRequest(InputContainer container)
        {
            this.container = container;
            inputUrl = new Uri(this.container.Url);
        }

        public void GetHtml()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers["User-Agent"] = userAgent;
                webClient.DownloadStringCompleted += wc_DownloadStringCompleted;
                webClient.DownloadStringAsync(inputUrl);
            }
        }

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            html = e.Result;
        }
    }
}
