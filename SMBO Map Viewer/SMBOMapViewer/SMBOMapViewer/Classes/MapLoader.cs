using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualBasic;

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

            int fileNum = -1;

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

                //The next 64 bytes contain the length of the first rank, and the following 64 bytes contain the length of the second rank
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();

                //We're now at the tile data
                for (int i = 0; i < mapRec.Tile.GetLength(0); i++)
                {
                    for (int j = 0; j < mapRec.Tile.GetLength(1); j++)
                    {
                        ReadTileData(ref mapRec.Tile[i, j], binaryReader);
                    }
                }

                int val = 5;
                int val2 = 23;
                Console.WriteLine(mapRec.Tile[val, val2]);
                
                ////Get next available file num from freefile
                //fileNum = FileSystem.FreeFile();

                ////Console.WriteLine($"fileNum is {fileNum}");

                //MapRec mapRec = new MapRec();

                ////Read the data from the file
                //FileSystem.FileOpen(fileNum, fileName, OpenMode.Binary);

                ////Read in all data
                //mapRec.Name = ConvertFixedLengthString(fileNum, 60);
                //FileSystem.FileGet(fileNum, ref mapRec.Revision);
                //FileSystem.FileGet(fileNum, ref mapRec.Moral);
                //FileSystem.FileGet(fileNum, ref mapRec.Up);
                //FileSystem.FileGet(fileNum, ref mapRec.Down);
                //FileSystem.FileGet(fileNum, ref mapRec.Left);
                //FileSystem.FileGet(fileNum, ref mapRec.Right);
                //mapRec.music = ConvertDynamicLengthString(fileNum);
                //FileSystem.FileGet(fileNum, ref mapRec.BootMap);
                //FileSystem.FileGet(fileNum, ref mapRec.BootX);
                //FileSystem.FileGet(fileNum, ref mapRec.BootY);
                //FileSystem.FileGet(fileNum, ref mapRec.Shop);
                //FileSystem.FileGet(fileNum, ref mapRec.Indoors);
                //mapRec.Tile = Convert2DArray(fileNum, mapRec.Tile, true);
                //mapRec.Npc = ConvertArray(fileNum, mapRec.Npc, false);
                //mapRec.SpawnX = ConvertArray(fileNum, mapRec.SpawnX, false);
                //mapRec.SpawnY = ConvertArray(fileNum, mapRec.SpawnY, false);
                //FileSystem.FileGet(fileNum, ref mapRec.owner);
                //FileSystem.FileGet(fileNum, ref mapRec.scrolling);
                //FileSystem.FileGet(fileNum, ref mapRec.Weather);

                return mapRec;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
            }
            finally
            {
                //Close the file guaranteed
                if (fileNum >= 0)
                {
                    FileSystem.FileClose(fileNum);
                }

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

        /// <summary>
        /// Converts a VB6 dynamic length string stored in binary to a C# string.
        /// </summary>
        /// <param name="fileNum"></param>
        /// <returns></returns>
        private static string ConvertDynamicLengthString(int fileNum)
        {
            //Get first 2 bytes for length information
            byte[] dStr = new byte[2];
            Array array = dStr;

            FileSystem.FileGet(fileNum, ref array);

            //The first byte tells the length of the string
            dStr = (byte[])array;

            return ConvertFixedLengthString(fileNum, dStr[0]);
        }

        /// <summary>
        /// Converts a VB6 fixed-length string stored in binary to a C# string.
        /// </summary>
        /// <param name="fileNum"></param>
        /// <param name="length">The length of the fixed-length string.</param>
        /// <returns></returns>
        private static string ConvertFixedLengthString(int fileNum, int length)
        {
            //Fixed length strings have a two-byte descriptor
            byte[] strArray = new byte[length];
            Array array = strArray;

            FileSystem.FileGet(fileNum, ref array, StringIsFixedLength: true);
            strArray = (byte[])array;

            string na = System.Text.Encoding.UTF8.GetString(strArray);

            return na;
        }

        /// <summary>
        /// Converts a VB6 array stored in binary to a C# array.
        /// </summary>
        private static T[] ConvertArray<T>(int fileNum, T[] array, bool isArrayDynamic)
        {
            Array arr = array;
            FileSystem.FileGet(fileNum, ref arr, ArrayIsDynamic: isArrayDynamic);

            return (T[])arr;
        }

        /// <summary>
        /// Converts a VB6 multi-dimensional array stored in binary to a C# array. This does not handle jagged arrays.
        /// </summary>
        private static T[,] Convert2DArray<T>(int fileNum, T[,] array, bool isArrayDynamic)
        {
            Array arr = array;
            FileSystem.FileGet(fileNum, ref arr, ArrayIsDynamic: isArrayDynamic);

            return (T[,])arr;
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
