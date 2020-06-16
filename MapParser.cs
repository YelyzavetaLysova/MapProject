using System;
using System.Collections.Generic;
using MapProject.Model;
using System.Linq;


namespace MapProject
{
    public class MapParser
    {

        private MapProject.Model.Point[,] _points;

        public Model.Region ParseImage(System.Drawing.Bitmap img)
        {
            this._points = new Point[img.Width, img.Height];

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    this._points[i, j] = new Point(i, j, this.isBorder(img.GetPixel(i, j)));
                }
            }
           
            Point p = this.GetRandomPoint();

            //region.Points.Add(p);

            this.ProcessPoint(p);

        }

        private void ProcessPoint(Point point, Region region = null)
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

            foreach (Point p in this.GetAroundPoints(point))
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

        private bool isBorder(System.Drawing.Color point)
        {
            if (point.Name == "White")
            {
                return false;
            }

            return true;
        }

        private Point GetRandomPoint()
        {

            Random r = new Random();

            Point result;

            do
            {
                result = this._points[r.Next(0, this._points.GetLength(0)), r.Next(0, this._points.GetLength(1))];

            } while (!result.IsBorder);

            return result;

        }

        public IEnumerable<Point> GetAroundPoints(Point p)
        {
            List<Point> result = new List<Point>();

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
