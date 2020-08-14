using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Map
    {
        public string Name
        {
            get;
            private set;
        }

        public List<Region> Regions
        {
            get;
            private set;
        }

        public Map()
        {
            this.Regions = new List<Region>();
        }

        public Map(string name, IEnumerable<Region> regions) : this()
        {
            this.Name = name;
            this.Regions.AddRange(regions);
        }
    }
}
