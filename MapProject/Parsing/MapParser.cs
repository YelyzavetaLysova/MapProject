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

        private ILogger _logger;

        private string _pointsImagePath = Environment.CurrentDirectory + "/" + "debug_region_points.jpeg";
        private string _regionsImagePath = Environment.CurrentDirectory + "/" + "debug_regions.jpeg";
        private string _bordersImagePath = Environment.CurrentDirectory + "/" + "debug_region_borders.jpeg";

        public MapParser(ILogger logger)
        {
            this._logger = logger;

            this._logger.LogDebug("MapParser was initialized");
        }


        private MapProject.Model.Point[,] _points;


        public IEnumerable<Region> ParseImage(Image<Rgba32> img)
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

            this._logger.LogInformation("Saving points to image - " + this._pointsImagePath + "...");

            ImageHelper.SavePointsToImage(this._points, this._pointsImagePath);


            int counter = 0;

            List<Region> regions = new List<Region>();

            for(int i = 0; i < this._points.GetLength(0); i++)
            {
                for(int j = 0; j < this._points.GetLength(1); j++)
                {
                    counter++;

                    if (counter % 100 == 0)
                    {
                        this._logger.LogInformation($"{counter} / {_points.Length}");
                    }

                    if (String.IsNullOrWhiteSpace(this._points[i, j].ParentId) && this._points[i, j].IsBorder == false)
                    {
                        Region r = new Region();

                        this._logger.LogDebug("New region created: " + r.Id);

                        List <Model.Point> states = new List<Model.Point>() { this._points[i, j] };

                        do
                        { 
                            this.ProcessPoint(states.First(), r, states);
                        }
                        while (states.Count() != 0);

                        regions.Add(r);



                    }
                }
            }

            this._logger.LogInformation("Total regions count: " + regions.Count());
            this._logger.LogInformation("Big regions count: " + regions.Count((x => x.Points.Count() > 200)));
            this._logger.LogInformation("Removing small regions...");


            regions.RemoveAll(x => x.Points.Count() < 200);

            this._logger.LogInformation("Saving regions to image - " + this._regionsImagePath + "...");

            ImageHelper.SaveRegionsToImage(regions, this._regionsImagePath);

            

            this._logger.LogInformation("Saving region borders to image - " + _bordersImagePath + "...");

            ImageHelper.SaveRegionBordersToImage(regions, _bordersImagePath, 100);



            this._logger.LogInformation("Image processing finished");

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

        private void ProcessPoint(Model.Point current, Region region, List<Model.Point> states)
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
                    if (p.IsBorder == false && p.Processed == false)
                    {
                        if (!states.Contains(p))
                        {
                            states.Add(p);
                        }
                    }
                    else
                    {
                        p.ParentId = region.Id;
                        region.Points.Add(p);
                    }
                }

                
            } 

            states.Remove(current);

            current.Processed = true;



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

            return result;
        }
    }
}
