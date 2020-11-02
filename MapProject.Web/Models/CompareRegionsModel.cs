using MapProject.Model;
using MapProject.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapProject.Web.Models
{
    public class CompareRegionsModel
    {
        public Region Region1
        {
            get;
            set;
        }

        public Region Region2
        {
            get;
            set;
        }

        public DataItem DataItem1
        {
            get;
            set;
        }

        public DataItem DataItem2
        {
            get;
            set;
        }

    }
}
