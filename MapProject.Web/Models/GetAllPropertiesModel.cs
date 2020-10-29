using MapProject.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class GetAllPropertiesModel
    {

        public string MapName
        {
            get;
            set;
        }

        public string DataSetId
        {
            get;
            set;
        }

        public string RegionId
        {
            get;
            set;
        }

        public DataItem DataItem
        {
            get;
            set;
        }

        public GetAllPropertiesModel()
        {

        }

        public GetAllPropertiesModel(string mapName, string dataSetId, string regionId, DataItem item)
        {
            this.MapName = mapName;
            this.DataSetId = dataSetId;
            this.RegionId = regionId;
            this.DataItem = item;
        }
    }
}
