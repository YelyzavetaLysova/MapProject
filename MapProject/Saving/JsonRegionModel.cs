using MapProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapProject.Saving
{
    class JsonRegionModel
    {
        [JsonProperty(PropertyName = "i")]
        public string Id
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "p")]
        public List<JsonPointModel> Points
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "m")]
        public JsonMapModel ReferencedMap
        {
            get;
            set;
        }

        public bool ShouldSerializeReferencedMap()
        {
            if (this.ReferencedMap != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonRegionModel()
        {

        }

        public JsonRegionModel(Region region)
        {
            this.Id = region.Id;
            this.Points = region.Points.Select(x => new JsonPointModel(x)).ToList();

            if (region.ReferencedMap != null)
            {
                this.ReferencedMap = new JsonMapModel(region.ReferencedMap);
            }
        }

        public static Region ToRegion(JsonRegionModel regionModel)
        {
            if (regionModel == null)
            {
                return null;
            }

            return new Region(regionModel.Id, regionModel.Points.Select(x => JsonPointModel.ToPoint(x)), JsonMapModel.ToMap(regionModel.ReferencedMap));
        }
    }
}
