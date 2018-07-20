using System;
using System.IO;
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
    /// Loads maps.
    /// </summary>
    public static class MapLoader
    {
        /// <summary>
        /// Loads a map by map number.
        /// </summary>
        /// <param name="MapNum"></param>
        /// <returns></returns>
        public static MapRec LoadMap(int MapNum)
        {
            /* Original VB6 Code *
             * Dim FileName As String
              Dim F As Long

              FileName = App.Path & "\maps\map" & MapNum & ".dat"

              If FileExists("maps\map" & MapNum & ".dat") = False Then
                  Exit Sub
              End If
              F = FreeFile
              Open FileName For Binary As #F
              Get #F, , Map(MapNum)
              Close #F
             * */

            //VB6's "Put" function writes strings as ANSI using the system default code page

            string fileName = Path.GetFullPath($"{Constants.MapDataPath}map{MapNum}.dat");
            
            if (File.Exists(fileName) == false)
            {
                Console.WriteLine($"Cannot find map path: {fileName}");
                return null;
            }

            BinaryReader binaryReader = null;

            try
            {
                MapRec mapRec = new MapRec();

                binaryReader = new BinaryReader(new FileStream(fileName, FileMode.Open), Encoding.ASCII);

                //Read in all data
                mapRec.Name = new string(binaryReader.ReadChars(60));
                mapRec.Revision = binaryReader.ReadInt16();
                mapRec.Moral = binaryReader.ReadByte();
                mapRec.Up = binaryReader.ReadInt16();
                mapRec.Down = binaryReader.ReadInt16();
                mapRec.Left = binaryReader.ReadInt16();
                mapRec.Right = binaryReader.ReadInt16();

                //The first two bytes contain the length of the string
                //The second byte will always be 0 since we don't have names longer than 256 characters in SMBO
                int length = binaryReader.ReadByte();
                binaryReader.ReadByte();
                if (length == 0)
                {
                    mapRec.music = string.Empty;
                }
                else mapRec.music = new string(binaryReader.ReadChars(length));

                mapRec.BootMap = binaryReader.ReadInt16();
                mapRec.BootX = binaryReader.ReadByte();
                mapRec.BootY = binaryReader.ReadByte();
                mapRec.Shop = binaryReader.ReadInt16();
                mapRec.Indoors = binaryReader.ReadByte();

                //The first two bytes of the multi-dimensional array are reserved for the rank, with the first byte telling it
                binaryReader.ReadBytes(2);

                //The next 8 bytes contain the length of the first rank, and the following 8 bytes contain the length of the second rank
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();

                //We're now at the tile data
                for (int i = 0; i < mapRec.Tile.GetLength(0); i++)
                {
                    for (int j = 0; j < mapRec.Tile.GetLength(1); j++)
                    {
                        //Read in differently; the map comes in a different form
                        ReadTileData(ref mapRec.Tile[j, i], binaryReader);
                    }

                    if (i != (mapRec.Tile.GetLength(0) - 1))
                    {
                        //Offset by a tile (not entirely sure why this is needed...but this makes it work)
                        TileRec t = new TileRec();
                        ReadTileData(ref t, binaryReader);
                    }
                }

                /* Reading the rest of the data seemingly doesn't work, but we won't need it anyway unless we decide to show NPCs
                   For Map 7, the VB methods return the following for the NPC array:
                   2,2,2,0,0,0,0,0,0,0,0,0,0,0,0
                   as the NPC data; I could not find an address with a value of 2 anywhere after the tile data (offset EFA9 for Map 7)
                   in a hex editor, so something else must be going on */

                //VB6 arrays contain 20 bytes of memory, plus 8 bytes per dimension, plus the memory required to store the data.

                return mapRec;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
            finally
            {
                //Close the file guaranteed
                if (binaryReader != null)
                    binaryReader.Close();
            }

            return null;
        }

        /// <summary>
        /// Reads in all data for a tile.
        /// </summary>
        /// <param name="tileRec">A TileRec, passed by reference, to be read in.</param>
        /// <param name="reader">The BinaryReader reading in the data.</param>
        private static void ReadTileData(ref TileRec tileRec, BinaryReader reader)
        {
            tileRec.Ground = reader.ReadInt32();
            tileRec.Mask = reader.ReadInt32();
            tileRec.Anim = reader.ReadInt32();
            tileRec.Mask2 = reader.ReadInt32();
            tileRec.M2Anim = reader.ReadInt32();
            tileRec.Fringe = reader.ReadInt32();
            tileRec.FAnim = reader.ReadInt32();
            tileRec.Fringe2 = reader.ReadInt32();
            tileRec.F2Anim = reader.ReadInt32();
            tileRec.Type = reader.ReadByte();
            tileRec.Data1 = reader.ReadInt32();
            tileRec.Data2 = reader.ReadInt32();
            tileRec.Data3 = reader.ReadInt32();

            int length = reader.ReadByte();
            reader.ReadByte();
            //If the length is 0, the string itself will take up nothing in memory, but we should still set it to an empty one
            if (length == 0)
            {
                tileRec.String1 = string.Empty;
            }
            else tileRec.String1 = new string(reader.ReadChars(length));

            length = reader.ReadByte();
            reader.ReadByte();
            if (length == 0)
            {
                tileRec.String2 = string.Empty;
            }
            else tileRec.String2 = new string(reader.ReadChars(length));

            length = reader.ReadByte();
            reader.ReadByte();
            if (length == 0)
            {
                tileRec.String3 = string.Empty;
            }
            else tileRec.String3 = new string(reader.ReadChars(length));

            tileRec.light = reader.ReadInt32();

            tileRec.GroundSet = reader.ReadByte();
            tileRec.MaskSet = reader.ReadByte();
            tileRec.AnimSet = reader.ReadByte();
            tileRec.Mask2Set = reader.ReadByte();
            tileRec.M2AnimSet = reader.ReadByte();
            tileRec.FringeSet = reader.ReadByte();
            tileRec.FAnimSet = reader.ReadByte();
            tileRec.Fringe2Set = reader.ReadByte();
            tileRec.F2AnimSet = reader.ReadByte();
        }

        private static void Print2DArray<T>(T[,] array)
        {
            if (array == null) return;
            
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.WriteLine($"i: {i}, j: {j} - {array[i, j]}");
                }
            }
        }

        private static void PrintArray<T>(T[] array)
        {
            if (array == null) return;

            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }
        }
    }
}
