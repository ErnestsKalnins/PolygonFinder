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
                if (i == this.Lines.Count - 1)
                {
                    foreach (var line in this.Lines[i].IntersectsWith)
                    {
                        
                        if (line != this.Lines[i-1] && this.Lines.Contains(line))
                        {
                            this.Vertices.Add(
                                this.Lines[i].GetIntersectionPoint(line)
                                );
                        }
                    }
                }
                else
                {
                    this.Vertices.Add(
                        this.Lines[i].GetIntersectionPoint(this.Lines[i + 1])
                        );
                }
            }
        }
    }
}
