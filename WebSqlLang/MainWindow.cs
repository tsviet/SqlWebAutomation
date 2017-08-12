/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSqlLang.LanguageImplementation;

namespace WebSqlLang
{
    public partial class MainWindow : Form
    {

        public List<IData> DataCollected = null;
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
            DataCollected = new List<IData>();
            var table = new DataTable();
            // Main function that will start interpretation of input text and shoving results to a table.
            var programText = textBox1.Text;

            //Generate output grid
            var grid = new DataGridView
            {
                BackgroundColor = Color.White,
                AutoSize = true,
                AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode) DataGridViewAutoSizeColumnMode.AllCells

            };

            List<IData> finalData = null;
            var container = new InputContainer();
            if (Tokenizer.IsTokenizeble(programText))
            {
                container = Tokenizer.Parse(programText);
                var web = new WebRequest(container);
                web.GetHtml();
                web.PropertyChanged += (sender1, e1) =>
                {
                    //Some code from here https://stackoverflow.com/questions/13294662/propertychangedeventhandler-how-to-get-value
                    switch (e1.PropertyName)
                    {
                        case "Html":
                            var html = (sender1 as WebRequest)?.Html;
                            finalData = HtmlHelper.Parse(container, html);
                            UpdateTableAndGrid(finalData, table, container, grid);
                            break;
                    }
                };
            }
            else
            {
                table.Columns.Add("Error");
                table.Rows.Add("Something goes wrong! Check if your program starts with SELECT!");
                grid.DataSource = table;
                tabControl1.TabPages[0].VerticalScroll.Enabled = true;
                tabControl1.TabPages[0].Controls.Add(grid);
                tabControl1.Refresh();
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table1 = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table1.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (var item in data)
            {
                var row = table1.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table1.Rows.Add(row);
            }
            return table1;

        }

        private void UpdateTableAndGrid(List<IData> finalData, DataTable table, InputContainer container, DataGridView grid)
        {
            try
            {
                var convertedList = finalData.ConvertAll(x => (Links)x);
                var resultTable = ConvertToDataTable(convertedList);

                grid.DataSource = resultTable;
                tabControl1.TabPages[0].VerticalScroll.Enabled = true;
                tabControl1.TabPages[0].Controls.Add(grid);
                tabControl1.Refresh();
            }
            catch (Exception)
            {
                container.Errors.Add("Current method provided doesn't exist!");
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
