using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonFinder
{
    class Line
    {
        public int ID { get; }
        public Point Start { get; }
        public Point End { get; }

        public List<Line> IntersectsWith;

        // generalization of a linear function `y = a*x + b`, where `a` is the slope
        // and `b` is the constant term
        private double Slope { get; }
        private double Constant { get; }

        // In case if the line is expressed in terms of `x = 1` or `y = 1`,
        // then we assign the constant value to these variables
        private bool IsConstantX { get; }
        private double ConstantX { get; }
        private bool IsConstantY { get; }
        private double ConstantY { get; }

        public Line(int id, Point start, Point end)
        {
            this.ID = id;
            this.Start = start;
            this.End = end;

            this.IntersectsWith = new List<Line>();

            var dx = (end.X - start.X);
            var dy = (end.Y - start.Y);

            if (dx == 0) {
                this.ConstantX = start.X;
                this.IsConstantX = true;
            } else if (dy == 0) {
                this.ConstantY = start.Y;
                this.IsConstantY = true;
            } else {
                this.Slope = dx == 0 ? 0 : dy / dx;
                this.Constant = start.Y - start.X * this.Slope;
            }
        }

        // Get the coordinates of two line intersection
        public Point GetIntersectionPoint(Line line)
        {
            // Make sure the `line` is an intersecting one
            if (!this.IntersectsWith.Contains(line))
                throw new ArgumentException();

            double x, y;

            if (this.IsConstantX && line.IsConstantY)
            {
                x = this.ConstantX;
                y = line.ConstantY;
            }
            else if (line.IsConstantX && this.IsConstantY) {
                x = line.ConstantX;
                y = this.ConstantY;
            }
            else if (this.IsConstantX)
            {
                x = this.ConstantX;
                y = line.Slope * x + line.Constant;
            }
            else if (this.IsConstantY)
            {
                y = this.ConstantY;
                x = (line.Constant - y) / line.Slope;
            }
            else if (line.IsConstantX)
            {
                x = line.ConstantX;
                y = this.Slope * x + this.Constant;
            }
            else if (line.IsConstantY)
            {
                y = line.ConstantY;
                x = (this.Constant - y) / this.Slope;
            }
            else
            {
                x = (this.Constant - line.Constant) / (line.Slope - this.Slope);
                y = this.Slope * x + this.Constant;
            }

            return new Point(x, y);
        }

        // This method checks if two lines intersect by checking how 
        // each of the lines start and end points are aligned.
        public bool Intersects(Line line)
        {
            // Determine the four orientations necessary for general 
            // and special cases
            Orientation o1 = this.GetOrientation(line.Start);
            Orientation o2 = this.GetOrientation(line.End);
            Orientation o3 = line.GetOrientation(this.Start);
            Orientation o4 = line.GetOrientation(this.End);

            // General case check
            if (o1 != o2 && o3 != o4)
                return true;

            // Special case check
            if (o1 == Orientation.Colinear && this.IsOnSegment(line.Start)) return true;
            if (o2 == Orientation.Colinear && this.IsOnSegment(line.End)) return true;
            if (o3 == Orientation.Colinear && line.IsOnSegment(this.Start)) return true;
            if (o4 == Orientation.Colinear && line.IsOnSegment(this.End)) return true;

            return false;
        }

        // Orientation detects whether this lines start point, end point and 
        // outside point `p` are oriented colinearly, clockwise or counterclockwise.
        private Orientation GetOrientation(Point p)
        {
            double deltaSlope = (this.End.Y - this.Start.Y) * (p.X - this.End.X) -
                                (p.Y - this.End.Y) * (this.End.X - this.Start.X);

            if (deltaSlope == 0) return Orientation.Colinear;

            return (deltaSlope > 0) ? Orientation.Clockwise : Orientation.CounterClockwise;
        }

        // IsOnSegment checks if the point `p` X and Y coordinates lie
        // within this Lines X and Y projection range
        private bool IsOnSegment(Point p)
        {
            if (p.X <= Math.Max(this.Start.X, this.End.X) &&
                p.X >= Math.Min(this.Start.X, this.End.X) &&
                p.Y <= Math.Max(this.Start.Y, this.End.Y) &&
                p.Y >= Math.Min(this.Start.Y, this.End.Y))
                return true;

            return false;
        }
    }
}
