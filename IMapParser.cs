using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProject
{
    public interface IMapParser
    {
       Model.Region ParseImage(Image<Rgba32> img);
    }
}
