using MapProject.Model;
using MapProject.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class CompareRegionsModel : BaseModel
    {
        public Region Region1
        {
            get;
            set;
        }

        public Region Region2
        {
            get;
            set;
        }

        public DataItem DataItem1
        {
            get;
            set;
        }

        public DataItem DataItem2
        {
            get;
            set;
        }

        public CompareRegionsModel(Region region1, DataItem dataItem1, Region region2, DataItem dataItem2, string mapName, string dataSetName) : base(mapName, dataSetName, null)
        {
            this.Region1 = region1;
            this.DataItem1 = dataItem1;
            this.Region2 = region2;
            this.DataItem2 = dataItem2;
        }
    }
}
