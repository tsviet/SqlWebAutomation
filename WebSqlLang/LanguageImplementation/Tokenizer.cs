/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

            if (container.Errors.Count > 0) return container;
            GetWhereFromString(formInput, container);

            //Fill Column map with method and columns
            container.ColumnsMap.Add(parsedMethod, parsedColumnList);

            return container;
        }

        private static void GetUrlFromString(string formInput, InputContainer container)
        {
            //Parse method name single word 
            var urlString = Regex.Match(formInput, "(?is)from\\s+(http.+?)(\\s+$)?(\\s+where.+?$)?$").Groups[1].Value;

            if (string.IsNullOrWhiteSpace(urlString))
            {
                container.Errors.Add("Something goes wrong! Did you forgot to put method name between USING and FROM ??");
            }
            else
            {
                container.Url = urlString;
            }
        }

        private static void GetWhereFromString(string formInput, InputContainer container)
        {
            //Parse where
            var where = Regex.Match(formInput, "(?is)where\\s+(.+?)(\\s+$)?$").Groups[1].Value;
            container.Where = where;
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

        public static List<Where> ParseWhere(string where)
        {
            var res = new List<Where>();
            var and = new string[1] {" and "};
            var or = new string[1] { " or " };
            var none = new string[1] { " none " };
            //Remove redundant spaces or new lines
            where = Regex.Replace(where, "\\s\\s+", "");
            
            if (where.ToLower().Contains(" and "))
            {
                res.Add(GetWhereObject(where, and));
            }
            if (where.ToLower().Contains(" or "))
            {
                res.Add(GetWhereObject(where, or));
            }

            res.Add(GetWhereObject(where, none));

            return res;
        }

        private static Where GetWhereObject(string where, string[] and)
        {
            var w1 = new Where
            {
                Seperator = and[0],
                Data = new List<Limits>()
            };
            var whereArray = where.Split(and, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in whereArray)
            {
                var has = new string[1] {" contains "};
                var greater = new string[1] {" > "};
                var greaterEqual = new string[1] {" >= "};
                var less = new string[1] {" < "};
                var lessEqual = new string[1] {" =< "};
                var equal = new string[1] {" == "};
                var regMatch = new string[1] {" regex "};

                var lim = new Limits();
                if (str.Contains(has[0]))
                {
                    lim = SetLimits(str, has[0]); 
                }
                else if (str.Contains(greater[0]))
                {
                    lim = SetLimits(str, greater[0]);
                }
                else if (str.Contains(greaterEqual[0]))
                {
                    lim = SetLimits(str, greaterEqual[0]);
                }
                else if (str.Contains(less[0]))
                {
                    lim = SetLimits(str, less[0]);
                }
                else if (str.Contains(lessEqual[0]))
                {
                    lim = SetLimits(str, lessEqual[0]);
                }
                else if (str.Contains(equal[0]))
                {
                    lim = SetLimits(str, equal[0]);
                }
                else if (str.Contains(regMatch[0]))
                {
                    lim = SetLimits(str, regMatch[0]);
                }

                if (!string.IsNullOrEmpty(lim?.Name))
                {
                    w1.Data.Add(lim);
                }
            }
            return w1;
        }

        private static Limits SetLimits(string str, string s)
        {
            var limits = new Limits();
            var res = Regex.Matches(str, $@"(?is)^\s*(\w+){s}""(.+?)""");
            if (res.Count <= 0) return limits;
            limits.Name = res[0].Groups[1].Value;
            limits.Operator = s.Trim();
            limits.Value = res[0].Groups[2].Value;
            return limits;
        }

        public static IList<T> FilterDataArray<T>(IList<T> data, List<Where> where)
        {
            //var finalList = new List<IData>();
            var properties = TypeDescriptor.GetProperties(typeof(T));

            if (where != null && where.Count > 0)
            {
                foreach (var w in where)
                {
                    if (w.Seperator.Trim() == "and")
                    {
                        foreach (var d in w.Data)
                        {
                            data = ApplyFilter(data, properties, d);
                        }
                    }
                    //Add logic when "or" provided
                    else if (w.Seperator.Trim() == "or")
                    {
                        foreach (var d in w.Data)
                        {
                            data = ApplyFilter(data, properties, d);
                        }
                    }
                    else
                    {
                        data = ApplyFilter(data, properties, w.Data.FirstOrDefault());
                    }
                }

            }

            return data;
        }

        private static IList<T> ApplyFilter<T>(IList<T> data, PropertyDescriptorCollection properties, Limits d)
        {
            foreach (PropertyDescriptor prop in properties)
            {
                //Skip not matching cases
                if (d.Name.ToLower() != prop.Name.ToLower()) continue;

                if (d.Operator == "contains")
                {
                    data = data?.Where(x =>
                    {
                        var value = prop.GetValue(x);
                        return value != null && value.ToString().ToLower().Contains(d.Value.ToLower());
                    }).ToList();
                }

                if (d.Operator == "==")
                {
                    data = data?.Where(x =>
                    {
                        var value = prop.GetValue(x);
                        return value != null && value.ToString().ToLower() == d.Value.ToLower();
                    }).ToList();
                }
            }
            return data;
        }
    }
}
