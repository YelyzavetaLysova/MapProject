using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Point
    {
        public Region Parent
        {
            get;
            set;
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public bool IsBorder
        {
            get;
            private set;
        }

        public Point(int x, int y, bool isBorder)
        {
            this.X = x;
            this.Y = y;
            this.IsBorder = isBorder;
        }
    }
}
