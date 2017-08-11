/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebSqlLang.LanguageImplementation
{
    public static class Tokenizer
    {
        public static InputContainer Parse(string formInput)
        {
            var container = new InputContainer();

            Regex.Match(formInput, "select (.+?) HEA")

            return container;
        }
        
        public static bool IsTokenizeble(string formInput)
        {
            if (string.IsNullOrEmpty(formInput)) return false;
            if (!formInput.ToUpper().StartsWith("SELECT")) return false;
            if (!formInput.ToUpper().Contains("FROM")) return false;
            if (!formInput.ToUpper().Contains("HTTP")) return false;

            return true;
        }
    }
}
