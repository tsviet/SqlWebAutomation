using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSqlLang.LanguageImplementation;

namespace WebSqlLang
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());

            //TODO: MOVE THIS TO ONCLICK METHOD WHEN UI WILL BE READY

            OnButtonClick();

        }

        private static void OnButtonClick()
        {
            var formInput = "SELECT * USING HEADERS FROM https://wwww.google.com";
            //Check if string has required minimum text to be albe to do somthing
            if (Tokenizer.isTokenizeble(formInput))
            {
                Tokenizer.Parse(formInput);
            }

            //TODO: FILL in this oject dynamically
            InputContainer.Url = "https://wwww.google.com";
            InputContainer.Fields.Add("*");
            InputContainer.Using = "HEADERS";



        }
    }
}
