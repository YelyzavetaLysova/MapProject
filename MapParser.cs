using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MapProject.Model;

namespace MapProject
{
    public class MapParser
    {

        private MapProject.Model.Point[,] _points;

        public Region ParseImage(Bitmap img)
        {
            this._points = new MapProject.Model.Point[img.Width, img.Height];

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    this._points[i, j] = img.GetPixel(i, j);

                }
            }


        }
    }
}
