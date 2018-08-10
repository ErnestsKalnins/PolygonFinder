using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    class ExcelFormatter
    {
        private ExcelFile Excel;

        public ExcelFormatter(ExcelFile excel)
        {
            this.Excel = excel;
        }

        public void WritePolygons(List<Polygon> polygons)
        {
            int verticalOffset = 1;
            int polygonNr = 1;
            foreach (var polygon in polygons)
            {
                var lineIDs = new List<int>();
                var vertexX = new List<double>();
                var vertexY = new List<double>();

                // polygons always have the same number of vertices and lines
                for (int i = 0; i < polygon.Lines.Count; i++)
                {
                    lineIDs.Add(polygon.Lines[i].ID);
                    vertexX.Add(polygon.Vertices[i].X);
                    vertexY.Add(polygon.Vertices[i].Y);
                }

                this.WriteHeader(verticalOffset, polygonNr++);

                this.Excel.WriteColumn(
                    1, 
                    lineIDs.Select((n) => n.ToString()).ToArray(), 
                    verticalOffset + 3
                    );
                this.Excel.WriteColumn(
                    2, 
                    vertexX.Select((n) => n.ToString()).ToArray(), 
                    verticalOffset + 3
                    );
                this.Excel.WriteColumn(
                    3, 
                    vertexY.Select((n) => n.ToString()).ToArray(), 
                    verticalOffset + 3
                    );

                verticalOffset += polygon.Lines.Count + 4;
            }
        }

        private void WriteHeader(int row, int number)
        {
            var headerLine1 = new string[] { "Polygon " + number };
            var headerLine2 = new string[] { "Line numbers", "Vertices" };
            var headerLine3 = new string[] { "X", "Y" };

            this.Excel.MergeCells(row, 1, row, 3);
            this.Excel.MergeCells(row + 1, 1, row + 2, 1);
            this.Excel.MergeCells(row + 1, 2, row + 1, 3);

            this.Excel.WriteRow(row, headerLine1);
            this.Excel.WriteRow(row + 1, headerLine2); 
            this.Excel.WriteRow(row + 2, headerLine3, 2); 
        }

    }
}
