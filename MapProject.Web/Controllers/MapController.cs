using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MapProject.Model;
using MapProject.Parsing;
using MapProject.Saving;
using MapProject.Statistic;
using MapProject.Statistic.FileSystem;
using MapProject.Web.Components;
using MapProject.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProject.Web.Controllers
{

    public static class MapContext
    {
        public static string FirstRegionId
        {
            get;
            private set;
        }

        public static string SecondRegionId
        {
            get;
            private set;
        }

        public static int Step
        {
            get;
            private set;
        }

        public static void SetRegionToCompare(string regionToCompare)
        {
            if (regionToCompare == SecondRegionId || regionToCompare == FirstRegionId)
            {
                return;
            }

            if (Step == 1)
            {
                SecondRegionId = regionToCompare;
                Step = 0;
            }
            else
            {

                if (Step == 0)
                {
                    FirstRegionId = regionToCompare;
                    Step++;
                }
            }

        }

    }


    public static class Strings
    {
        public static string NamePropertyKey = "Назва";
        public static string DescriptionPropertyKey = "Description";
        public static string ReferencedMapKey = "Пов'язана карта";

        public static string DeleteActionString = "Видалити";
        public static string DeleteAllActionString = "Видалити на карті";
        public static string ExpandActionString = "Встановити на карті";

        public static string NoDataPropertyKey = "Інформація відсутня";
    }

    public class MapController : Controller
    {
        Manager _manager;
        IWebHostEnvironment _hostingEnvironment;

        public MapController(IWebHostEnvironment hostingEnvironment, Manager manager)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._manager = manager;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RenderMap(string mapName, string dataSetName, string regionId)
        {
            Map map = this._manager.GetMap(mapName);

            List<string> dataSetNames = this._manager.GetDataSets(map);

            DataSet dataSet = null;

            if (!String.IsNullOrWhiteSpace(dataSetName))
            {
                dataSet = this._manager.GetDataSet(dataSetName, map);
            }

            RenderMapModel renderMapModel = new RenderMapModel(map, dataSetNames, dataSet, regionId);

            return View("RenderMap", renderMapModel);
        }

        [HttpPost]
        public IActionResult CreateMap(IFormFile mapImageFile)
        {
            CreateMapModel model = null;

            if (mapImageFile != null)
            {
                var folder = Path.Combine(this._hostingEnvironment.ContentRootPath, "tmp");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var filePath = Path.Combine(folder, mapImageFile.FileName);

                if (mapImageFile.Length > 0)
                {

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        mapImageFile.CopyTo(fileStream);
                    }


                }

                model = new CreateMapModel(filePath);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateMap()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ParseMapFromImage(string imageFilePath)
        {
            var map = this._manager.PrarseMapFromImage(imageFilePath);

            this._manager.SaveMap(map);

            return RedirectToAction("RenderMap", new { dataSetName = String.Empty, mapName = map.Name });
        }

        public IActionResult RenderMaps()
        {

            var listOfMaps = this._manager.GetMaps();


            return View("RenderMap", new RenderMapModel(listOfMaps));
        }

        public IActionResult SaveProperty(string propertyName, string propertyValue, string dataSetKey, string mapName, string regionId)
        {

            var map = this._manager.GetMap(mapName);

            var dataSet = this._manager.GetDataSet(dataSetKey, map);

            //var property = dataSet.GetProperty(propertyName, regionId);


            if (!String.IsNullOrWhiteSpace(regionId))
            {
                var dataItem = dataSet.GetDataItem(regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                    dataSet.DataItems.Add(dataItem);
                }

                var property = dataItem.GetProperty(propertyName);

                if (property == null)
                {
                    property = new DataProperty<string>(propertyName, propertyValue);
                    dataItem.Properties.Add(property);
                }
                else
                {
                    property.Value = propertyValue;
                }

                this._manager.SaveDataSet(dataSet, map);
            }

            return this.ManageRegion(regionId, dataSetKey, mapName);
        }

        public IActionResult SaveStatistic(string statisticName, double statisticValue, string dataSetKey, string mapName, string regionId)
        {

            var map = this._manager.GetMap(mapName);

            var dataSet = this._manager.GetDataSet(dataSetKey, map);

            if (!String.IsNullOrWhiteSpace(regionId))
            {
                var dataItem = dataSet.GetDataItem(regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                    dataSet.DataItems.Add(dataItem);
                }

                var statistic = dataItem.GetStatistic(statisticName);

                if (statistic == null)
                {
                    statistic = new DataProperty<double>(statisticName, statisticValue);
                    dataItem.Statistics.Add(statistic);
                }
                else
                {
                    statistic.Value = statisticValue;
                }

                this._manager.SaveDataSet(dataSet, map);
            }

            return this.ManageRegion(regionId, dataSetKey, mapName);
        }

        public IActionResult ExpanadProperties(string regionId, string dataSetName, string mapName)
        {
            var map = this._manager.GetMap(mapName);
            var dataSet = this._manager.GetDataSet(dataSetName, map);
            var exampleDataItem = dataSet.GetDataItem(regionId);

            if (exampleDataItem != null)
            {
                foreach (var region in map.Regions)
                {
                    var currentDataItem = dataSet.GetDataItem(region.Id);

                    if (currentDataItem == null)
                    {
                        currentDataItem = new DataItem(region.Id);
                        dataSet.DataItems.Add(currentDataItem);
                    }
                    if (regionId != region.Id)
                    {
                        foreach (var property in exampleDataItem.Properties)
                        {
                            if (!currentDataItem.Properties.Any(x => x.Name == property.Name))
                            {
                                var newPropety = new DataProperty<string>(property.Name, String.Empty);

                                currentDataItem.Properties.Add(newPropety);
                            }

                        }

                        foreach (var statistic in exampleDataItem.Statistics)
                        {
                            if (!currentDataItem.Statistics.Any(x => x.Name == statistic.Name))
                            {
                                var newStatistic = new DataProperty<double>(statistic.Name, 0);

                                currentDataItem.Statistics.Add(newStatistic);
                            }

                        }
                    }

                }

                this._manager.SaveDataSet(dataSet, map);


            }

            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        public IActionResult ManageDataProperty(string dataPropertyName, string dataSetName, string mapName, string regionId, bool ifStatistic, string toDo)
        {

            var regionsToProcess = new List<string>();

            var map = this._manager.GetMap(mapName);

            var dataSet = this._manager.GetDataSet(dataSetName, map);


            if (toDo == Strings.DeleteActionString || toDo == Strings.DeleteAllActionString)
            {
                if (toDo == Strings.DeleteAllActionString)
                {
                    regionsToProcess = map.Regions.Select(x => x.Id).ToList();
                }
                if (toDo == Strings.DeleteActionString)
                {
                    regionsToProcess.Add(regionId);


                }

                foreach (var region in regionsToProcess)
                {
                    var dataItem = dataSet.GetDataItem(region);

                    if (ifStatistic)
                    {
                        dataItem.Statistics.RemoveAll(x => x.Name == dataPropertyName);
                    }
                    else
                    {
                        dataItem.Properties.RemoveAll(x => x.Name == dataPropertyName);
                    }
                }

            }

            if (toDo == Strings.ExpandActionString)
            {
                var dataItem = dataSet.GetDataItem(regionId);

                if (ifStatistic)
                {
                    //var statistic = dataItem.Statistics.FirstOrDefault(x => x.Name == dataPropertyName);

                    foreach (var region in map.Regions)
                    {
                        var regionDataItem = dataSet.GetDataItem(region.Id);

                        if (regionDataItem == null)
                        {
                            regionDataItem = new DataItem(region.Id);
                            dataSet.DataItems.Add(regionDataItem);
                        }

                        if (regionDataItem.GetStatistic(dataPropertyName) == null)
                        {
                            regionDataItem.Statistics.Add(new DataProperty<double>(dataPropertyName, 0));
                        }
                    }

                }
                else
                {
                    //var property = dataItem.Properties.FirstOrDefault(x => x.Name == dataPropertyName);

                    foreach (var region in map.Regions)
                    {
                        var regionDataItem = dataSet.GetDataItem(region.Id);

                        if (regionDataItem == null)
                        {
                            regionDataItem = new DataItem(region.Id);
                            dataSet.DataItems.Add(regionDataItem);
                        }

                        if (regionDataItem.GetProperty(dataPropertyName) == null)
                        {
                            regionDataItem.Properties.Add(new DataProperty<string>(dataPropertyName, String.Empty));
                        }
                    }
                }

            }


            this._manager.SaveDataSet(dataSet, map);


            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        public IActionResult AddAttachment(string dataSetName, string mapName, string regionId, IFormFile attachmentFile)
        {

            var map = this._manager.GetMap(mapName);
            //CreateMapModel model = null;

            if (attachmentFile != null)
            {
                var folder = Path.Combine(this._hostingEnvironment.ContentRootPath, "tmp");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var filePath = Path.Combine(folder, attachmentFile.FileName);

                if (attachmentFile.Length > 0)
                {

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        attachmentFile.CopyTo(fileStream);
                    }


                }

                this._manager.AddAttachment(dataSetName, regionId, map, filePath);
            }

            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        public IActionResult DeleteAttachment(string dataSetName, string mapName, string regionId, string attachmentName)
        {
            var map = this._manager.GetMap(mapName);

            var dataSet = this._manager.GetDataSet(dataSetName, map);

            var dataItem = dataSet.GetDataItem(regionId);

            var attachment = dataItem.Attachments.FirstOrDefault(x => x.Name == attachmentName);

            if (System.IO.File.Exists(attachment.Value))
            {
                System.IO.File.Delete(attachment.Value);
            }

            dataItem.Attachments.Remove(attachment);

            this._manager.SaveDataSet(dataSet, map);

            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        public IActionResult DownloadAttachment(string regionId, string dataSetName, string mapName, string attachmentName)
        {

            var map = this._manager.GetMap(mapName);
            var dataSet = this._manager.GetDataSet(dataSetName, map);
            var dataItem = dataSet.GetDataItem(regionId);

            var attachment = dataItem.Attachments.FirstOrDefault(x => x.Name == attachmentName);

            string mimeType = String.Empty;

            FileStream stream = new FileStream(attachment.Value, FileMode.Open);


            //var content = new System.IO.MemoryStream(attachment.Value);

            var mimeProvider = new FileExtensionContentTypeProvider();

            if (!mimeProvider.TryGetContentType(attachment.Value, out mimeType))
            {
                mimeType = "application/octet-stream";
            }

            //var readStream = fileInfo.CreateReadStream();

            return File(stream, mimeType, attachment.Name);
        }

        public IActionResult ManageRegion(string regionId, string dataSetName, string mapName)
        {
            return ViewComponent(ManageRegionViewComponent.Name, new BaseModel(mapName, dataSetName, regionId));
        }

        public IActionResult AssignMap(string mapToAssignName, string regionId, string dataSetName, string mapName)
        {

            Map map = this._manager.GetMap(mapName);
            Map mapToAssign = this._manager.GetMap(mapToAssignName);

            if (mapToAssign != null)
            {
                var dataSet = this._manager.GetDataSet(dataSetName, map);

                var dataItem = dataSet.GetDataItem(regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                }

                var property = dataItem.GetProperty(Strings.ReferencedMapKey);


                var count = dataItem.Properties.Count(x => x.Name.Contains(Strings.ReferencedMapKey));


                property = new DataProperty<string>(Strings.ReferencedMapKey + " #" + (count + 1), mapToAssignName);

                dataItem.Properties.Add(property);


                this._manager.SaveDataSet(dataSet, map);

            }

            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        public IActionResult AddRegionToCompare(string mapName, string dataSetName, string regionToCompare)
        {
            MapContext.SetRegionToCompare(regionToCompare);

            string firstRegionName = String.Empty;
            string secondRegionName = String.Empty;

            var map = this._manager.GetMap(mapName);
            var dataSet = this._manager.GetDataSet(dataSetName, map);



            var firstDataItem = dataSet.GetDataItem(MapContext.FirstRegionId);
            var secondDataItem = dataSet.GetDataItem(MapContext.SecondRegionId);


            //var firstRegionNameProperty = dataSet.GetProperty(Strings.NamePropertyKey, CompareContext.FirstRegionId);
            //var secondRegionNameProperty = dataSet.GetProperty(Strings.NamePropertyKey, CompareContext.SecondRegionId);


            //var firstRegionNameProperty

            //firstRegionName = dataSet.GetProperty(Strings.NamePropertyKey, CompareContext.FirstRegionId) == null ? String.Empty : ;

            //string 

            if (firstDataItem != null)
            {
                var nameProperty = firstDataItem.GetProperty(Strings.NamePropertyKey);

                firstRegionName = nameProperty == null ? MapContext.FirstRegionId : nameProperty.Value;

            }

            if (secondDataItem != null)
            {
                var nameProperty = secondDataItem.GetProperty(Strings.NamePropertyKey);

                secondRegionName = nameProperty == null ? MapContext.SecondRegionId : nameProperty.Value;

            }

            return ViewComponent(CompareRegionsPaneViewComponent.Name, new CopmareRegionsPaneModel(firstRegionName, secondRegionName, new BaseModel(mapName, dataSetName, null)));
        }

        public IActionResult CreateDataSet(string dataSetName, string mapName)
        {
            DataSet dataSet = new DataSet(dataSetName);

            this._manager.SaveDataSet(dataSet, this._manager.GetMap(mapName));


            return RedirectToAction("RenderMap", new { dataSetName = dataSetName, mapName = mapName });
        }

        public IActionResult LoadDataSet(string dataSetName, string mapName)
        {
            return RedirectToAction("RenderMap", new { dataSetName = dataSetName, mapName = mapName });
        }

        public IActionResult DeleteDataSet(string dataSetName, string mapName)
        {

            Map map = this._manager.GetMap(mapName);

            this._manager.RemoveDataSet(dataSetName, map);

            return RedirectToAction("RenderMap", new { dataSetName = String.Empty, mapName = mapName });
        }

        public IActionResult CloseDataSet(string mapName)
        {
            return RedirectToAction("RenderMap", new { dataSetName = String.Empty, mapName = mapName });
        }


        public class StatisticColorItem
        {

            public string RegionId
            {
                get;
                set;
            }

            public double Value
            {
                get;
                set;
            }

            public bool HasData
            {
                get;
                set;
            }

            public Rgb24 Color
            {
                get;
                set;
            }

            public double Opacity
            {
                get;
                set;
            }
        }

        public IActionResult DrawStatistic(string statisticName, string dataSetName, string mapName, string color)
        {


            var colorsDictionary = new List<StatisticColorItem>();

            Map map = this._manager.GetMap(mapName);
            DataSet dataSet = this._manager.GetDataSet(dataSetName, map);

            var dataItems = dataSet.DataItems.Where(x => x.StructureId == dataSet.DataItems.First().StructureId);

            List<double> values = new List<double>();

            foreach (var dataItem in dataSet.DataItems)
            {
                DataProperty<double> statistic = dataItem.GetStatistic(statisticName);

                if (statistic != null)
                {
                    values.Add(statistic.Value);
                }
            }

            double minValue = values.Min();
            double maxValue = values.Max();

            List<StatisticColorItem> items = new List<StatisticColorItem>();

            foreach (var region in map.Regions)
            {
                DataItem dataItem = dataSet.GetDataItem(region.Id);

                DataProperty<double> statistic = dataItem == null ? null : dataItem.GetStatistic(statisticName);

                StatisticColorItem item = new StatisticColorItem();

                item.RegionId = region.Id;

                if (statistic == null || dataItem.GetProperty(Strings.NoDataPropertyKey) != null)
                {
                    item.HasData = false;
                    item.Color = Color.White;
                    item.Opacity = 1;
                }
                else
                {

                    //string color = Color.Red.ToString();

                    item.HasData = true;
                    item.Color = Rgba32.ParseHex(color).Rgb;
                    item.Value = statistic.Value;

                    if (maxValue == minValue)
                    {
                        item.Opacity = 1;
                    }
                    else
                    {
                        item.Opacity = (item.Value - minValue) / (maxValue - minValue) + 0.1;
                    }

                }



                items.Add(item);

            }

            return View("RenderMap", new RenderMapModel(mapName, dataSetName, statisticName, items) { Map = map, DataSet = dataSet });
        }


        public IActionResult Information()
        {
            return this.View();
        }

        public IActionResult CompareRegions(string mapName, string dataSetName)
        {


            if (String.IsNullOrWhiteSpace(MapContext.FirstRegionId) || String.IsNullOrWhiteSpace(MapContext.SecondRegionId))
            {
                return new EmptyResult();
            }

            var map = this._manager.GetMap(mapName);
            var dataSet = this._manager.GetDataSet(dataSetName, map);


            var dataItem1 = dataSet.GetDataItem(MapContext.FirstRegionId);
            var dataItem2 = dataSet.GetDataItem(MapContext.SecondRegionId);

            var region1 = map.Regions.FirstOrDefault(x => x.Id == MapContext.FirstRegionId);
            var region2 = map.Regions.FirstOrDefault(x => x.Id == MapContext.SecondRegionId);

            return this.View(new CompareRegionsModel(region1, dataItem1, region2, dataItem2, mapName, dataSetName));

        }

    }
}