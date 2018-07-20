using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBOMapViewer
{
    /// <summary>
    /// Stores constant values.
    /// </summary>
    public static class Constants
    {
        public const string ContentRoot = "Content";
        public const string MapTilesPath = "Content/Tiles/";
        public const string MapDataPath = "Content/MapData/";

        /// <summary>
        /// The TileType value for Roof tiles
        /// </summary>
        public const byte ROOF_TILE = 28;

        public const int MAX_MAPX = 30;
        public const int MAX_MAPY = 30;
        public const int PIC_X = 32;
        public const int PIC_Y = 32;
        public const int TILESINSHEETS = 14;

        public const int MIN_MAPNUM = 1;
        public const int MAX_MAPNUM = 350;

        /// <summary>
        /// How often map animations are updated.
        /// </summary>
        public const double MAP_ANIM_TIMER = 250d;

        public const string SCREENSHOT_FOLDER_NAME = "Screenshots";
    }
}
