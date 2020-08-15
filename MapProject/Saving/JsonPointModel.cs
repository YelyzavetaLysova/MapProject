using MapProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Saving
{
    class JsonPointModel
    {
        [JsonProperty(PropertyName = "x")]
        public int X
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "y")]
        public int Y
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "b")]
        public int IsBorder
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool Processed
        {
            get;
            set;
        }

        public JsonPointModel()
        {

        }

        public JsonPointModel(Point point)
        {
            this.IsBorder = point.IsBorder == false ? 0 : 1;
            this.X = point.X;
            this.X = point.Y;
            this.Processed = Processed;
        }

        public Point ToPoint(string parentId)
        {
            return new Point(this.X, this.Y, parentId, this.IsBorder != 0, this.Processed);
        }
    }
}
