/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                    list.AddRange(ParseLinksFromHtml(list, container.Url, html));
                }
                
            }

            return list;
        }

        private static IEnumerable<IData> ParseLinksFromHtml(ICollection<IData> list, string containerUrl, string html)
        {
            
            var count = 0;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            //Get all A tags from html
            var aTagResults = doc.DocumentNode.SelectNodes("//a");
            GetLinksObject(ref list, aTagResults, ref count, containerUrl, "href");

            //Get all A tags from html
            var imgTagResults = doc.DocumentNode.SelectNodes("//img");
            GetLinksObject(ref list, imgTagResults, ref count, containerUrl, "src");

            //Get all A tags from html
            var linkTagResults = doc.DocumentNode.SelectNodes("//link");
            GetLinksObject(ref list, linkTagResults, ref count, containerUrl, "src");
            return list;
        }

        private static void GetLinksObject(ref ICollection<IData> list, HtmlNodeCollection aTagResults, ref int count, string containerUrl, string param)
        {

            var domainUrl = new Uri(containerUrl).Host;
            var shemeUrl = new Uri(containerUrl).Scheme;

            if (aTagResults == null) return;
            foreach (var result in aTagResults)
            {
                var urlWithOutDomain = result.Attributes[param]?.Value;
                if (!string.IsNullOrEmpty(urlWithOutDomain))
                {
                    var currentLinkObject = new Links
                    {
                        RowNumber = ++count,
                        Url = !urlWithOutDomain.StartsWith("http") ? $"{shemeUrl}://{domainUrl}{urlWithOutDomain}" : urlWithOutDomain,
                        Name = Regex.Replace(result.InnerText.Trim(), "\\s\\s+", ""),
                        Type = result.Attributes["type"]?.Value,
                    };
                    list.Add(currentLinkObject);
                }
            }
        }

        public static List<IData> ConvertToHeaders(InputContainer container, Dictionary<string, string> headers)
        {
            List<IData> list = new List<IData>();
            if (headers == null) return list;

            foreach (var header in headers)
            {
                var obj = new Headers
                {
                    Name = header.Key,
                    Value = header.Value
                };
                list.Add(obj);
            }
            return list;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data, InputContainer container)
        {
            //https://stackoverflow.com/questions/29898412/convert-listt-to-datatable-including-t-customclass-properties
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var outputTable = new DataTable();
            if (container.ColumnsMap.FirstOrDefault().Value.Contains("*"))
            {
                foreach (PropertyDescriptor prop in properties)
                {
                    outputTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }
            foreach (PropertyDescriptor prop in properties)
            {
                //Will work only for sinle method in query will need to be rebuilded when JOIN will be designed
                if (container.ColumnsMap.FirstOrDefault().Value.Contains(prop.Name.ToLower()))
                {
                    var name = prop.Name;
                    if (outputTable.Columns.Contains(prop.Name))
                    {
                        name = prop.Name + "_1";
                    }
                    outputTable.Columns.Add(name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }

            foreach (var item in data)
            {
                var row = outputTable.NewRow();
                foreach (DataColumn column in outputTable.Columns)
                {
                    var prop = properties.Find(column.ColumnName.Replace("_1", ""), false);
                    row[column.ColumnName] = prop.GetValue(item) ?? DBNull.Value;
                }
                outputTable.Rows.Add(row);
            }
            return outputTable;

        }
    }

}
