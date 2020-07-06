using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProject
{
    public class MapManager
    {

        IMapParser _parser;
        
        public MapManager(IMapParser parser)
        {
            this._parser = parser;
        }

        public void ProcessMapFromImage(string pathToImage)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(pathToImage);

            this._parser.ParseImage(image);

        }
    }
}
