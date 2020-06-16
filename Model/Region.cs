using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Region
    {
        public List<Region> InnerRegions
        {
            get;
            private set;
        }

        public Region OuterRegion
        {
            get;
            set;
        }

        public List<Point> Points
        {
            get;
            private set;
        } 

        public Region()
        {
            this.Points = new List<Point>();
        }

        public Region(IEnumerable<Point> points) : this()
        {
            this.AddPoint(points);
        }

        public void AddPoint(Point p) 
        {
            this.Points.Add(p);
        }

        public void AddPoint(IEnumerable<Point> points)
        {
            this.Points.AddRange(points);
        }

    }
}
