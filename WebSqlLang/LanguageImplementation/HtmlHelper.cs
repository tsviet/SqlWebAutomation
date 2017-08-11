/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace WebSqlLang.LanguageImplementation
{
    //This class helps to parse defferent html based on container method
    public static class HtmlHelper
    {
        public static List<IData> Parse(InputContainer container, string html)
        {
            List<IData> list = new List<IData>();
            //Get name of map and columns
            foreach (var map in container.ColumnsMap)
            {
                if (map.Key.ToLower() == "links")
                {
                    list.AddRange(ParseLinksFromHtml(list, map, html));
                }
                
            }
            return list;

        }

        private static List<IData> ParseLinksFromHtml(List<IData> list, KeyValuePair<string, List<string>> map, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var results = doc.DocumentNode.SelectNodes("a");


        }
    }

}
