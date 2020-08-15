using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Region
    {
        private string _id;
        
        public string Id
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._id))
                {
                    this._id = Guid.NewGuid().ToString().Replace("-", String.Empty);
                }

                return this._id;
            }
            private set
            {
                this._id = value;
            }
            
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
