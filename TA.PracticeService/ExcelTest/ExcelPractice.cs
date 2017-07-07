using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace TA.PracticeService.ExcelTest
{
    public class ExcelPractice : IDisposable
    {
        #region Initialization
        private static ExcelPractice _instance;
        private static object _lockObject = new object();
        private Excel.Application xlApp;
        private Excel.Range xlRange;
        private Excel.Workbook xlWorkbook;
        private Excel._Worksheet xlWorksheet;

        private ExcelPractice()
        {

        }
        public static ExcelPractice Instance
        {
            get
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelPractice();
                    }
                    return _instance;
                }
            }
        }
        private void InitializeComObjects(string filePath, int sheetnumber)
        {
            xlApp = new Excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(filePath);
            xlWorksheet = xlWorkbook.Sheets[sheetnumber];
            xlRange = xlWorksheet.UsedRange;
        }

        public void Dispose()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
        #endregion

        public DataTable ReadExcelFile(string filePath)
        {
            return ReadExcelFile(filePath, 1);
        }

        public DataTable ReadExcelFile(string filePath, int sheetNumber)
        {
            return ReadExcelFile(filePath, sheetNumber, true);
        }

        public DataTable ReadExcelFile(string filePath, bool isFirstRowHeader)
        {
            return ReadExcelFile(filePath, 1, isFirstRowHeader);
        }

        public DataTable ReadExcelFile(string filePath, int sheetNumber, bool isFirstRowHeader)
        {
            InitializeComObjects(filePath, sheetNumber);
            var rowCount = xlRange.Rows.Count;
            var colCount = xlRange.Columns.Count;

            var excelTable = CreateDataTable(isFirstRowHeader, out string[] dataTableColumns);
            var startRowNumber = isFirstRowHeader ? 2 : 1;
            for (int i = startRowNumber; i <= rowCount; i++)
            {
                var newRow = excelTable.NewRow();
                for (int j = 1; j <= colCount; j++)
                {
                    newRow[dataTableColumns[j-1]] = xlRange.Cells[i, j].Value2.ToString();
                }
                excelTable.Rows.Add(newRow);
            }
            return excelTable;
        }

        private DataTable CreateDataTable(bool isFirstRowHeader, out string[] columnNames)
        {
            var excelTable = new DataTable("ExcelData");
            var colCount = xlRange.Columns.Count;
            var columnNameList = new List<string>();
            if (isFirstRowHeader)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    columnNameList.Add(xlRange.Cells[1, j].Value2.ToString());
                    excelTable.Columns.Add(xlRange.Cells[1, j].Value2.ToString());
                }
            }
            else
            {
                for (int j = 1; j <= colCount; j++)
                {
                    columnNameList.Add($"Column{j}");
                    excelTable.Columns.Add($"Column{j}");
                }
            }
            columnNames = columnNameList.ToArray();
            return excelTable;
        }
    }
}
