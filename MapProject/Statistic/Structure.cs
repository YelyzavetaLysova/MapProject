using MapProject.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class Structure
    {
        public string Id
        {
            get;
            private set;
        }

        public List<DataItem> Data
        {
            get;
            private set;
        }

        private Structure()
        {
            this.Data = new List<DataItem>();
        }

        public Structure(Map map) : this()
        {
            this.Id = map.Name;
        }

        public Structure(Region region) : this()
        {
            this.Id = region.Id;
        }
    }
}
