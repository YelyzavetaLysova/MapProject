using MapProject.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Controllers
{
    public class MyController: Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult SaveMyModel(MyModel myModel)
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult SaveMyModel()
        {
            return this.View("RenderMyModel");
        }
    }
}
