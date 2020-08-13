using MapProject.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject
{
    public interface ISaveProvider
    {

        List<Map> GetMap(string name);

        List<string> GetMaps();

        void SaveMap(Map project);
    }
}
