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

        IMapParser _parser;
        IMapProvider _mapProvider;
        IStatisticProvider _statisticProvider;
        
        public Manager(IMapParser parser, IMapProvider mapProvider, IStatisticProvider statisticProvider)
        {
            this._parser = parser;
            this._mapProvider = mapProvider;
            this._statisticProvider = statisticProvider;
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
            return this._mapProvider.GetMap(name);
        }

        public void SaveMap(Map map)
        {
            this._mapProvider.SaveMap(map);
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
            return this._statisticProvider.GetDataSet(dataSetKey, map);
        }

        public void SaveDataSet(DataSet dataSet, Map map)
        {
            this._statisticProvider.SaveDataSet(dataSet, map);
        }

        public void RemoveDataSet(string dataSetKey, Map map)
        {
            this._statisticProvider.RemoveDataSet(dataSetKey, map);
        }
    }
}
