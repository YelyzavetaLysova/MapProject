using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class CopmareRegionsPaneModel : BaseModel
    {
        public string FirstRegion
        {
            get;
            set;
        }

        public string SecondRegion
        {
            get;
            set;
        }

        public CopmareRegionsPaneModel(string firstRegion, string secondRegion, string mapName, string dataSetName, string regionId) : base(mapName, dataSetName, regionId)
        {
            this.FirstRegion = firstRegion;
            this.SecondRegion = secondRegion;
        }

        public CopmareRegionsPaneModel(string firstRegion, string secondRegion, BaseModel baseModel) : base(baseModel)
        {
            this.FirstRegion = firstRegion;
            this.SecondRegion = secondRegion;
        }
    }
}
