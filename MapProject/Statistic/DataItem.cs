using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class DataItem
    {
        public string StructureId
        {
            get;
            private set;
        }

        public List<Property> Properties
        {
            get;
            private set;
        }

        public List<Statistic> Statistics
        {
            get;
            private set;
        }

        public List<Attachment> Attachments
        {
            get;
            private set;
        }

        public DataItem(string structureId)
        {
            this.StructureId = structureId;

            this.Properties = new List<Property>();
            this.Statistics = new List<Statistic>();
            this.Attachments = new List<Attachment>();
        }
    }
}
