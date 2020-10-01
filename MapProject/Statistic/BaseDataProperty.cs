using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class BaseDataProperty<T>
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

        public BaseDataProperty(string key, T value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
