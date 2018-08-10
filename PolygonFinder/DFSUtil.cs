using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    // DFSUtil provides a utility function to find cycles in a graph.
    // The algorithm implemented is a depth-first search as described in
    // CLRS page 604 with modifications for cycle detection.
    class DFSUtil
    {
        // Unvisited Lines
        private List<Line> Open;

        // Lines forming the current traversal path
        private List<Line> Selected;

        // Visited lines
        private List<Line> Closed;

        // Helper variable that tracks trimmed lines in a cycle containing sub-graph
        private List<Line> Trimmed;

        // Sub-graphs containing cycles
        private List<List<Line>> Cycles;

        // The public interface method that returns all loops in a list of lines
        public List<List<Line>> FindLoops(List<Line> lines)
        {
            this.Open = new List<Line>(lines);
            this.Selected = new List<Line>();
            this.Closed = new List<Line>();
            this.Cycles = new List<List<Line>>();

            foreach (var line in lines)
            {
                if (this.Open.Contains(line))
                    this.Visit(line);
            }

            return this.Cycles;
        }

        // Visit selects the current line and visits all its adjacent lines recursively.
        private void Visit(Line line)
        {
            this.Open.Remove(line);
            this.Selected.Add(line);

            foreach (var adjacentLine in line.IntersectsWith)
            {
                if (this.Open.Contains(adjacentLine))
                    this.Visit(adjacentLine);
            }

            // When a line has no Open adjacent lines, it is checked if the line has at least
            // two adjacent Selected lines. If this is the case, then the Selected Line List
            // contains a sub-graph with a single cycle.
            if (this.FormsCycle(line))
            {
                // Avoid run-time reference bugs by initializing a new list
                var cycle = this.TrimCycle(new List<Line>(this.Selected));
                this.Cycles.Add(cycle);
            }

            this.Selected.Remove(line);
            this.Closed.Add(line);
        }

        private bool FormsCycle(Line line)
        {
            var selectedAdjacentLines = 0;
            foreach (var adjacentLine in line.IntersectsWith)
            {
                if (this.Selected.Contains(adjacentLine))
                    ++selectedAdjacentLines;
            }

            return selectedAdjacentLines == 2;
        }

        // TrimCycle trims any excess lines that don't form a cycle. 
        private List<Line> TrimCycle(List<Line> lines)
        {
            this.Trimmed = new List<Line>();

            for (int i = 0; i < lines.Count; i++)
            {
                int counter = 0;
                for (int j = 0; j < lines[i].IntersectsWith.Count; j++)
                {
                    if (lines.Contains(lines[i].IntersectsWith[j]))
                    {
                        ++counter;
                    }
                }
                if (counter < 2)
                {
                    this.TrimTrailingLine(lines, lines[i]);
                }
            }

            /*
            foreach (var line in lines)
            {
                if (line.IntersectsWith.Count < 2)
                {
                    this.TrimTrailingLine(lines, line);
                }
            }
            */
            foreach (var trimmed in this.Trimmed)
            {
                lines.Remove(trimmed);
            }

            return lines;
        }

        // TrimTrailingLine trims the line if it has less than two 
        private void TrimTrailingLine(List<Line> lines, Line line)
        {
            int validNeighbors = 0;
            int validNeighborIndex = 0;

            for (int i = 0; i < line.IntersectsWith.Count; i++)
            {
                if (lines.Contains(line.IntersectsWith[i]) && !this.Trimmed.Contains(line.IntersectsWith[i]))
                {
                    ++validNeighbors;
                    // In case if this is overwritten twice, it doesn't matter, since the line forms a cycle
                    validNeighborIndex = i;
                }
            }

            if (validNeighbors < 2)
            {
                this.Trimmed.Add(line);
                this.TrimTrailingLine(lines, line.IntersectsWith[validNeighborIndex]);
            }
        }
    }
}
