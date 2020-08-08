using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using MapProject.Parsing;
using MapProject.Model;

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




        public Map ProcessMapFromImage(string pathToImage)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(pathToImage);

            return this._parser.ParseImage(image);

        }
    }
}
