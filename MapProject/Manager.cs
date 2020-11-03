using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using MapProject.Parsing;
using MapProject.Model;
using System.Linq;
using MapProject.Saving;
using MapProject.Statistic;
using System;

namespace MapProject
{
    public class Manager
    {

        Dictionary<string, object> _cache;

        IMapParser _parser;
        IMapProvider _mapProvider;
        IStatisticProvider _statisticProvider;
        
        public Manager(IMapParser parser, IMapProvider mapProvider, IStatisticProvider statisticProvider)
        {
            this._parser = parser;
            this._mapProvider = mapProvider;
            this._statisticProvider = statisticProvider;

            this._cache = new Dictionary<string, object>();
        }

        public Map PrarseMapFromImage(string pathToImage)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(pathToImage);

            return this.PrarseMapFromImage(image, pathToImage.Split('/', '\\').Last());
        }

        public Map PrarseMapFromImage(Image<Rgba32> image, string name)
        {
            var regions = this._parser.ParseImage(image);

            return new Map(name, regions);
        }

        public Map GetMap(string name)
        {
            if (this._cache.ContainsKey(name))
            {
                return this._cache[name] as Map;
            }

            var map = this._mapProvider.GetMap(name);

            if (map != null)
            {
                this._cache[name] = map;
            }

            return map;
        }

        public void SaveMap(Map map)
        {
            this._mapProvider.SaveMap(map);

            this._cache[map.Name] = map;

        }

        public List<string> GetMaps()
        {
            return this._mapProvider.GetMaps();
        }


        public List<string> GetDataSets(Map map)
        {
            return this._statisticProvider.GetDataSets(map);
        }


        public DataSet GetDataSet(string dataSetKey, Map map)
        {
            string dataSetCacheKey = GetDataSetCacheKey(dataSetKey, map.Name);

            if (this._cache.ContainsKey(dataSetCacheKey))
            {
                return this._cache[dataSetCacheKey] as DataSet;
            }

            var dataSet = this._statisticProvider.GetDataSet(dataSetKey, map);

            if (dataSet != null)
            {
                this._cache[dataSetKey] = dataSet;
            }

            return dataSet;
        }

        public void SaveDataSet(DataSet dataSet, Map map)
        {
            this._statisticProvider.SaveDataSet(dataSet, map);

            this._cache[this.GetDataSetCacheKey(dataSet.Key, map.Name)] = dataSet;
        }

        public void AddAttachment(string dataSetKey, string regionId, Map map, string pathToFile)
        {
            this._statisticProvider.AddAttachment(dataSetKey, regionId, map, pathToFile);
        }

        public void RemoveDataSet(string dataSetKey, Map map)
        {
            this._statisticProvider.RemoveDataSet(dataSetKey, map);

            string dataSetCacheKey = GetDataSetCacheKey(dataSetKey, map.Name);

            if (this._cache.ContainsKey(dataSetCacheKey))
            {
                this._cache.Remove(dataSetCacheKey);
            }
        }


        private string GetDataSetCacheKey(string dataSetKey, string mapName)
        {
            return mapName + "_" + dataSetKey;
        }
    }
}
