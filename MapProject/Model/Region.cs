using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Region
    {
        public Dictionary<string, string> Info
        {
            get;
            set;
        }

        public List<Point> Points
        {
            get;
            private set;
        } 

        public Map ReferencedMap
        {
            get;
            set;
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
