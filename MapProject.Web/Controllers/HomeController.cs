using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MapProject.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Information()
        {
            return this.View();
        }

        public IActionResult Theorie()
        {
            return this.View();
        }

        public IActionResult DoSmth()
        {
            return this.View();
        }

        
    }





}