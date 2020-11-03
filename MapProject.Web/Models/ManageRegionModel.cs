using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapProject.Statistic;

namespace MapProject.Web.Models
{
    public class ManageRegionModel  : BaseModel
    {
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
        public string ReferencedMap
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

        public ManageRegionModel(string regionName, string regionDescription, string referencedMap, DataItem dataItem, List<string> maps, BaseModel baseModel) : base(baseModel)
        {
            this.RegionName = regionName;
            this.RegionDescription = regionDescription;
            this.ReferencedMap = referencedMap;
            this.DataItem = dataItem;
            this.Maps = maps;
        }
    }
}
