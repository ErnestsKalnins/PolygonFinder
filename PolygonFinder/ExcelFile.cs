using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace PolygonFinder
{
    class ExcelFile
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkbook;
        private Excel.Worksheet xlWorksheet;
        private Excel.Range xlRange;

        // Constructor that creates a new excel file
        public ExcelFile()
        {
            this.xlApp = new Excel.Application();
            this.xlApp.Visible = true;
            this.xlWorkbook = this.xlApp.Workbooks.Add();
            this.xlWorksheet = (Excel.Worksheet)this.xlApp.ActiveSheet;
            this.xlRange = this.xlWorksheet.UsedRange;
        }

        // Constructor that opens an excel file in `filePath`
        public ExcelFile(string filePath)
        {
            this.xlApp = new Excel.Application();
            this.xlWorkbook = this.xlApp.Workbooks.Open(filePath);
            this.xlWorksheet = this.xlWorkbook.Sheets[1];
            this.xlRange = this.xlWorksheet.UsedRange;
        }

        ~ExcelFile()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(this.xlRange);
            Marshal.ReleaseComObject(this.xlWorksheet);

            this.xlWorkbook.Close();
            Marshal.ReleaseComObject(this.xlWorkbook);

            this.xlApp.Quit();
            Marshal.ReleaseComObject(this.xlApp);
        }

        public List<dynamic> ReadRow(int row, int offset = 1)
        {
            if (row < 1 || offset < 1)
                throw new ArgumentException();

            var readRow = new List<dynamic>();
            for (int i = offset; i <= this.xlRange.Columns.Count; i++)
            {
                readRow.Add(
                    this.ReadCell(row, i)
                    );
            }
            return readRow;
        }

        public List<dynamic> ReadColumn(int col, int offset = 1)
        {
            if (col < 1 || offset < 1)
                throw new ArgumentException();

            var readCol = new List<dynamic>();
            for (int i = offset; i <= this.xlRange.Rows.Count; i++)
            {
                readCol.Add(
                    this.ReadCell(i, col)
                    );
            }
            return readCol;
        }

        public dynamic ReadCell(int row, int col)
        {
            if (row < 1 || col < 1)
                throw new ArgumentException();

            if (xlRange.Cells[row, col] == null && xlRange.Cells[row, col].Value2 == null)
                throw new Exception("Selected Excel Cell is null");

            return xlRange.Cells[row, col].Value2;
        }

        public void WriteRow(int row, string[] data, int offset = 1)
        {
            if (row < 1 || offset < 1)
                throw new ArgumentException();

            for (int i = offset; i < offset + data.Length; i++)
            {
                this.WriteCell(row, i, data[i - offset]);
            }
        }

        public void WriteColumn(int col, string[] data, int offset = 1)
        {
            if (col < 1 || offset < 1)
                throw new ArgumentException();

            for (int i = offset; i < offset + data.Length; i++)
            {
                this.WriteCell(i, col, data[i - offset]);
            }
        }

        public void WriteCell(int row, int col, string data)
        {
            if (col < 1 || col < 1)
                throw new ArgumentException();

            this.xlWorksheet.Cells[row, col] = data;
        }

        public void MergeCells(int xFrom, int yFrom, int xTo, int yTo)
        {
            var range = this.xlWorksheet.Range[
                    xlWorksheet.Cells[xFrom, yFrom],
                    xlWorksheet.Cells[xTo, yTo]
                ];

            range.Merge();
            range.Cells.WrapText = true;
        }
    }
}
