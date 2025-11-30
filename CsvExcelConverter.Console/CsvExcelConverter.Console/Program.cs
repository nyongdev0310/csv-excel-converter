using System;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace CsvExcelConverter.ConsoleVersion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== CSV to Excel Converter (Console) ===");

            // Default input/output paths
            string inputFile = "sample/test.csv";
            string outputFile = "sample/result.xlsx";

            // Allow overriding via command-line arguments
            if (args.Length >= 1)
                inputFile = args[0];
            if (args.Length >= 2)
                outputFile = args[1];

            Console.WriteLine($"Input : {inputFile}");
            Console.WriteLine($"Output: {outputFile}");

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"CSV file not found: {inputFile}");
                return;
            }

            try
            {
                DataTable table = LoadCsvToDataTable(inputFile);
                SaveDataTableToExcel(table, outputFile);

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while converting CSV to Excel:");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Reads a CSV file and converts it into a DataTable.
        /// Note: this is a simple parser (no advanced CSV support).
        /// </summary>
        private static DataTable LoadCsvToDataTable(string path)
        {
            var table = new DataTable();

            using (var reader = new StreamReader(path))
            {
                bool headerRead = false;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine() ?? string.Empty;
                    var values = line.Split(',');

                    if (!headerRead)
                    {
                        foreach (var header in values)
                            table.Columns.Add(header.Trim());

                        headerRead = true;
                    }
                    else
                    {
                        var row = table.NewRow();
                        for (int i = 0; i < values.Length && i < table.Columns.Count; i++)
                        {
                            row[i] = values[i].Trim();
                        }
                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Saves a DataTable as an Excel file using ClosedXML.
        /// </summary>
        private static void SaveDataTableToExcel(DataTable table, string outputPath)
        {
            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.Worksheets.Add(table, "Sheet1");
                sheet.Columns().AdjustToContents();
                workbook.SaveAs(outputPath);
            }
        }
    }
}
