using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            ISaveProvider saveProvider = new JsonFileSystemSaveProvider();
            IMapParser mapParser = new MapParser();
            this._manager = new Manager(mapParser, saveProvider);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RenderMap(string mapName)
        {
           var map = this._manager.GetMap(mapName);
            return View(map);
        }

        public IActionResult RenderMaps()
        {

            var listOfMaps = this._manager.GetMaps();


            return View(listOfMaps);
        }
    }
}