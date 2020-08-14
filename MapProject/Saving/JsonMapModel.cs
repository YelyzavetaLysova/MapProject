using MapProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapProject.Saving
{
    class JsonMapModel
    {
        [JsonProperty(PropertyName = "n")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "r")]
        public List<JsonRegionModel> Regions
        {
            get;
            set;
        }

        public JsonMapModel()
        {
         
        }

        public JsonMapModel(Map map)
        {
            this.Name = map.Name;
            this.Regions = map.Regions.Select(x => new JsonRegionModel(x)).ToList();
        }

        public static Map ToMap(JsonMapModel mapModel)
        {
            if (mapModel == null)
            {
                return null;
            }

            return new Map(mapModel.Name, mapModel.Regions.Select(x => JsonRegionModel.ToRegion(x)));
        }
    }
}
