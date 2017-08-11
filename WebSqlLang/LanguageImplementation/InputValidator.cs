/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WebSqlLang.LanguageImplementation
{
    public static class InputValidator
    {
        public static Dictionary<string, List<string>> ColumnsMapValidator = new Dictionary<string, List<string>>
        {
            { "headers", new List<string> {"name","value"} },
            { "links", new List<string> { "domain","name","url","text"} },
            { "tables", new List<string> {"table_id","row_id","c*"} }
        };
    }
}
