/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSqlLang.LanguageImplementation;

namespace WebSqlLang
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Add some code here
        }

        private void csvToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        // From https://stackoverflow.com/questions/6239544/populate-treeview-with-file-system-directory-structure

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            return directoryNode;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Main function that will start interpretation of input text and shoving results to a table.
            var programText = textBox1.Text;
            var grid = new DataGridView();

            var container = new InputContainer();
            if (Tokenizer.IsTokenizeble(programText))
            {
                container = Tokenizer.Parse(programText);
                var web = new WebRequest(container);
                web.GetHtml();
                var s = web.Html;
            }

            DataTable table = new DataTable();
            table.Columns.Add("Name");
            table.Rows.Add(programText);

            grid.DataSource = table;
            grid.BackgroundColor = Color.White;
            grid.AutoSize = true;

            tabControl1.TabPages[0].Controls.Add(grid);            

            grid.AutoResizeColumns();

            tabControl1.Refresh();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
