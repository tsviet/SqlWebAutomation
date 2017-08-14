/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System.Collections.Generic;

namespace WebSqlLang.LanguageImplementation
{
    public class Where
    {
        public string Seperator { get; set; }
        public List<Limits> Data { get; set; }
    }

    public class Limits
    {
        public string Name { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
