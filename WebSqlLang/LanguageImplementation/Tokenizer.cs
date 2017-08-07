using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSqlLang.LanguageImplementation
{
    public static class Tokenizer
    {
        public static void Parse(string formInput)
        {
            //throw new NotImplementedException();
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
