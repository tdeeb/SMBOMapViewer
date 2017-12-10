using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBOMapViewer
{
    /// <summary>
    /// Holds global settings for viewing maps.
    /// </summary>
    public class MapControlSettings
    {
        /// <summary>
        /// Whether to show tiles hidden by roofs or not.
        /// </summary>
        public static bool ShowHiddenRoofTiles = false;

        /// <summary>
        /// Whether to render the map's animation tiles or the mask tiles.
        /// </summary>
        public static bool RenderAnimTiles = false;
    }
}
