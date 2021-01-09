using MapProject.Model;
using MapProject.Statistic;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using MapProject.Web.Controllers;


namespace MapProject.Web.Models
{
    public class RenderMapModel : BaseModel
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

        public List<string> Maps
        {
            get;
            set;
        }

        public List<MapController.StatisticColorItem> StatisticColorItems
        {
            get;
            set;
        }

        public string PropertyName
        {
            get;
            set;
        }

        public RenderMapModel(List<string> maps) : base(null, null, null)
        {
            this.Maps = maps;
        }

        public RenderMapModel(Map map, List<string> dataSetNames, DataSet dataSet, string regionId) : base(map.Name, dataSet == null ? null : dataSet.Key, regionId)
        {
            this.DataSet = dataSet;
            this.Map = map;
            this.DataSetNames = dataSetNames;
        }

        public RenderMapModel(string mapName, string dataSetName, string propertyName, List<MapController.StatisticColorItem> items) : base(mapName, dataSetName, null)
        {
            this.StatisticColorItems = items;
            this.PropertyName = propertyName;
            this.StatisticColorItems = items;
        }
    }
}
