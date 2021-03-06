﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input excel file path (c:/.../file.xlsx): ");
            string excelPath = Console.ReadLine();
            ExcelFile readExcel;
            try
            {
                readExcel = new ExcelFile(excelPath);
            }
            catch
            {
                Console.WriteLine("Invalid file path");
                return;
            }

            var lines = ReadLines(readExcel);

            var canvas = new Canvas(lines);

            canvas.FindPolygons();

            var writeExcel = new ExcelFile();
            var excelFormatter = new ExcelFormatter(writeExcel);

            excelFormatter.WritePolygons(canvas.Polygons);
        }

        // I could not find any other suitable class to put this in.
        public static List<Line> ReadLines(ExcelFile xl)
        {
            var ids = xl.ReadColumn(1, 3);
            var startX = xl.ReadColumn(2, 3);
            var startY = xl.ReadColumn(3, 3);
            var endX = xl.ReadColumn(4, 3);
            var endY = xl.ReadColumn(5, 3);

            var lines = new List<Line>();

            for (int i = 0; i < ids.Count; i++)
            {
                lines.Add(
                    new Line(
                        (int)ids[i],
                        new Point(startX[i], startY[i]),
                        new Point(endX[i], endY[i])
                        )
                    );
            }

            return lines;
        }
    }
}
