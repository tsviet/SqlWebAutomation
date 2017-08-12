/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace WebSqlLang.LanguageImplementation
{
    public class WebRequest : INotifyPropertyChanged
    {
        private readonly string _userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1";
        private readonly InputContainer container = null;
        private readonly Uri _inputUrl;
        private string _html = "";
        public string Html {
            get { return _html; }
            private set
            {
                _html = value;
                //On value change call parser to get proper data

                OnPropertyChanged("Html");
                //if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value))
                //{
                //HtmlHelper.Parse(container, value);
                //}
            }
        }

        public WebRequest(InputContainer container)
        {
            this.container = container;
            _inputUrl = new Uri(this.container.Url);
        }

        public void GetHtml()
        {
            //Getting some code from here https://stackoverflow.com/questions/7129256/how-to-use-string-in-webclient-downloadstringasync-url (keyboardP)
            using (var webClient = new WebClient())
            {
                webClient.Headers["User-Agent"] = _userAgent;
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted).Invoke;
                webClient.DownloadStringAsync(_inputUrl);
            }
        }

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Html = e.Result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
