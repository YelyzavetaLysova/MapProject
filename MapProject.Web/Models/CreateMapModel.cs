using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{

    public class CreateMapModel : PageModel
    {
        public string FilePath
        {
            get;
            set;
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(this.FilePath);
            }
        }

        public CreateMapModel(string pathToImage)
        {
            this.FilePath = pathToImage;
        }

        public CreateMapModel()
        {
        }
    }
}
