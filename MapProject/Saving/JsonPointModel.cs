using MapProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Saving
{
    class JsonPointModel
    {
        [JsonProperty(PropertyName = "p")]
        public string ParentId
        {
            get;
            set;
        }

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
            this.ParentId = point.ParentId;
            this.X = point.X;
            this.X = point.Y;
            this.Processed = Processed;
        }

        public static Point ToPoint(JsonPointModel pointModel)
        {
            if (pointModel == null)
            {
                return null;
            }

            return new Point(pointModel.X, pointModel.Y, pointModel.ParentId, pointModel.IsBorder != 0, pointModel.Processed);
        }
    }
}
