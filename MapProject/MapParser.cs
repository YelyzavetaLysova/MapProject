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

            //MapProject.Model.Point p = this.GetRandomPoint();


            int counter = 0;

            List<Region> regions = new List<Region>();

            foreach(MapProject.Model.Point p in this._points)
            {
                counter++;

                if (p.Parent == null)
                {
                    List<AvoidRecursionModel> states = new List<AvoidRecursionModel>() { new AvoidRecursionModel(p, null, null) };

                    while (states.Count() != 0) 
                    {
                        Region r = new Region();
                        this.ProcessPoint(p, r, states);

                        regions.Add(r);
                    }

                    states.Clear();
                }
            }

            //while (MapProject.Model.Point p in this.GetAroundPoints(point).Where(x => x.Parent == null))
            //{
            //    if (point.IsBorder == false)
            //    {
            //        if (point.Parent == null)
            //        {
            //            if (previuosPoint != null && previuosPoint.IsBorder == true)
            //            {
            //                region = new Region();
            //            }

            //            point.Parent = region;
            //            region.Points.Add(point);
            //        }
            //        else
            //        {
            //            if (point.Parent != region)
            //            {
            //                this.MergeRegions(region, point.Parent);
            //            }
            //        }
            //    }
            //}


            //region.Points.Add(p);

            //var recursionCount = 0;

           

            //Region first = new Region();


            

            //int counter = 0;

            //while (counter < this._points.Length)
            //{
            //    List<AvoidRecursionModel> states1 = new List<AvoidRecursionModel>();

            //    states1.AddRange(states);

            //    foreach (var state in states)
            //    {
            //        this.ProcessPoint(state.Point, state.Region, states1, ref counter, state.PreviuosPoint);
            //    }

            //    states.Clear();
            //    states.AddRange(states1);
            //}

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


        //private IEnumerable<MapProject.Model.Point> GetPointsNoRecursion(MapProject.Model.Point p)
        //{
        //    List<MapProject.Model.Point> result = new List<MapProject.Model.Point>();

        //    if (p.X != 0)
        //    {
        //        result.Add(this._points[p.X - 1, p.Y]);
        //    }

        //    if (p.X != this._points.GetLength(0))
        //    {
        //        result.Add(this._points[p.X + 1, p.Y]);
        //    }

        //    if (p.Y != 0)
        //    {
        //        result.Add(this._points[p.X, p.Y - 1]);
        //    }

        //    if (p.Y != this._points.GetLength(0))
        //    {
        //        result.Add(this._points[p.X, p.Y + 1]);
        //    }

        //    return result.Where(x => x.Parent == null);

        //}

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


        //private void MYProcessPoint(Model.Point point, Region region)
        //{
        //    foreach(var p in this._points)
        //    {
        //        if(p.IsBorder != true)
        //        {
        //            point.Parent = region;
        //            region.Points.Add(point);

        //        }
        //    }
        //}

        private void ProcessPoint(MapProject.Model.Point point, Region region, List<AvoidRecursionModel> list)
        {

            list.Clear();

            //var aroundPoints = this.GetAroundPoints(point);

            if (!point.IsBorder)
            {
                //counter++;

                if (point.Parent == null)
                {
                    point.Parent = region;
                    region.Points.Add(point);

                   
                }

                point.Processed = true;
            }

            foreach (MapProject.Model.Point p in this.GetAroundPoints(point).Where(x => !x.IsBorder && x.Processed == false))
            {
                AvoidRecursionModel state = new AvoidRecursionModel(p, point, region);

                list.Add(state);
            }

        }

        //private void ProcessPoint(MapProject.Model.Point point, Region region = null)
        //{
        //    if (point.IsBorder == false && point.Parent == null)
        //    {
               
        //    }


        //    if (region == null)
        //    {
        //        region = new Region();
        //    }
            
        //    if (point.Parent == region)
        //    {
        //        return;
        //    }

        //    if (point.Parent != null)
        //    {
        //        this.MergeRegions(region, point.Parent);

        //        return;
        //    }
        //    else
        //    {
        //        if (!point.IsBorder)
        //        {
        //            region.Points.Add(point);
        //            point.Parent = region;
        //        }
        //        else
        //        {
        //            region = null;
        //        }
        //    }


        //    var pointsToProcess = this.GetAroundPoints(point);

        //    if (pointsToProcess.All(x => x.Parent != null))
        //    {
        //        return;
        //    }

        //    foreach (MapProject.Model.Point p in pointsToProcess)
        //    {
        //        this.ProcessPoint(p, region);
        //    }
        //}


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
