using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class ManageDataSetModel
    {
        public string MapName
        {
            get;
            set;
        }

        public string DataSetName
        {
            get;
            set;
        }

        public List<string> DataSets
        {
            get;
            set;
        }

    }
}
