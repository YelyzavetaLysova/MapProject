using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class BaseModel
    {
        public string MapName
        {
            get;
            set;
        }

        public string DataSetName
        {
            get;
            set;
        }

        public string RegionId
        {
            get;
            set;
        }

        public BaseModel(string mapName, string dataSetName, string regionId)
        {
            this.MapName = mapName;
            this.DataSetName = dataSetName;
            this.RegionId = regionId;
        }

        public BaseModel(BaseModel model)
        {
            this.MapName = model.MapName;
            this.DataSetName = model.DataSetName;
            this.RegionId = model.RegionId;
        }
    }
}
