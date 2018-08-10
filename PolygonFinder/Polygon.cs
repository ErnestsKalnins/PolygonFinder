using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    class Polygon
    {
        public List<Line> Lines { get; }
        public List<Point> Vertices { get; }

        public Polygon(List<Line> lines)
        {
            this.Lines = lines;
            this.Vertices = new List<Point>();

            this.CalculateVertices();
        }

        private void CalculateVertices()
        {
            for (int i = 0; i < this.Lines.Count; i++)
            {
                int j = (i == this.Lines.Count - 1) ? 0 : i + 1;

                this.Vertices.Add(
                    this.Lines[i].GetIntersectionPoint(this.Lines[j])
                    );
            }
        }
    }
}
