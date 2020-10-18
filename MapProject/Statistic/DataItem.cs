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

        public List<DataProperty<string>> Properties
        {
            get;
            private set;
        }

        public List<DataProperty<double>> Statistics
        {
            get;
            private set;
        }

        public List<DataProperty<string>> Attachments
        {
            get;
            private set;
        }

        public DataItem(string structureId)
        {
            this.StructureId = structureId;

            this.Properties = new List<DataProperty<string>>();
            this.Statistics = new List<DataProperty<double>>();
            this.Attachments = new List<DataProperty<string>>();
        }
    }
}
