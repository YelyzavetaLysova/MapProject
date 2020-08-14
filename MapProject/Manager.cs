using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using MapProject.Parsing;
using MapProject.Model;
using System.Linq;
using MapProject.Saving;
using Microsoft.Extensions.Logging;

namespace MapProject
{
    public class Manager
    {

        IMapParser _parser;
        ISaveProvider _provider;
        ILogger _logger;

        public MapProcessingSettings Settings
        {
            get;
            private set;
        }
        
        public Manager(MapProcessingSettings settings, IMapParser parser, ISaveProvider provider, ILogger logger)
        {
            this._parser = parser;
            this._provider = provider;
            this._logger = logger;
            this.Settings = settings;
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
            return this._provider.GetMap(name);
        }

        public void SaveMap(Map map)
        {
            this._provider.SaveMap(map);
        }

        public List<string> GetMaps()
        {
            return this._provider.GetMaps();
        }

    }
}
