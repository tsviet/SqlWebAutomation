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
            var parsedColumnList = new List<string>();
            var container = new InputContainer();
            //Parse columns from input string
            var columnsString = Regex.Match(formInput, "select (.+?) using").Groups[1].Value;
            //Empty user input base case
            if (string.IsNullOrWhiteSpace(columnsString))
            {
                container.errors.Add("Please type your code in a window above!");
                //return container;
            }
            //Case of * or [All] 
            if (columnsString.Trim() == "*" || columnsString.Trim().ToLower() == "[all]")
            {
                parsedColumnList.Add(columnsString.Trim().ToLower());
            }

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
