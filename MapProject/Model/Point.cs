using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Point
    {
        public string ParentId
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public bool IsBorder
        {
            get;
            set;
        }

        public Point(int x, int y, bool isBorder)
        {
            this.X = x;
            this.Y = y;
            this.IsBorder = isBorder;
        }

        public Point(int x, int y, string parentId, bool isBorder) : this(x, y, isBorder)
        {
            this.ParentId = parentId;
        }
    }
}
