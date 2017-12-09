using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SMBOMapViewer
{
    /// <summary>
    /// Map Data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class MapRec
    {
        public string Name; //Max length defined as 60 in the VB6 source //The name of the map
        public short Revision; //VB6 integers are 16 bits and longs are 32 bits //The revision of the map; how many times it was edited
        public byte Moral; //0 for non-PvP, 1 for PvP
        public short Up; //Map number up from this map
        public short Down; //Map number down from this map
        public short Left; //Map number left from this map
        public short Right; //Map number right from this map
        public string music; //The name of the music track playing
        public short BootMap;
        public byte BootX;
        public byte BootY;
        public short Shop;
        public byte Indoors; //0 for outside, 1 for indoors
        public TileRec[,] Tile = new TileRec[Constants.MAX_MAPX, Constants.MAX_MAPY]; //Tile data
        public short[] Npc = new short[15]; //NPC data
        public byte[] SpawnX = new byte[15]; //NPC X position spawn data
        public byte[] SpawnY = new byte[15]; //NPC Y position spawn data
        public string owner;
        public byte scrolling;
        public short Weather;

        public void DrawMap()
        {
            //Draw tiles
            DrawTiles();
        }

        /// <summary>
        /// Draw all the tiles on the map.
        /// </summary>
        public void DrawTiles()
        {
            for (int i = 0; i < Tile.GetLength(0); i++)
            {
                for (int j = 0; j < Tile.GetLength(1); j++)
                {
                    DrawTile(i, j);
                }
            }
        }

        /// <summary>
        /// Draw an individual tile on the map.
        /// </summary>
        /// <param name="x">The X index of the tile.</param>
        /// <param name="y">The Y index of the tile.</param>
        public void DrawTile(int x, int y)
        {
            TileRec tile = Tile[x, y];

            //Get all tile layers and draw them
            CroppedTexture2D ground = GetTileLayerData(tile.Ground, tile.GroundSet);
            CroppedTexture2D mask = GetTileLayerData(tile.Mask, tile.MaskSet);
            CroppedTexture2D mask2 = GetTileLayerData(tile.Mask2, tile.Mask2Set);
            CroppedTexture2D fringe = GetTileLayerData(tile.Fringe, tile.FringeSet);

            Vector2 renderPos = new Vector2(x * Constants.PIC_X, y * Constants.PIC_Y);

            //Render the layers
            SpriteRenderer.Instance.Draw(ground.Tex, renderPos, ground.SourceRect);
            SpriteRenderer.Instance.Draw(mask.Tex, renderPos, mask.SourceRect);
            SpriteRenderer.Instance.Draw(mask2.Tex, renderPos, mask2.SourceRect);
            SpriteRenderer.Instance.Draw(fringe.Tex, renderPos, fringe.SourceRect);

            //Check if we should render tiles hidden by roofs or not
            if (MapControlSettings.ShowHiddenRoofTiles == false || tile.Type != Constants.ROOF_TILE)
            {
                CroppedTexture2D fringe2 = GetTileLayerData(tile.Fringe2, tile.Fringe2Set);
                SpriteRenderer.Instance.Draw(fringe2.Tex, renderPos, fringe2.SourceRect);
            }
        }

        /// <summary>
        /// Gets layer data on a tile.
        /// </summary>
        /// <param name="tileLayer">An integer representing layers such as Ground, Mask, Mask2, Fringe, or Fringe2.</param>
        /// <param name="tileset">The tileset to get the tile from</param>
        /// <returns></returns>
        private CroppedTexture2D GetTileLayerData(int tileLayer, int tileset)
        {
            //Load the texture
            Texture2D tileSet = AssetManager.Instance.LoadRawTexture2D($"{Constants.MapTilesPath}Tiles{tileset}.bmp");

            //Set the draw region; get the tile's region in the tile sheet
            Rectangle rect = Rectangle.Empty;
            rect.Y = (tileLayer / Constants.TILESINSHEETS) * Constants.PIC_Y;
            rect.Height = Constants.PIC_Y;
            rect.X = (tileLayer - (tileLayer / Constants.TILESINSHEETS) * Constants.TILESINSHEETS) * Constants.PIC_X;
            rect.Width = Constants.PIC_X;

            return new CroppedTexture2D(tileSet, rect);
        }

        public override string ToString()
        {
            return $"NAME: {Name}\n" +
                $"REVISION: {Revision}\n" +
                $"MORAL: {Moral}\n" +
                $"UP: {Up}\n" +
                $"DOWN: {Down}\n" +
                $"LEFT: {Left}\n" +
                $"RIGHT: {Right}\n" +
                $"MUSIC: {music}\n" +
                $"BOOTMAP: {BootMap}\n" +
                $"BOOTX: {BootX}\n" +
                $"BOOTY: {BootY}\n" +
                $"SHOP: {Shop}\n" +
                $"INDOORS: {Indoors}\n" +
                $"TILE: {Tile}\n" +
                $"NPC: {Npc}\n" +
                $"SPAWNX: {SpawnX}\n" +
                $"SPAWNY: {SpawnY}\n" +
                $"OWNER: {owner}\n" +
                $"SCROLLING: {scrolling}\n" +
                $"WEATHER: {Weather}\n";
        }
    }
}
