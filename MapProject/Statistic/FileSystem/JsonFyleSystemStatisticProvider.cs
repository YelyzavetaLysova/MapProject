using MapProject.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MapProject.Statistic.FileSystem
{
    public class JsonFyleSystemStatisticProvider : IStatisticProvider
    {

        private string _path = Environment.CurrentDirectory + "/Maps";
        private string _dataSetExtention = ".dataset";

        public JsonFyleSystemStatisticProvider()
        {
            if (!Directory.Exists(this._path))
            {
                Directory.CreateDirectory(this._path);
            }
        }

       
        public void SaveDataSet(DataSet set, Map map)
        {

            string dataSetFile = this.GenerateDataSetFilePath(set.Key, map.Name);

            if (!Directory.Exists(this.GenerateMapFolderPath(map.Name)))
            {
                Directory.CreateDirectory(this.GenerateMapFolderPath(map.Name));
            }

            File.WriteAllText(dataSetFile, JsonConvert.SerializeObject(set));
                        
        }

        private string GenerateDataSetFilePath(string dataSetKey, string mapName)
        {
            return this.GenerateMapFolderPath(mapName) + "/" + dataSetKey + this._dataSetExtention;
        }

        private string GenerateMapFolderPath(string mapName)
        {
            return this._path + "/" + mapName;
        }

        public DataSet GetDataSet(string dataSetKey, Map map)
        {
            string dataSetFile = this.GenerateDataSetFilePath(dataSetKey, map.Name);

            if (!File.Exists(dataSetFile))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<DataSet>(File.ReadAllText(dataSetFile));
        }

        public void RemoveDataSet(string dataSetKey, Map map)
        {
            string dataSetFile = this.GenerateDataSetFilePath(dataSetKey, map.Name);

            if (File.Exists(dataSetFile))
            {
                File.Delete(dataSetFile);
            }
        }


        public List<string> GetDataSets(Map map)
        {

            string mapFolderPath = this.GenerateMapFolderPath(map.Name);

            if (!Directory.Exists(mapFolderPath))
            {
                return new List<string>();
            }

            return Directory.GetFiles(mapFolderPath, "*" + this._dataSetExtention).Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
        }
    }
}
