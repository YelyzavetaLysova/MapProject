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

        public Region ToRegion()
        {

            Map map = this.ReferencedMap == null ? null : this.ReferencedMap.ToMap();

            return new Region(this.Id, this.Points.Select(x => x.ToPoint(this.Id)), map);
        }
    }
}
