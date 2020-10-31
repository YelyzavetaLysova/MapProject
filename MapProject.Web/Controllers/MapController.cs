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
using MapProject.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace MapProject.Web.Controllers
{
    public class MapController : Controller
    {
        Manager _manager;
        IWebHostEnvironment _hostingEnvironment;

        public MapController(IWebHostEnvironment hostingEnvironment)
        {

            this._hostingEnvironment = hostingEnvironment;

            IMapProvider saveProvider = new JsonFileSystemMapProvider();
            IMapParser mapParser = new MapParser();
            IStatisticProvider statisticProvider = new JsonFyleSystemStatisticProvider();

            this._manager = new Manager(mapParser, saveProvider, statisticProvider);
        }
        public IActionResult Index()
        {
            return View();
        }


        //public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> battlePlans)
        //{

        //}

        //public 


        public IActionResult RenderMap(string mapName, string dataSetName)
        {
            Map map = this._manager.GetMap(mapName);

            List<string> dataSetNames = this._manager.GetDataSets(map);



            DataSet dataSet = null;

            if (!String.IsNullOrWhiteSpace(dataSetName))
            {
                dataSet = this._manager.GetDataSet(dataSetName, map);
            }

            RenderMapModel renderMapModel = new RenderMapModel(map, dataSetNames, dataSet, JsonConvert.SerializeObject(dataSet));

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

            return View("CreateMap");
        }

        public IActionResult RenderMaps()
        {

            var listOfMaps = this._manager.GetMaps();


            return View(listOfMaps);
        }

        public IActionResult SaveProperty(string propertyName, string propertyValue, string dataSetKey, string mapName, string regionId)
        {

            var map = this._manager.GetMap(mapName);

            var dataSet = this._manager.GetDataSet(dataSetKey, map);

            if (!String.IsNullOrWhiteSpace(regionId))
            {
                var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                }

                var property = dataItem.Properties.FirstOrDefault(x => x.Name == propertyName);

                if (property == null)
                {
                    property = new DataProperty<string>(propertyName, propertyValue);
                    dataItem.Properties.Add(property);
                }
                else
                {
                    property.Value = propertyValue;
                }



                dataSet.DataItems.Add(dataItem);

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
                var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                }

                var statistic = dataItem.Statistics.FirstOrDefault(x => x.Name == statisticName);

                if (statistic == null)
                {
                    statistic = new DataProperty<double>(statisticName, statisticValue);
                    dataItem.Statistics.Add(statistic);
                }
                else
                {
                    statistic.Value = statisticValue;
                }



                dataSet.DataItems.Add(dataItem);

                this._manager.SaveDataSet(dataSet, map);
            }

            return this.ManageRegion(regionId, dataSetKey, mapName);
        }

        //public IActionResult ManageRegion(string regionId, string dataSetName, string mapName)
        //{
        //    var map = this._manager.GetMap(mapName);

        //    var dataSet = this._manager.GetDataSet(dataSetName, map);

        //    var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

        //    return ViewComponent("ManageRegion", new ManageRegionModel() { MapName = mapName, DataSetName = dataSetName, RegionId = regionId, DataItem = dataItem });
        //}


        public IActionResult ExpanadProperties(string regionId, string dataSetName, string mapName)
        {
            var map = this._manager.GetMap(mapName);
            var dataSet = this._manager.GetDataSet(dataSetName, map);
            var exampleDataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

            if (exampleDataItem != null)
            {
                foreach (var region in map.Regions)
                {
                    var currentDataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == region.Id);

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


            if (toDo == "Delete All" || toDo == "Delete")
            {
                if (toDo == "Delete All")
                {
                    regionsToProcess = map.Regions.Select(x => x.Id).ToList();
                }
                if (toDo == "Delete")
                {
                    regionsToProcess.Add(regionId);


                }

                foreach (var region in regionsToProcess)
                {
                    var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == region);

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

            if (toDo == "Expand")
            {
                var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

                if (ifStatistic)
                {
                    //var statistic = dataItem.Statistics.FirstOrDefault(x => x.Name == dataPropertyName);

                    foreach(var region in map.Regions)
                    {
                        var regionDataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == region.Id);

                        if (regionDataItem.Statistics.FirstOrDefault(x => x.Name == dataPropertyName) == null)
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
                        var regionDataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == region.Id);

                        if (regionDataItem == null)
                        {
                            regionDataItem = new DataItem(region.Id);
                            dataSet.DataItems.Add(regionDataItem);
                        }

                        if (regionDataItem.Properties.FirstOrDefault(x => x.Name == dataPropertyName) == null)
                        {
                            regionDataItem.Properties.Add(new DataProperty<string>(dataPropertyName, String.Empty));
                        }
                    }
                }

            }


                this._manager.SaveDataSet(dataSet, map);


            return this.ManageRegion(regionId, dataSetName, mapName);
        }


                    //        <input type = "file" name="attachmentFile" />

                    //<input class="form-control" type="text" name="dataSetName" value="@Model.DataSetName" hidden />
                    //<input class="form-control" type="text" name="mapName" value="@Model.MapName" hidden />
                    //<input class="form-control" type="text" name="regionId" value="@Model.RegionId" hidden />

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

            var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

            var attachment = dataItem.Attachments.FirstOrDefault(x => x.Name == attachmentName);

            if (System.IO.File.Exists(attachment.Value))
            {
                System.IO.File.Delete(attachment.Value);
            }

            dataItem.Attachments.Remove(attachment);

            this._manager.SaveDataSet(dataSet, map);

            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        #region Region Management

        public IActionResult DownloadAttachment(string regionId, string dataSetName, string mapName, string attachmentName)
        {

            var map = this._manager.GetMap(mapName);
            var dataSet = this._manager.GetDataSet(dataSetName, map);
            var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

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
            var map = this._manager.GetMap(mapName);

            DataSet dataSet = null;

            if (!String.IsNullOrEmpty(dataSetName))
            {
                dataSet = this._manager.GetDataSet(dataSetName, map);
            }

            DataItem dataItem = null;

            string regionName = String.Empty;
            string regionDescription = String.Empty;
            string referencedMap = String.Empty;

            if (dataSet != null)
            {
                dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                    dataSet.DataItems.Add(dataItem);


                    this._manager.SaveDataSet(dataSet, map);
                }

                var nameProperty = dataItem.Properties.FirstOrDefault(x => x.Name == "Name");
                var descriptionProperty = dataItem.Properties.FirstOrDefault(x => x.Name == "Description");
                var referencedMapProperty = dataItem.Properties.FirstOrDefault(x => x.Name == "ReferencedMap");

                if (nameProperty != null)
                {
                    regionName = nameProperty.Value;
                }

                if (descriptionProperty != null)
                {
                    regionDescription = descriptionProperty.Value;
                }

                if (referencedMapProperty != null)
                {
                    referencedMap = referencedMapProperty.Value;
                }

            }

            var maps = this._manager.GetMaps();

            maps.RemoveAll(x => x == mapName);

           

            return ViewComponent("ManageRegion", new ManageRegionModel() { RegionId = regionId, DataSetName = dataSetName, MapName = mapName, DataItem = dataItem, RegionName = regionName, RegionDescription = regionDescription, Maps = maps, ReferencedMapName = referencedMap });
        }

        public IActionResult AssignMap(string mapToAssignName, string regionId, string dataSetName, string mapName)
        {

            Map map = this._manager.GetMap(mapName);
            Map mapToAssign = this._manager.GetMap(mapToAssignName);

            if (mapToAssign != null)
            {
                var dataSet = this._manager.GetDataSet(dataSetName, map);

                var dataItem = dataSet.DataItems.FirstOrDefault(x => x.StructureId == regionId);

                if (dataItem == null)
                {
                    dataItem = new DataItem(regionId);
                }

                var property = dataItem.Properties.FirstOrDefault(x => x.Name == "ReferencedMap");

                if (property == null)
                {
                    property = new DataProperty<string>("ReferencedMap", mapToAssignName);

                    dataItem.Properties.Add(property);
                }

                this._manager.SaveDataSet(dataSet, map);

            }

            return this.ManageRegion(regionId, dataSetName, mapName);
        }

        #endregion

        #region DataSet Management

        public IActionResult CreateDataSet(string dataSetName, string mapName)
        {
            DataSet dataSet = new DataSet(dataSetName);

            this._manager.SaveDataSet(dataSet, this._manager.GetMap(mapName));


            return this.RenderMap(mapName, dataSetName);
        }

        public IActionResult LoadDataSet(string dataSetName, string mapName)
        {
            return this.RenderMap(mapName, dataSetName);
        }

        public IActionResult DeleteDataSet(string dataSetName, string mapName)
        {

            Map map = this._manager.GetMap(mapName);

            this._manager.RemoveDataSet(dataSetName, map);

            return this.RenderMap(mapName, null);
        }

        public IActionResult CloseDataSet(string mapName)
        {
            return this.RenderMap(mapName, null);
        }


        #endregion

    }
}