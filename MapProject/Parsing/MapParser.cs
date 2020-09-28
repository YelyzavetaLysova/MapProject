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
                    
                    this._points[x, y] = new MapProject.Model.Point(x, y, (x == 0 || y == 0 || x == img.Width - 1 || y == img.Height - 1) ? true : this.isBorder(new Color(pixelsRow[x])));
                    
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

            Console.WriteLine("Borders pre-processing started...");

            int ii = 0;

            for(int i = 0; i<regions.Count(); i++)
            {
                var region = regions[i];

                Console.WriteLine("Pre-processing region " + ++ii + "/" + regions.Count());
                var pointsToRemove = new List<Model.Point>();

                var points = region.Points.Where(x => x.IsBorder).ToArray();

                for(int j = 0; j < points.Length; j++)
                {
                    var point = points[j];

                    var aroundBorderPoints = this.GetAroundBorderPoints(point, region);
                    var aroundInternalPoints = this.GetAroundPoints(point, region);
                    var allAroundPoints = this.GetAroundPoints(point);
                    
                    if (aroundInternalPoints.Count(x => x.IsBorder == false) == 0)
                    {
                        pointsToRemove.Add(point);
                        point.ParentId = null;

                        continue;
                    }

                    if (aroundBorderPoints.Count() == 1)
                    {
                        pointsToRemove.Add(point);
                        point.ParentId = null;
                        continue;
                    }

                    //if (allAroundPoints.Where(x => x.IsBorder == false).All(x => region.Points.Contains(x)))
                    //{
                    //    pointsToRemove.Add(point);
                    //    continue;
                    //}
                }

                

                foreach (var pointToRemove in pointsToRemove)
                {
                    region.Points.Remove(pointToRemove);

                    //if (region.Points.Any(x => x.X == pointToRemove.X && x.Y == pointToRemove.Y))
                    //{
                    //    region.Points.RemoveAll(x => x.X == pointToRemove.X && x.Y == pointToRemove.Y);
                    //}

                   
                }
            }

            Console.WriteLine("Borders pre-processing finished");

            foreach (var region in regions)
            {
                List<Contur> sealedConturs = this.FindSealedConturs(region).ToList();
                sealedConturs.RemoveAll(x => x.PointsList.Count() < 3);
                //int i = 0;
                Console.WriteLine("I am workingre...");

                foreach (var contur in sealedConturs)
                {
                    ImageHelper.SavePointsToImage(contur.PointsList);
                }

                region.Points.Clear();

                region.Points.AddRange(sealedConturs.OrderByDescending(x => x.PointsList.Count).First().PointsList);

            }

            Console.WriteLine("count2: " + this.count2);

            Console.WriteLine("Done.");





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
        
        private IEnumerable<MapProject.Model.Point> GetAroundPoints(MapProject.Model.Point point, Region region = null, bool directOnly = false)
        {
            List<MapProject.Model.Point> result = new List<MapProject.Model.Point>();

            //if (region == null)
            //{

                if (point.X != 0)
                {
                    result.Add(this._points[point.X - 1, point.Y]);
                }

                if (point.X != this._points.GetLength(0) - 1)
                {
                    result.Add(this._points[point.X + 1, point.Y]);
                }

                if (point.Y != 0)
                {
                    result.Add(this._points[point.X, point.Y - 1]);
                }

                if (point.Y != this._points.GetLength(1) - 1)
                {
                    result.Add(this._points[point.X, point.Y + 1]);
                }

            if (!directOnly)
            {

                if (point.X != 0 && point.Y != 0)
                {
                    result.Add(this._points[point.X - 1, point.Y - 1]);
                }

                if (point.X != this._points.GetLength(0) - 1 && point.Y != 0)
                {
                    result.Add(this._points[point.X + 1, point.Y - 1]);
                }

                if (point.Y != this._points.GetLength(1) - 1 && point.X != 0)
                {
                    result.Add(this._points[point.X - 1, point.Y + 1]);
                }

                if (point.Y != this._points.GetLength(1) - 1 && point.X != this._points.GetLength(0) - 1)
                {
                    result.Add(this._points[point.X + 1, point.Y + 1]);
                }

            }
        
            if (region != null)
            {
                return result.Where(x => x.ParentId == region.Id).ToList();
            }

            return result;
        }




        private class Contur
        {
            public HashSet<Model.Point> PointsList
            {
                get;
                private set;
            }

            public Contur()
            {
                this.PointsList = new HashSet<Model.Point>();
            }

            public Contur CopyContur()
            {
                Contur result = new Contur();

                foreach(var element in this.PointsList)
                {
                    result.PointsList.Add(element);
                }

                return result;
            }
        }

        IEnumerable<Model.Point> GetAroundBorderPoints(Model.Point point, Model.Region region, bool directOnly = false)
        {
            return this.GetAroundPoints(point, region, directOnly).Where(x => x.IsBorder);
        }

        //IEnumerable<Model.Point> GetAroundBorderPoints(Model.Point point, Model.Region region)
        //{
        //    List<MapProject.Model.Point> result = new List<MapProject.Model.Point>();

        //    if (point.X != 0)
        //    {
        //        result.Add(this._points[point.X - 1, point.Y]);
        //    }

        //    if (point.X != this._points.GetLength(0) - 1)
        //    {
        //        result.Add(this._points[point.X + 1, point.Y]);
        //    }

        //    if (point.Y != 0)
        //    {
        //        result.Add(this._points[point.X, point.Y - 1]);
        //    }

        //    if (point.Y != this._points.GetLength(1) - 1)
        //    {
        //        result.Add(this._points[point.X, point.Y + 1]);
        //    }

        //    if (point.X != 0 && point.Y != 0)
        //    {
        //        result.Add(this._points[point.X - 1, point.Y - 1]);
        //    }

        //    if (point.X != this._points.GetLength(0) - 1 && point.Y != 0)
        //    {
        //        result.Add(this._points[point.X + 1, point.Y - 1]);
        //    }

        //    if (point.Y != this._points.GetLength(1) - 1 && point.X != 0)
        //    {
        //        result.Add(this._points[point.X - 1, point.Y + 1]);
        //    }

        //    if (point.Y != this._points.GetLength(1) - 1 && point.X != this._points.GetLength(0) - 1)
        //    {
        //        result.Add(this._points[point.X + 1, point.Y + 1]);
        //    }

        //    if (region != null)
        //    {
        //        return result.Where(x => x.IsBorder == true && x.ParentId == region.Id).ToList();
        //    }

        //    return result;
        //}

        IEnumerable<Contur> FindSealedConturs(Model.Region region)
        {
            List<Contur> result = new List<Contur>();

            foreach (var point in region.Points.Where(x => x.IsBorder))
            {

                List<Contur> conturs = new List<Contur>();

                Contur contur = new Contur();
                //contur.PointsList.Add(point);

                if (this.GetAroundBorderPoints(point, region, true).Count() == 2)
                {
                    //if (!conturs.Any(x => x.PointsList.Contains(point)))
                    //{
                        this.ProcessBorderPoints(point, contur, region, /*conturs,*/ true);
                    //}
                }

                //if (conturs.Count != 0)
                //{
                //    result.Add(conturs.OrderByDescending(x => x.PointsList.Count()).First());
                //}

                if (contur.PointsList.Count() != 0)
                {
                    result.Add(contur);

                }
            }

            

            return result;
        }


        private int count2 = 0;

        private void ProcessBorderPoints(Model.Point point, Contur contur, Model.Region region, bool first = false)
        {


            if (!this._processedBorderPoints.Contains(point))
            {

                contur.PointsList.Add(point);

                _processedBorderPoints.Add(point);


                var nextPoints = this.GetAroundBorderPoints(point, region, true);

                if (first)
                {
                    nextPoints = new List<Model.Point> { nextPoints.First() };
                }

                if (nextPoints.All(x => this._processedBorderPoints.Contains(x)))
                {
                    return;
                }

                if (nextPoints.Count() > 2)
                {
                    count2++;
                }

                foreach (var nextPoint in nextPoints.Where(x => !this._processedBorderPoints.Contains(x)))
                {
                    this.ProcessBorderPoints(nextPoint, contur, region);
                }
            }


        }

        //private void ProcessBorderPoints(Model.Point point, Contur contur, Model.Region region, List<Contur> conturs, bool first = false)
        //{

        //    if (!conturs.Contains(contur))
        //    {
        //        conturs.Add(contur);
        //    }

        //    if (!contur.PointsList.Contains(point))
        //    {
        //        contur.PointsList.Add(point);

        //        IEnumerable<Model.Point> nextPoints = this.GetAroundBorderPoints(point, region).Where(x => !contur.PointsList.Contains(x)).ToList();

        //        if (nextPoints.Count() == 0)
        //        {
        //            return;
        //        }

        //        if (first)
        //        {
        //            nextPoints = new List<Model.Point> { nextPoints.First() };
        //        }

        //        if (nextPoints.Any())
        //        {
        //            if (nextPoints.Count() > 1)
        //            {
        //                foreach (var nextPoint in nextPoints)
        //                {
        //                    Contur newContur = contur.CopyContur();

        //                    this.ProcessBorderPoints(nextPoint, newContur, region, conturs);
        //                }
        //            }
        //            else
        //            {
        //                this.ProcessBorderPoints(nextPoints.First(), contur, region, conturs);
        //            }


        //        }
        //    }
        //}

        //private bool CheckInternalIntersection(Model.Point currentPoint, Model.Point nextPoint, Region region)
        //{
        //    var currentInternalPoints = new HashSet<Model.Point>(this.GetAroundPoints(currentPoint, region).Where(x => !x.IsBorder));

        //    var nextInternalPoints = new HashSet<Model.Point>((this.GetAroundPoints(nextPoint, region).Where(x => !x.IsBorder)));

        //    return currentInternalPoints.Overlaps(nextInternalPoints);
        //}

        //private bool CheckExternalIntersection(Model.Point currentPoint, Model.Point nextPoint, Region region)
        //{
        //    var currentExternalPoints = new HashSet<Model.Point>(this.GetAroundPoints(currentPoint).Where(x => !region.Points.Contains(x)));

        //    var nextExternalPoints = new HashSet<Model.Point>(this.GetAroundPoints(nextPoint).Where(x => !region.Points.Contains(x)));

        //    return currentExternalPoints.Overlaps(nextExternalPoints);
        //}

        //private void ProcessBorderPoints(Model.Point point, Contur contur, Model.Region region)
        //{

        //    if (!this._processedBorderPoints.Contains(point))
        //    {

        //        contur.PointsList.Add(point);

        //        _processedBorderPoints.Add(point);

        //        //1. Выбрать все окружающие точки с общими внутренними и внешними точками



        //        //var nextInter


        //        var points = this.GetAroundBorderPoints(point, region);

        //        if (points.All(x => this._processedBorderPoints.Contains(x)))
        //        {
        //            return;
        //        }



        //        //var test = points.Where(x => this.CheckInternalIntersection(point, x, region));

        //        bool pointProcessed = false;

        //        foreach (var nextPoint in points)
        //        {
        //            var externalPoints = this.GetAroundPoints(nextPoint);

        //            if (!pointProcessed && externalPoints.Any(x => ! region.Points.Contains(x)))
        //            {
        //                pointProcessed = true;

        //                this.ProcessBorderPoints(nextPoint, contur, region);
        //            }
        //            else
        //            {
        //                this._processedBorderPoints.Add(nextPoint);
        //            }

        //        }
        //    }
        //}


        //private void ProcessBorderPoints(Model.Point point, Contur contur, Model.Region region)
        //{

        //    if (!this._processedBorderPoints.Contains(point))
        //    {

        //        contur.PointsList.Add(point);

        //        _processedBorderPoints.Add(point);


        //        var points = this.GetAroundBorderPoints(point, region);

        //        if (points.All(x => this._processedBorderPoints.Contains(x)))
        //        {
        //            return;
        //        }

        //        foreach (var nextPoint in points.Where(x => !this._processedBorderPoints.Contains(x)))
        //        {
        //            this.ProcessBorderPoints(nextPoint, contur, region);
        //        }
        //    }
        //}
    }
}
