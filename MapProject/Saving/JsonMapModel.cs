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

        public JsonMapModel(Map map) : this()
        {
            this.Name = map.Name;

            this.Regions = map.Regions.Select(x => new JsonRegionModel(x)).ToList();
        }

        public Map ToMap()
        {
            return new Map(this.Name, this.Regions.Select(x => x.ToRegion()));
        }
    }
}
