using System;
using System.Collections.Generic;
using MapProject.Model;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace MapProject
{
    public class MapParser : IMapParser
    {

        private MapProject.Model.Point[,] _points;

        public Model.Region ParseImage(Image<Rgba32> img)
        {

            this._points = new MapProject.Model.Point[img.Width, img.Height];

            for (int y = 0; y < img.Height; y++)
            {
                Span<Rgba32> pixelsRow = img.GetPixelRowSpan(y);

                for (int x = 0; x < img.Width; x++)
                {
                    this._points[x, y] = new MapProject.Model.Point(x, y, this.isBorder(new Color(pixelsRow[x])));
                }
            }

            using (var imageToSave = new Image<Rgba32>(this._points.GetLength(0), this._points.GetLength(1)))
            {

                for (int y = 0; y < this._points.GetLength(1); y++)
                {
                    Span<Rgba32> pixelsRow = imageToSave.GetPixelRowSpan(y);

                    for (int x = 0; x < this._points.GetLength(0); x++)
                    {

                        if (this._points[x, y].IsBorder)
                        {
                            pixelsRow[x] = Color.Black;
                        }
                        else
                        {
                            pixelsRow[x] = Color.White;
                        }

                    }
                }

                using (Stream s = new FileStream(Environment.CurrentDirectory + "/savedimage.jpeg", FileMode.Create))
                {
                    imageToSave.SaveAsJpeg(s);

                }
            }

            MapProject.Model.Point p = this.GetRandomPoint();



            //region.Points.Add(p);

            //this.ProcessPoint(p);


            return new Region();
        }

        private void ProcessPoint(MapProject.Model.Point point, Region region = null)
        {
            if (region == null)
            {
                region = new Region();
            }
            
            if (point.Parent == region)
            {
                return;
            }

            if (point.Parent != null)
            {
                this.MergeRegions(region, point.Parent);

                return;
            }
            else
            {
                if (!point.IsBorder)
                {
                    region.Points.Add(point);
                    point.Parent = region;
                }
                else
                {
                    region = null;
                }
            }

            foreach (MapProject.Model.Point p in this.GetAroundPoints(point))
            {
                this.ProcessPoint(p, region);
            }
        }


        private void MergeRegions(Region region1, Region region2)
        {
            region1.Points.AddRange(region2.Points);

            //foreach(Point p in region2.Points)
            //{
            //    p.Parent = region1;
            //}
           region2.Points.ForEach(p => p.Parent = region1);
          
           region2 = null;
        }

        private bool isBorder(Color point)
        {
            if (point == Color.White)
            {
                return false;
            }

            return true;
        }

        private MapProject.Model.Point GetRandomPoint()
        {

            Random r = new Random();

            MapProject.Model.Point result;

            do
            {
                result = this._points[r.Next(0, this._points.GetLength(0)), r.Next(0, this._points.GetLength(1))];

            } while (!result.IsBorder);

            return result;

        }

        public IEnumerable<MapProject.Model.Point> GetAroundPoints(MapProject.Model.Point p)
        {
            List<MapProject.Model.Point> result = new List<MapProject.Model.Point>();

            if (p.X != 0)
            {
                result.Add(this._points[p.X - 1, p.Y]);
            }

            if (p.X != this._points.GetLength(0))
            {
                result.Add(this._points[p.X + 1, p.Y]);
            }

            if (p.Y != 0)
            {
                result.Add(this._points[p.X, p.Y - 1]);
            }

            if (p.Y != this._points.GetLength(0))
            {
                result.Add(this._points[p.X, p.Y + 1]);
            }

            return result;
        }
    }
}
