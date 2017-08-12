/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSqlLang.LanguageImplementation
{
    //Class container that holds links from html
    public class Links : IData
    {
        public int RowNumber { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
