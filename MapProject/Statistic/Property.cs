using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class Property : DataProperty<string>
    {
        public Property(string key, string value) : base(key, value)
        {
        }
    }
}
