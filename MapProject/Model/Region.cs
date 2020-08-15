using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Region
    {
        public string Id
        {
            get;
            private set;
        }

        public List<Point> Points
        {
            get;
            private set;
        } 

        public Map ReferencedMap
        {
            get;
            private set;
        }

        public Region()
        {
            
            this.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);

            this.Points = new List<Point>();
        }

        public Region(IEnumerable<Point> points) : this()
        {
            this.Points.AddRange(points);
        }

        public Region(string id, IEnumerable<Point> points, Map referencedMap) : this(points)
        {
            this.Id = id;
            this.ReferencedMap = referencedMap;
        }
    }
}
