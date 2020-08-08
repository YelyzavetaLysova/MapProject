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
            set;
        }

        public List<Region> Regions
        {
            get;
            set;
        }

        public Map(string name, List<Region> regions)
        {
            this.Name = name;
            this.Regions = regions;
        }
    }
}
