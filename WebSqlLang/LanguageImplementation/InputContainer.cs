/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSqlLang.LanguageImplementation
{
    public class InputContainer
    {

        public InputContainer()
        {
            Url = "";
            ColumnsMap = new Dictionary<string, List<string>>();
            Errors = new List<string>();
        }
        public string Url { get; set; }
        public Dictionary<string, List<string>> ColumnsMap { get; set; }
        public List<string> Errors { get; set; }

    }
}
