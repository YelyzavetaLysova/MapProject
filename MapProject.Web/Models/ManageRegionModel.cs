using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapProject.Statistic;

namespace MapProject.Web.Models
{
    public class ManageRegionModel
    {
        public string RegionId
        {
            get;
            set;
        }

        public string RegionName
        {
            get;
            set;
        }

        public string RegionDescription
        {
            get;
            set;
        }

        public string MapName
        {
            get;
            set;
        }

        public string ReferencedMapName
        {
            get;
            set;
        }

        public string DataSetName
        {
            get;
            set;
        }

        public DataItem DataItem
        {
            get;
            set;
        }

        public List<string> Maps
        {
            get;
            set;
        }
    }
}
