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

        public IActionResult RenderMap(string mapName, string dataSetName)
        {
            Map map = this._manager.GetMap(mapName);

            List<string> dataSetNames = this._manager.GetDataSets(map);

            DataSet dataSet = null;

            if (!String.IsNullOrWhiteSpace(dataSetName))
            {
                dataSet = this._manager.GetDataSet(dataSetName, map);
            }

            RenderMapModel renderMapModel = new RenderMapModel(map, dataSetNames, dataSet);

            return View("RenderMap", renderMapModel);
        }

        public IActionResult CreateDataSet(string dataSetName, string mapName)
        {
            DataSet dataSet = new DataSet(dataSetName);

            this._manager.SaveDataSet(dataSet, this._manager.GetMap(mapName));


            return this.RenderMap(mapName, dataSetName);
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


        
    }
}