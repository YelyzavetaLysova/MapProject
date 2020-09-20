using MapProject.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MapProject.Helpers
{
    public static class ImageHelper
    {

        private static string _pointsImagePath = Environment.CurrentDirectory + "/" + "debug_region_points.jpeg";
        private static string _regionsImagePath = Environment.CurrentDirectory + "/" + "debug_regions.jpeg";
        private static string _bordersImagePath = Environment.CurrentDirectory + "/" + "debug_region_borders.jpeg";

        public static void SaveRegionBordersToImage(IEnumerable<Region> regions, int sampling)
        {
            if (sampling < 0)
            {
                sampling = 0;
            }
            if (sampling > 100)
            {
                sampling = 100;
            }

            sampling = 100 / sampling;

            List<Model.Point> points = new List<Model.Point>();

            foreach (var collection in regions.Select(x => x.Points))
            {
                points.AddRange(collection);
            }

            var maxX = points.Max(x => x.X + 1);
            var maxY = points.Max(x => x.Y + 1);

            using (var imageToSave = new Image<Rgba32>(maxX, maxY))
            {
                Random r = new Random();

                foreach (var region in regions)
                {

                    byte red = Convert.ToByte(r.Next(0, 256));
                    byte green = Convert.ToByte(r.Next(0, 256));
                    byte blue = Convert.ToByte(r.Next(0, 256));

                    int i = 0;

                    foreach (var point in region.Points.Where(x => x.IsBorder))
                    {
                        i++;

                        if (i % sampling == 0)
                        {
                            Span<Rgba32> pixelsRow = imageToSave.GetPixelRowSpan(point.Y);
                            pixelsRow[point.X] = Color.FromRgb(red, green, blue);
                        }
                    }
                }



                using (Stream s = new FileStream(_bordersImagePath, FileMode.Create))
                {
                    imageToSave.SaveAsJpeg(s);

                }
            }
        }
        public static void SavePointsToImage(Model.Point[,] points)
        {

            using (var imageToSave = new Image<Rgba32>(points.GetLength(0), points.GetLength(1)))
            {
                for (int y = 0; y < points.GetLength(1); y++)
                {
                    Span<Rgba32> pixelsRow = imageToSave.GetPixelRowSpan(y);

                    for (int x = 0; x < points.GetLength(0); x++)
                    {

                        if (points[x, y].IsBorder)
                        {
                            pixelsRow[x] = Color.Red;
                        }
                        else
                        {
                            pixelsRow[x] = Color.White;
                        }

                    }
                }

                using (Stream s = new FileStream(_pointsImagePath, FileMode.Create))
                {
                    imageToSave.SaveAsJpeg(s);

                }
            }
        }
        public static void SaveRegionsToImage(IEnumerable<Region> regions)
        {
            List<Model.Point> points = new List<Model.Point>();

            foreach (var collection in regions.Select(x => x.Points))
            {
                points.AddRange(collection);
            }

            var maxX = points.Max(x => x.X + 1);
            var maxY = points.Max(x => x.Y + 1);

            using (var imageToSave = new Image<Rgba32>(maxX, maxY))
            {
                Random r = new Random();

                foreach (var region in regions.Where(x => x.Points.Count() > 200))
                {

                    byte red = Convert.ToByte(r.Next(0, 256));
                    byte green = Convert.ToByte(r.Next(0, 256));
                    byte blue = Convert.ToByte(r.Next(0, 256));

                    foreach (var point in region.Points)
                    {
                        int y = point.Y;
                        int x = point.X;

                        Span<Rgba32> pixelsRow = imageToSave.GetPixelRowSpan(y);

                        pixelsRow[x] = Color.FromRgb(red, green, blue);

                    }
                }



                using (Stream s = new FileStream(_regionsImagePath, FileMode.Create))
                {
                    imageToSave.SaveAsJpeg(s);

                }
            }
        }

        public static void SavePointsToImage(IEnumerable<Model.Point> points)
        {
            //List<Model.Point> points = new List<Model.Point>();

            //foreach (var collection in regions.Select(x => x.Points))
            //{
            //    points.AddRange(collection);
            //}

            var maxX = points.Max(x => x.X + 1);
            var maxY = points.Max(x => x.Y + 1);

            using (var imageToSave = new Image<Rgba32>(maxX, maxY))
            {
                Random r = new Random();


                foreach (var point in points)
                {
                    int y = point.Y;
                    int x = point.X;

                    Span<Rgba32> pixelsRow = imageToSave.GetPixelRowSpan(y);

                    pixelsRow[x] = Color.White;
                }


                using (Stream s = new FileStream(Environment.CurrentDirectory + "/" + Guid.NewGuid() + ".png", FileMode.Create))
                {
                    imageToSave.SaveAsJpeg(s);

                }
            }
        }
    }
}
