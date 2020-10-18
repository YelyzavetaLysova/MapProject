using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class DataProperty<T>
    {
        public string Key
        {
            get;
            private set;
        }

        public T Value
        {
            get;
            private set;
        }

        public DataProperty(string key, T value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
