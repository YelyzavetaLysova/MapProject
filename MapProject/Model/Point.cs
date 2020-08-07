using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject.Model
{
    public class Point
    {
        //private Guid _id = Guid.Empty;
        public Guid Id
        {
            get;
            private set;
        }

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

        public bool Processed
        {
            get;
            set;
        }

        public Point(int x, int y, bool isBorder)
        {
            this.X = x;
            this.Y = y;
            this.IsBorder = isBorder;
            this.Processed = false;

            this.Id = Guid.NewGuid();
        }
    }
}
