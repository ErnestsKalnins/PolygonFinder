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
            this.WriteHeader();
            int verticalOffset = 3;
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

                this.Excel.WriteColumn(
                    1, 
                    lineIDs.Select((n) => n.ToString()).ToArray(), 
                    verticalOffset
                    );
                this.Excel.WriteColumn(
                    2, 
                    vertexX.Select((n) => Math.Round(n).ToString()).ToArray(), 
                    verticalOffset
                    );
                this.Excel.WriteColumn(
                    3, 
                    vertexY.Select((n) => Math.Round(n).ToString()).ToArray(), 
                    verticalOffset
                    );

                verticalOffset += polygon.Lines.Count + 1;
            }
        }

        private void WriteHeader()
        {
            var headerLine1 = new string[] { "Line numbers", "Vertices" };
            var headerLine2 = new string[] { "X", "Y" };

            this.Excel.MergeCells(1, 1, 2, 1);
            this.Excel.MergeCells(1, 2, 1, 3);

            this.Excel.WriteRow(1, headerLine1); 
            this.Excel.WriteRow(2, headerLine2, 2); 
        }

    }
}
