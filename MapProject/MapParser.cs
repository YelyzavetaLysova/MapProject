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
                            pixelsRow[x] = Color.Red;
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


            int counter = 0;

            List<Region> regions = new List<Region>();

            for(int i = 0; i < this._points.GetLength(0); i++)
            {
                for(int j = 0; j < this._points.GetLength(1); j++)
                {
                    counter++;

                    Console.WriteLine($"{counter} / {_points.Length}");

                    if (this._points[i, j].Parent == null && this._points[i, j].IsBorder == false)
                    {
                        Region r = new Region();

                        List<Model.Point> states = new List<Model.Point>() { this._points[i, j] };

                       
                        do
                        {
                            this.ProcessPoint(states.First(), r, states);
                        }
                        while (states.Count() != 0);

                        regions.Add(r);

                    }
                }
            }




            Console.WriteLine("Total regions count: " + regions.Count());
            Console.WriteLine("Big regions count: " + regions.Count((x => x.Points.Count() > 200)));


            using (var imageToSave = new Image<Rgba32>(this._points.GetLength(0), this._points.GetLength(1)))
            {
                Random r = new Random();

                foreach (var region in regions.Where(x => x.Points.Count() > 200))
                {

                    byte red = Convert.ToByte(r.Next(0, 256));
                    byte green = Convert.ToByte(r.Next(0, 256));
                    byte blue = Convert.ToByte(r.Next(0, 256));

                    Console.WriteLine("(" + red + ", " + green + ", " + blue + ")");


                    foreach (var point in region.Points)
                    {
                        int y = point.Y;
                        int x = point.X;

                        Span<Rgba32> pixelsRow = imageToSave.GetPixelRowSpan(y);

                        pixelsRow[x] = Color.FromRgb(red, green, blue);

                    }
                }

                

                using (Stream s = new FileStream(Environment.CurrentDirectory + "/regions.jpeg", FileMode.Create))
                {
                    imageToSave.SaveAsJpeg(s);

                }
            }

            return new Region();
        }



        public void AddPointsToRegion(Model.Point point, Region region)
        {
            if (!point.IsBorder)
            {
                region.AddPoint(point);
                point.Parent = region;
            }
        }

        
        private class AvoidRecursionModel
        {
            public MapProject.Model.Point Point
            {
                get;
                private set;
            }
            public MapProject.Model.Point PreviuosPoint
            {
                get;
                private set;
            }
            public Region Region
            {
                get;
                private set;
            }

            public AvoidRecursionModel(MapProject.Model.Point point, MapProject.Model.Point previuosPoint, Region region)
            {
                this.Point = point;
                this.PreviuosPoint = previuosPoint;
                this.Region = region;
            }
        }

        int iteration = 0;
        private void ProcessPoint(Model.Point current, Region region, List<Model.Point> states)
        {


            //Console.WriteLine("(" + current.X + " " + current.Y + ")");
            
            

            if (!current.IsBorder)
            {
                if (current.Parent == null)
                {
                    current.Parent = region;
                    region.Points.Add(current);
                }

                foreach (MapProject.Model.Point p in this.GetAroundPoints(current).Where(x => !x.IsBorder && x.Processed == false))
                {
                    if (!states.Contains(p))
                    {
                        states.Add(p);
                    }
                }
            }

            states.Remove(current);

            current.Processed = true;



        }

        private void MergeRegions(Region region1, Region region2)
        {
           region1.Points.AddRange(region2.Points);

           region2.Points.ForEach(p => p.Parent = region1);
          
           region2 = null;
        }

        private bool isBorder(Color point)
        {
            if (point == Color.White)
            {
                //Color.FromRgb(80, 122, 172)
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

            if (p.X != this._points.GetLength(0) - 1)
            {
                result.Add(this._points[p.X + 1, p.Y]);
            }

            if (p.Y != 0)
            {
                result.Add(this._points[p.X, p.Y - 1]);
            }

            if (p.Y != this._points.GetLength(1) - 1)
            {
                result.Add(this._points[p.X, p.Y + 1]);
            }

            return result;
        }
    }
}
