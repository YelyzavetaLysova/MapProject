using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Statistic
{
    public class Attachment : BaseDataProperty<string>
    {
        public Attachment(string fileName, string filePath) : base(fileName, filePath)
        {
        }
    }
}
