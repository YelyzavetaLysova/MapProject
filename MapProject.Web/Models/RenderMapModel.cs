using MapProject.Model;
using MapProject.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class RenderMapModel
    {
        public Map Map
        {
            get;
            set;
        }

        public DataSet DataSet
        {
            get;
            set;
        }

        public List<string> DataSetNames
        {
            get;
            set;
        }

        public RenderMapModel(Map map, List<string> dataSetNames, DataSet dataSet)
        {
            this.DataSet = dataSet;
            this.Map = map;
            this.DataSetNames = dataSetNames;
        }

        public RenderMapModel()
        {
        }
    }
}
