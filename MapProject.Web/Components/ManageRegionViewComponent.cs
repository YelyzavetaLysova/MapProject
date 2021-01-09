using MapProject.Statistic;
using MapProject.Web.Controllers;
using MapProject.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Components
{
    public class ManageRegionViewComponent : ViewComponent
    {
        public static string Name
        {
            get
            {
                return "ManageRegion";
            }
        }


        Manager _manager;

        public ManageRegionViewComponent(Manager manager)
        {
            this._manager = manager;
        }

        public IViewComponentResult Invoke(BaseModel model)
        {

            string mapName = model.MapName;
            string dataSetName = model.DataSetName;
            string regionId = model.RegionId;

            var map = this._manager.GetMap(mapName);

            DataSet dataSet = null;

            if (!String.IsNullOrEmpty(dataSetName))
            {
                dataSet = this._manager.GetDataSet(dataSetName, map);
            }

            DataItem dataItem = null;

            string regionName = String.Empty;
            string regionDescription = String.Empty;
            List<string> referencedMaps = new List<string>();

            if (dataSet != null)
            {
                dataItem = dataSet.GetDataItem(regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                    dataSet.DataItems.Add(dataItem);


                    this._manager.SaveDataSet(dataSet, map);
                }

                var nameProperty = dataItem.GetProperty(Strings.NamePropertyKey);
                var descriptionProperty = dataItem.GetProperty(Strings.DescriptionPropertyKey);
                
                referencedMaps = dataItem.Properties.Where(x => x.Name.Contains(Strings.ReferencedMapKey)).Select(x => x.Value).ToList();

                if (nameProperty != null)
                {
                    regionName = nameProperty.Value;
                }

                if (descriptionProperty != null)
                {
                    regionDescription = descriptionProperty.Value;
                }

            }

            var maps = this._manager.GetMaps();

            maps.RemoveAll(x => x == mapName);

            return View(new ManageRegionModel(regionName, regionDescription, referencedMaps, dataItem, maps, model));
        }
    }
}
