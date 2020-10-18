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

        public List<DataItem> DataItems
        {
            get;
            set;
        }

        public DataSet()
        {
            this.DataItems = new List<DataItem>();
        }

        public DataSet(string key) : this()
        {
            this.Key = key;
        }

        public DataSet(string key, List<DataItem> datas) : this(key)
        {
            this.DataItems = datas;
        }
    }
}
