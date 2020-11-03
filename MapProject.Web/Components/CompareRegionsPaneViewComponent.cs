using MapProject.Web.Controllers;
using MapProject.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Components
{
    public class CompareRegionsPaneViewComponent : ViewComponent
    {

        Manager _manager;

        public static string Name
        {
            get
            {
                return "CompareRegionsPane";
            }
        }

        public CompareRegionsPaneViewComponent(Manager manager)
        {
            this._manager = manager;
        }

        public IViewComponentResult Invoke(CopmareRegionsPaneModel model)
        {
            var map = this._manager.GetMap(model.MapName);
            var dataSet = this._manager.GetDataSet(model.DataSetName, map);

            if (dataSet == null)
            {
                return View(new CopmareRegionsPaneModel(null, null, model));
            }

            var firstDataItem = dataSet.GetDataItem(MapContext.FirstRegionId);
            var secondDataItem = dataSet.GetDataItem(MapContext.FirstRegionId);

            var firstNameProperty = firstDataItem == null ? null : firstDataItem.GetProperty(Strings.NamePropertyKey);
            var secondNameProperty = secondDataItem == null ? null : secondDataItem.GetProperty(Strings.NamePropertyKey);

            var firstName = firstNameProperty == null || String.IsNullOrWhiteSpace(firstNameProperty.Value) ? MapContext.FirstRegionId : firstNameProperty.Value;
            var secondName = secondNameProperty == null || String.IsNullOrWhiteSpace(secondNameProperty.Value) ? MapContext.SecondRegionId : secondNameProperty.Value;


            return View(new CopmareRegionsPaneModel(firstName, secondName, model));
        }
    }
}
