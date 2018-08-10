﻿using System;
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

        // Sub-graphs containing cycles
        private List<List<Line>> FoundLoops;

        // The public interface method that returns all loops in a list of lines
        public List<List<Line>> FindLoops(List<Line> lines)
        {
            this.Open = new List<Line>(lines);
            this.Selected = new List<Line>();
            this.Closed = new List<Line>();
            this.FoundLoops = new List<List<Line>>();

            foreach (var line in lines)
            {
                if (this.Open.Contains(line))
                    this.Visit(line);
            }

            return this.FoundLoops;
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
                // Avoid run-time reference bugs by initializing a new list
                this.FoundLoops.Add(new List<Line>(this.Selected));

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

            return selectedAdjacentLines > 1;
        }

    }
}