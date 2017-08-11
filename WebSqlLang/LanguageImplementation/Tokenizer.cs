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
            var parsedMethod = "";
            var container = new InputContainer();
            
            //Empty user input base case
            if (string.IsNullOrWhiteSpace(formInput))
            {
                container.Errors.Add("Please type your code in a window above!");
            }

            if (container.Errors.Count > 0) return container;
            GetColumnsFromString(formInput, container, parsedColumnList);


            if (container.Errors.Count > 0) return container;
            GetMethodFromString(formInput, container, ref parsedMethod);

            if (container.Errors.Count > 0) return container;
            GetUrlFromString(formInput, container);

            //Fill Column map with method and columns
            container.ColumnsMap.Add(parsedMethod, parsedColumnList);

            return container;
        }

        private static void GetUrlFromString(string formInput, InputContainer container)
        {
            //Parse method name single word 
            var urlString = Regex.Match(formInput, "(?is)from\\s+(http.+?)(\\s+$)?( where.+?$)?$").Groups[1].Value;

            if (string.IsNullOrWhiteSpace(urlString))
            {
                container.Errors.Add("Something goes wrong! Did you forgot to put method name between USING and FROM ??");
            }
            else
            {
                container.Url = urlString;
            }
        }

        private static void GetMethodFromString(string formInput, InputContainer container, ref string parsedMethod)
        {
            //Parse method name single word 
            var methodString = Regex.Match(formInput, "(?is)using\\s+(\\w+)\\s+from").Groups[1].Value;

            if (string.IsNullOrWhiteSpace(methodString))
            {
                container.Errors.Add("Something goes wrong! Did you forgot to put method name between USING and FROM ??");
            }
            else
            {
                parsedMethod = methodString;
            }
        }

        private static void GetColumnsFromString(string formInput, InputContainer container, List<string> parsedColumnList)
        {
            //Parse columns from input string
            var columnsString = Regex.Match(formInput, "(?is)select\\s+(.+?)\\s+using").Groups[1].Value;

            if (string.IsNullOrWhiteSpace(columnsString))
            {
                container.Errors.Add("Something goes wrong! Did you forgot to put column names between SELECT and USING ??");
            }

            //Case of * or [All] 
            if (columnsString.Trim() == "*" || columnsString.Trim().ToLower() == "[all]")
            {
                parsedColumnList.Add(columnsString.Trim().ToLower());
            }
            if (columnsString.Trim().ToLower().Contains(","))
            {
                var input = columnsString.Trim().ToLower();
                //Remove [] and create an array of strings
                var columns = input.Replace("[", "").Replace("]", "").Split(',');

                //Save to parsed column list we will validate columns later
                foreach (var column in columns)
                {
                    parsedColumnList.Add(column.ToLower().Trim());
                }
            }
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
