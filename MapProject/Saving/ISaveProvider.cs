using MapProject.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Saving
{
    public interface ISaveProvider
    {
        Map GetMap(string name);

        List<string> GetMaps();

        void SaveMap(Map map);
    }
}
