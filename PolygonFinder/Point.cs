using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
