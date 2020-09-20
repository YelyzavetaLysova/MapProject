using System;
using System.Collections.Generic;
using MapProject.Model;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Microsoft.Extensions.Logging;
using MapProject.Helpers;

namespace MapProject.Parsing
{
    public class MapParser : IMapParser
    {

        private MapProject.Model.Point[,] _points;

        private HashSet<Model.Point> _processedPoints;

        private HashSet<Model.Point> _processedBorderPoints;
        public IEnumerable<Region> ParseImage(Image<Rgba32> img)
        {

            this._points = new MapProject.Model.Point[img.Width, img.Height];
            this._processedPoints = new HashSet<Model.Point>();
            this._processedBorderPoints = new HashSet<Model.Point>();

            for (int y = 0; y < img.Height; y++)
            {
                Span<Rgba32> pixelsRow = img.GetPixelRowSpan(y);

                for (int x = 0; x < img.Width; x++)
                {
                    this._points[x, y] = new MapProject.Model.Point(x, y, this.isBorder(new Color(pixelsRow[x])));
                }
            }

            ImageHelper.SavePointsToImage(this._points);


            int counter = 0;

            List<Region> regions = new List<Region>();

            for (int i = 0; i < this._points.GetLength(0); i++)
            {
                for (int j = 0; j < this._points.GetLength(1); j++)
                {
                    counter++;

                    if(counter % 10000 == 0) Console.WriteLine(counter);

                    var point = this._points[i, j];

                    if (/*String.IsNullOrWhiteSpace(point.ParentId) && */point.IsBorder == false && !this.IsProcessed(point))
                    {
                        Region r = new Region();

                        List<Model.Point> states = new List<Model.Point>() { point };
                        List<Model.Point> borders = new List<Model.Point>();

                        do
                        {
                            this.ProcessPoint(states.First(), r, states, borders);
                        }
                        while (states.Count() != 0);

                        foreach(var borderPoint in borders)
                        {
                            borderPoint.IsBorder = true;
                        }

                        regions.Add(r);



                    }
                }
            }        

            regions.RemoveAll(x => x.Points.Count() < 200);

            ImageHelper.SaveRegionsToImage(regions);



            ImageHelper.SaveRegionBordersToImage(regions, 100);

            foreach (var region in regions)
            {
                List<Contur> sealedConturs = this.FindSealedConturs(region).ToList();
                sealedConturs.RemoveAll(x => x.PointsList.Count() < 3);
                int i = 0;
                Console.WriteLine("I am working...");

                foreach (var contur in sealedConturs)
                {
                    ImageHelper.SavePointsToImage(contur.PointsList);
                }

            }

            



            return regions;
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



          private bool IsProcessed(Model.Point point)
        {
            return this._processedPoints.Contains(point);
        }

        private void ProcessPoint(Model.Point current, Region region, List<Model.Point> pointsToProcess, List<Model.Point> borders)
        {
            if (!current.IsBorder)
            {
                if (String.IsNullOrWhiteSpace(current.ParentId))
                {
                    current.ParentId = region.Id;
                    region.Points.Add(current);
                }

                foreach (MapProject.Model.Point p in this.GetAroundPoints(current))
                {

                    if (p.IsBorder == false && !this.IsProcessed(p))
                    {
                        if (!pointsToProcess.Contains(p))
                        {
                            pointsToProcess.Add(p);
                        }
                    }
                    if (p.IsBorder == true && !borders.Contains(current))
                    {
                        borders.Add(current);
                    }

                }

                
            }

            this._processedPoints.Add(current);
            pointsToProcess.Remove(current);
        }

        private bool isBorder(Color point)
        {
            if (point == Color.White)
            {
                return false;
            }

            return true;
        }
        
        private IEnumerable<MapProject.Model.Point> GetAroundPoints(MapProject.Model.Point p)
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

            if (p.X != 0 && p.Y != 0)
            {
                result.Add(this._points[p.X - 1, p.Y - 1]);
            }

            if (p.X != this._points.GetLength(0) - 1 && p.Y != 0)
            {
                result.Add(this._points[p.X + 1, p.Y - 1]);
            }

            if (p.Y != this._points.GetLength(1) - 1 && p.X != 0)
            {
                result.Add(this._points[p.X - 1, p.Y + 1]);
            }

            if (p.Y != this._points.GetLength(1) - 1 && p.X != this._points.GetLength(0) - 1)
            {
                result.Add(this._points[p.X + 1, p.Y + 1]);
            }

            return result;
        }




        class Contur
        {
            public List<Model.Point> PointsList
            {
                private set;
                get;
            }

            public Contur()
            {
                this.PointsList = new List<Model.Point>();
            }
        }

        IEnumerable<Model.Point> GetAroundBorderPoints(Model.Point point, Model.Region region)
        {
            //List<Model.Point> result = new List<Model.Point>();

            var result = region.Points.Where(x => x.IsBorder).Where(x =>
            {
                if ((point.X == x.X && point.Y == x.Y + 1) || (point.X == x.X + 1 && point.Y == x.Y) || (point.X == x.X - 1 && point.Y == x.Y) || (point.X == x.X && point.Y == x.Y - 1)
                || (point.X == x.X + 1 && point.Y == x.Y + 1) || (point.X == x.X + 1 && point.Y == x.Y - 1) || (point.X == x.X - 1 && point.Y == x.Y - 1) || (point.X == x.X - 1 && point.Y == x.Y + 1))
                {
                    return true;
                }
                return false;
            });
            return result;
        }

        IEnumerable<Contur> FindSealedConturs(Model.Region region)
        {
            List<Contur> result = new List<Contur>();



            foreach (var point in region.Points.Where(x => x.IsBorder))
            {
                Contur contur = new Contur();
                //contur.PointsList.Add(point);
                this.ProcessBorderPoints(point, contur, region);

                if (contur.PointsList.Count() != 0)
                {
                    result.Add(contur);
                }
            }

            return result;
        }

        private void ProcessBorderPoints(Model.Point point, Contur contur, Model.Region region)
        {

            if (!this._processedBorderPoints.Contains(point))
            {

                contur.PointsList.Add(point);

                _processedBorderPoints.Add(point);


                var points = this.GetAroundBorderPoints(point, region);

                if (points.All(x => this._processedBorderPoints.Contains(x)))
                {
                    return;
                }

                foreach (var nextPoint in points.Where(x => !this._processedBorderPoints.Contains(x)))
                {
                    this.ProcessBorderPoints(nextPoint, contur, region);
                }
            }
        }
    }
}
