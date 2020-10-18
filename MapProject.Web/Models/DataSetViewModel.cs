using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class DataSetViewModel
    {
        public string DataSetName
        {
            get;
            set;
        }

        public string MapName
        {
            get;
            set;
        }

        public DataSetViewModel(string dataSetName, string mapName)
        {
            this.DataSetName = dataSetName;
            this.MapName = mapName;
        }

        public DataSetViewModel()
        {

        }
    }
}
