using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapProject
{
    public class MapProcessingSettings
    {
        public bool ParseBordersOnly
        {
            get;
            private set;
        }

        public Color BordersColor
        {
            get;
            set;
        }

        public MapProcessingSettings()
        {

        }

        public MapProcessingSettings(bool parseBordersOnly, Color bordersColor)
        {
            this.ParseBordersOnly = parseBordersOnly;
            this.BordersColor = bordersColor;
        }
    }
}
