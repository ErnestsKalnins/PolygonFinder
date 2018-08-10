using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    class Canvas
    {
        public List<Polygon> Polygons { get; }
        private List<Line> Lines;

        public Canvas(List<Line> lines)
        {
            this.Lines = lines;
            this.Polygons = new List<Polygon>();

            // Once the lines are drawn, intersections become apparent
            this.FindLineIntersections();
        }

        public void FindPolygons()
        {
            var dfs = new DFSUtil();
            var polygonLines = dfs.FindLoops(this.Lines);
            foreach (var lines in polygonLines)
            {
                this.Polygons.Add(
                    new Polygon(lines)
                    );
            }
        }

        private void FindLineIntersections()
        {
            for (int i = 0; i < this.Lines.Count; i++)
            {
                for (int j = 0; j < this.Lines.Count; j++)
                {
                    if ( i != j && this.Lines[i].Intersects(this.Lines[j]))
                    {
                        this.Lines[i].IntersectsWith.Add(this.Lines[j]);
                    }
                }
            }
        }
    }
}
