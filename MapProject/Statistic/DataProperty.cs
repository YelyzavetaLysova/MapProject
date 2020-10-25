using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class DataProperty<T>
    {
        public string Name
        {
            get;
            private set;
        }

        public T Value
        {
            get;
            set;
        }

        public DataProperty(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
