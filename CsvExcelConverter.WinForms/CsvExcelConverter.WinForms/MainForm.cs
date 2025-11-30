using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace CsvExcelConverter.WinForms
{
    public partial class MainForm : Form
    {
        private TextBox txtCsvPath;
        private TextBox txtExcelPath;
        private Button btnBrowseCsv;
        private Button btnBrowseExcel;
        private Button btnConvert;
        private TextBox txtLog;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "CSV → Excel Converter";
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblCsv = new Label
            {
                Text = "CSV File:",
                Left = 20,
                Top = 20,
                Width = 80
            };

            txtCsvPath = new TextBox
            {
                Left = 100,
                Top = 18,
                Width = 360
            };

            btnBrowseCsv = new Button
            {
                Text = "Browse...",
                Left = 470,
                Top = 16,
                Width = 90
            };
            btnBrowseCsv.Click += BtnBrowseCsv_Click;

            var lblExcel = new Label
            {
                Text = "Excel File:",
                Left = 20,
                Top = 60,
                Width = 80
            };

            txtExcelPath = new TextBox
            {
                Left = 100,
                Top = 58,
                Width = 360
            };

            btnBrowseExcel = new Button
            {
                Text = "Browse...",
                Left = 470,
                Top = 56,
                Width = 90
            };
            btnBrowseExcel.Click += BtnBrowseExcel_Click;

            btnConvert = new Button
            {
                Text = "Convert",
                Left = 20,
                Top = 100,
                Width = 540,
                Height = 30
            };
            btnConvert.Click += BtnConvert_Click;

            txtLog = new TextBox
            {
                Left = 20,
                Top = 150,
                Width = 540,
                Height = 180,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            this.Controls.Add(lblCsv);
            this.Controls.Add(txtCsvPath);
            this.Controls.Add(btnBrowseCsv);
            this.Controls.Add(lblExcel);
            this.Controls.Add(txtExcelPath);
            this.Controls.Add(btnBrowseExcel);
            this.Controls.Add(btnConvert);
            this.Controls.Add(txtLog);
        }

        private void BtnBrowseCsv_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCsvPath.Text = ofd.FileName;

                    // 기본 Excel 경로 자동 세팅
                    string excelPath = Path.ChangeExtension(ofd.FileName, ".xlsx");
                    txtExcelPath.Text = excelPath;
                }
            }
        }

        private void BtnBrowseExcel_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                if (!string.IsNullOrWhiteSpace(txtExcelPath.Text))
                {
                    sfd.FileName = Path.GetFileName(txtExcelPath.Text);
                    sfd.InitialDirectory = Path.GetDirectoryName(txtExcelPath.Text);
                }

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtExcelPath.Text = sfd.FileName;
                }
            }
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            txtLog.Clear();

            string csvPath = txtCsvPath.Text;
            string excelPath = txtExcelPath.Text;

            if (!File.Exists(csvPath))
            {
                Log("CSV file not found.");
                MessageBox.Show("CSV file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(excelPath))
            {
                Log("Excel output path is empty.");
                MessageBox.Show("Please select an output Excel file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Log("Reading CSV...");
                DataTable table = LoadCsv(csvPath);

                Log("Creating Excel workbook...");
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(table, "Sheet1");
                    sheet.Columns().AdjustToContents();
                    workbook.SaveAs(excelPath);
                }

                Log("Conversion completed.");
                Log("Output: " + excelPath);

                MessageBox.Show("Conversion completed successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoadCsv(string path)
        {
            var table = new DataTable();
            using (var reader = new StreamReader(path))
            {
                bool headerRead = false;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (!headerRead)
                    {
                        foreach (var header in values)
                            table.Columns.Add(header.Trim());
                        headerRead = true;
                    }
                    else
                    {
                        table.Rows.Add(values);
                    }
                }
            }
            return table;
        }

        private void Log(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }
}
