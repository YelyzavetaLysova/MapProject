﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MapProject.Model;
using Newtonsoft.Json;

namespace MapProject.Saving
{
    public class JsonFileSystemMapProvider : IMapProvider
    {

        private string _path = Environment.CurrentDirectory + "/Maps";
        private string _extention = ".map";
        
        public JsonFileSystemMapProvider()
        {
            if(!Directory.Exists(this._path))
            {
                Directory.CreateDirectory(this._path);
            }
            
        }

        public Map GetMap(string name)
        {
            //Map result = new Map();

            string path = this._path + "/" + name + this._extention;

            if (!File.Exists(path))
            {
                return null;
            }

            var jsonMapModel = JsonConvert.DeserializeObject<JsonMapModel>(File.ReadAllText(path));

            return jsonMapModel.ToMap();
        }

        public List<string> GetMaps()
        {
            List<string> result = new List<string>();

            result.AddRange(Directory.GetFiles(this._path, "*" + this._extention).Select(x => { 
            
                return Path.GetFileNameWithoutExtension(x);


            }));

            return result;

        }

        public void SaveMap(Map map)
        {
            JsonMapModel mapModel = new JsonMapModel(map);

            File.WriteAllText(this._path + "/" + map.Name + this._extention, JsonConvert.SerializeObject(mapModel));
        }
    }
}
