using System;
using System.Collections.Generic;
using System.Text;
using MapProject.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProject.Parsing
{
    public interface IMapParser
    {
        IEnumerable<Region> ParseImage(Image<Rgba32> img);
    }
}
