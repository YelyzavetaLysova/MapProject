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

        public JsonRegionModel()
        {

        }

        public JsonRegionModel(Region region)
        {
            this.Id = region.Id;
            this.Points = region.Points.Select(x => new JsonPointModel(x)).ToList();
        }

        public Region ToRegion()
        {
            return new Region(this.Id, this.Points.Select(x => x.ToPoint(this.Id)));
        }
    }
}
