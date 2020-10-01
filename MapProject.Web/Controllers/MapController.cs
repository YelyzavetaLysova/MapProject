using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapProject.Model;
using MapProject.Parsing;
using MapProject.Saving;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Web.Controllers
{
    public class MapController : Controller
    {
        Manager _manager;
        public MapController()
        {
            IMapProvider saveProvider = new JsonFileSystemMapProvider();
            IMapParser mapParser = new MapParser();
            this._manager = new Manager(mapParser, saveProvider);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RenderMap(string mapName)
        {
            
            Map map = this._manager.GetMap(mapName);
            return View(map);
        }

        public IActionResult RenderMaps()
        {

            var listOfMaps = this._manager.GetMaps();


            return View(listOfMaps);
        }


        [HttpGet]
        public IActionResult AddStatisticKey()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStatisticKey(string parameter)
        {

            return View();
        }

    }
}