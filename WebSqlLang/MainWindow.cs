/* Copyright © 2017 Mykhailo Tsvietukhin. This program is released under the "GPL-3.0" lisense. Please see LICENSE for license terms. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WebSqlLang.LanguageImplementation;

namespace WebSqlLang
{
    public partial class MainWindow : Form
    {
        private string currentDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\WebSqlLang";
        public List<IData> DataCollected = null;

        public MainWindow()
        {
            InitializeComponent();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

            var box = new TextBox
            {
                Multiline = true,
                Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte) (0))),
                Text = @"SELECT [NAME, VALUE] using HEADERS FROM https://stackoverflow.com/questions/25688847/html-agility-pack-get-all-urls-on-page WHERE NAME contains ""Set-Cookie"" into csv ""test.csv""" 
            };

            mainInputTabControl.TabPages[0].Controls.Add(box);
            mainInputTabControl.Width = mainInputTabControl.Parent.Width - 20;
            mainInputTabControl.Height = mainInputTabControl.Parent.Bottom / 2 - 70;
            tabControl1.Width = tabControl1.Parent.Width - 20;
            tabControl1.Height = tabControl1.Parent.Bottom / 2 - 30;
            box.Height = box.Parent.Bottom;
            box.Width = box.Parent.Width;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            tabControl1.TabPages[0].Controls.Clear();

            DataCollected = new List<IData>();
            var table = new DataTable();
            // Main function that will start interpretation of input text and shoving results to a table.
            var programText = mainInputTabControl.SelectedTab.Controls[0] as TextBox;

            //Generate output grid
            var grid = new DataGridView
            {
                BackgroundColor = Color.White,
                AutoSize = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode) DataGridViewAutoSizeColumnMode.AllCells
            };

            List<IData> finalData = null;
            InputContainer container;
            if (Tokenizer.IsTokenizeble(programText?.Text))
            {
                container = Tokenizer.Parse(programText?.Text);
                var web = new WebRequest(container);
                if (container.ColumnsMap.FirstOrDefault().Key.ToLower() == "headers")
                {
                    var dic = web.GetHeaders();
                    finalData = HtmlHelper.ConvertToHeaders(container, dic);
                    UpdateTableAndGrid(finalData, container, grid);
                }
                else
                {
                    web.GetHtml();
                }
                web.PropertyChanged += (sender1, e1) =>
                {
                    //Some code from here https://stackoverflow.com/questions/13294662/propertychangedeventhandler-how-to-get-value
                    switch (e1.PropertyName)
                    {
                        case "Html":
                            var html = (sender1 as WebRequest)?.Html;
                            finalData = HtmlHelper.Parse(container, html);
                            UpdateTableAndGrid(finalData, container, grid);
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

        
        private void UpdateTableAndGrid(List<IData> finalData, InputContainer container, DataGridView grid)
        {

            var where = !string.IsNullOrWhiteSpace(container.Where) ? Tokenizer.ParseWhere(container.Where) : new List<Where>();

            try
            {
                var resultTable = new DataTable();
                if (container.ColumnsMap.FirstOrDefault().Key.ToLower() == "links")
                {
                    var res = where.Count > 0 ? Tokenizer.FilterDataArray(finalData.ConvertAll(x => (Links)x), where) : finalData.ConvertAll(x => (Links)x);
                    resultTable = HtmlHelper.ConvertToDataTable(res, container);
                }
                if (container.ColumnsMap.FirstOrDefault().Key.ToLower() == "headers")
                {
                    var res = where.Count > 0 ? Tokenizer.FilterDataArray(finalData.ConvertAll(x => (Headers)x), where) : finalData.ConvertAll(x => (Headers)x);
                    resultTable = HtmlHelper.ConvertToDataTable(res, container);
                }

                grid.DataSource = resultTable;
                if (container.SaveFormat == "csv")
                {
                    //Thanks to vc 74 https://stackoverflow.com/questions/4959722/c-sharp-datatable-to-csv
                    var sb = new StringBuilder();

                    var columnNames = resultTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in resultTable.Rows)
                    {
                        var fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }

                    File.WriteAllText(container.SavePath, sb.ToString());
                }
                tabControl1.TabPages[0].VerticalScroll.Enabled = true;
                tabControl1.TabPages[0].Controls.Add(grid);
                grid.Width = grid.Parent.Width;
                grid.Height = grid.Parent.Bottom;
                tabControl1.Refresh();
            }
            catch (Exception)
            {
                container.Errors.Add("Current method provided doesn't exist!");
            }
        }

        private void SaveCurrentWindow()
        {
            var fileName = currentDir + "\\" + mainInputTabControl.SelectedTab.Text;
            if (File.Exists(fileName))
            {
                File.WriteAllText(fileName, mainInputTabControl.SelectedTab.Controls[0].Text);
            }
            else
            {
                saveFileDialog1.ShowDialog();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var name = openFileDialog1.FileName;
            var content = File.ReadAllText(name);

            //Create new tab on file open
            var newTabPage = new TabPage {Text = openFileDialog1.SafeFileName, Name = openFileDialog1.SafeFileName};
            var box = new TextBox
            {
                Multiline = true,
                Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                Text = content
            };
            newTabPage.Controls.Add(box);
            mainInputTabControl.Controls.Add(newTabPage);
            mainInputTabControl.SelectedTab = newTabPage;
            box.Height = box.Parent.Bottom;
            box.Width = box.Parent.Width;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //https://www.dotnetperls.com/savefiledialog

            var name = saveFileDialog1.FileName;
            var box = mainInputTabControl.SelectedTab.Controls[0] as TextBox;
            File.WriteAllText(name, box?.Text);
        }

        private void mainInputTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            //https://stackoverflow.com/questions/3183352/close-button-in-tabcontrol
            Rectangle r = mainInputTabControl.GetTabRect(this.mainInputTabControl.SelectedIndex);
            Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 2, 15, 15);
            if (closeButton.Contains(e.Location))
            {
                this.mainInputTabControl.TabPages.Remove(this.mainInputTabControl.SelectedTab);
            }
        }

        private void mainInputTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            //This code will render a "x" mark at the end of the Tab caption. 
            e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 2);
            e.Graphics.DrawString(this.mainInputTabControl.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 2);
            e.DrawFocusRectangle();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void mainInputTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveCurrentWindow();
            }
            if (e.KeyCode == Keys.F5)
            {
                Run();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newTabPage = new TabPage();
            var box = new TextBox
            {
                Multiline = true,
                Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)))
            };
            newTabPage.Controls.Add(box);
            mainInputTabControl.Controls.Add(newTabPage);
            mainInputTabControl.SelectedTab = newTabPage;
            newTabPage.Text = $@"Program_{mainInputTabControl.Controls.Count}";
            
            box.Height = box.Parent.Bottom;
            box.Width = box.Parent.Width;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveCurrentWindow();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/tsviet/WebSqlLang/blob/master/LICENSE");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.DefaultExt = "wslang";
            openFileDialog1.Filter = @"All|*|Web Sql Language|*.wslang";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "*";
            openFileDialog1.ShowDialog();
        }
    }
}
