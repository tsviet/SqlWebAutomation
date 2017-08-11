/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */
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
       }
    }
}
