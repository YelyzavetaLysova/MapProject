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
        public IViewComponentResult Invoke(ManageRegionModel model)
        {
            return View(model);
        }
    }
}
