using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using MapProject.Parsing;
using MapProject.Model;
using System.Linq;
using MapProject.Saving;

namespace MapProject
{
    public class Manager
    {

        IMapParser _parser;
        ISaveProvider _provider;
        
        public Manager(IMapParser parser, ISaveProvider provider)
        {
            this._parser = parser;
            this._provider = provider;
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
