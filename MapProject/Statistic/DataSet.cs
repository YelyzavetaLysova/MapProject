using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class DataSet
    {

        public string Key
        {
            get;
            set;
        }

        public List<DataItem> StructureDatas
        {
            get;
            set;
        }

        public DataSet()
        {
            this.StructureDatas = new List<DataItem>();
        }

        public DataSet(string key) : this()
        {
            this.Key = key;
        }

        public DataSet(string key, List<DataItem> datas) : this(key)
        {
            this.StructureDatas = datas;
        }
    }
}
