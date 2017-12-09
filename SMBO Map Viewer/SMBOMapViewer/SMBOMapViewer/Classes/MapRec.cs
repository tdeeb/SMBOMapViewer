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
        public string Name; //Max length defined as 60 in the VB6 source
        public short Revision; //VB6 integers are 16 bits and longs are 32 bits
        public byte Moral;
        public short Up;
        public short Down;
        public short Left;
        public short Right;
        public string music;
        public short BootMap;
        public byte BootX;
        public byte BootY;
        public short Shop;
        public byte Indoors;
        public TileRec[,] Tile = new TileRec[Constants.MAX_MAPX, Constants.MAX_MAPY];
        public short[] Npc = new short[15];
        public byte[] SpawnX = new byte[15];
        public byte[] SpawnY = new byte[15];
        public string owner;
        public byte scrolling;
        public short Weather;

        public void DrawMap()
        {
            //Draw tiles
            DrawTiles();
        }

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
            //Get all tile layers and draw them
            CroppedTexture2D ground = GetTileLayerData(Tile[x, y].Ground, Tile[x, y].GroundSet);
            CroppedTexture2D mask = GetTileLayerData(Tile[x, y].Mask, Tile[x, y].MaskSet);
            CroppedTexture2D mask2 = GetTileLayerData(Tile[x, y].Mask2, Tile[x, y].Mask2Set);
            CroppedTexture2D fringe = GetTileLayerData(Tile[x, y].Fringe, Tile[x, y].FringeSet);
            CroppedTexture2D fringe2 = GetTileLayerData(Tile[x, y].Fringe2, Tile[x, y].Fringe2Set);

            Vector2 renderPos = new Vector2(x * Constants.PIC_X, y * Constants.PIC_Y);

            SpriteRenderer.Instance.Draw(ground.Tex, renderPos, ground.SourceRect, .3f);
            SpriteRenderer.Instance.Draw(mask.Tex, renderPos, mask.SourceRect, .3f);
            SpriteRenderer.Instance.Draw(mask2.Tex, renderPos, mask2.SourceRect, .3f);
            SpriteRenderer.Instance.Draw(fringe.Tex, renderPos, fringe.SourceRect, .3f);
            SpriteRenderer.Instance.Draw(fringe2.Tex, renderPos, fringe2.SourceRect, .3f);
        }

        /// <summary>
        /// Gets layer data on a tile.
        /// </summary>
        /// <param name="tileLayer"></param>
        /// <param name="tileset">The tileset to get the tile from</param>
        /// <returns></returns>
        private CroppedTexture2D GetTileLayerData(int tileLayer, int tileset)
        {
            //Load the texture
            Texture2D tileSet = AssetManager.Instance.LoadRawTexture2D($"{Constants.MapTilesPath}Tiles{tileset}.bmp");

            //Set the draw region
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
