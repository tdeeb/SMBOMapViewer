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

            try
            {
                //Get next available file num from freefile
                fileNum = FileSystem.FreeFile();

                //Console.WriteLine($"fileNum is {fileNum}");

                MapRec mapRec = new MapRec();

                //Read the data from the file
                FileSystem.FileOpen(fileNum, fileName, OpenMode.Binary);

                //Read in all data
                mapRec.Name = ConvertFixedLengthString(fileNum, 60);
                FileSystem.FileGet(fileNum, ref mapRec.Revision);
                FileSystem.FileGet(fileNum, ref mapRec.Moral);
                FileSystem.FileGet(fileNum, ref mapRec.Up);
                FileSystem.FileGet(fileNum, ref mapRec.Down);
                FileSystem.FileGet(fileNum, ref mapRec.Left);
                FileSystem.FileGet(fileNum, ref mapRec.Right);
                mapRec.music = ConvertDynamicLengthString(fileNum);
                FileSystem.FileGet(fileNum, ref mapRec.BootMap);
                FileSystem.FileGet(fileNum, ref mapRec.BootX);
                FileSystem.FileGet(fileNum, ref mapRec.BootY);
                FileSystem.FileGet(fileNum, ref mapRec.Shop);
                FileSystem.FileGet(fileNum, ref mapRec.Indoors);
                mapRec.Tile = Convert2DArray(fileNum, mapRec.Tile, true);
                mapRec.Npc = ConvertArray(fileNum, mapRec.Npc, false);
                mapRec.SpawnX = ConvertArray(fileNum, mapRec.SpawnX, false);
                mapRec.SpawnY = ConvertArray(fileNum, mapRec.SpawnY, false);
                FileSystem.FileGet(fileNum, ref mapRec.owner);
                FileSystem.FileGet(fileNum, ref mapRec.scrolling);
                FileSystem.FileGet(fileNum, ref mapRec.Weather);

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
            }

            return null;
        }

        /*private static TileRec LoadTile(int fileNum)
        {
            TileRec tileRec = new TileRec();

            FileSystem.FileGet(fileNum, ref tileRec.Ground);
            FileSystem.FileGet(fileNum, ref tileRec.Mask);
            FileSystem.FileGet(fileNum, ref tileRec.Anim);
            FileSystem.FileGet(fileNum, ref tileRec.Mask2);
            FileSystem.FileGet(fileNum, ref tileRec.M2Anim);
            FileSystem.FileGet(fileNum, ref tileRec.Fringe);
            FileSystem.FileGet(fileNum, ref tileRec.FAnim);
            FileSystem.FileGet(fileNum, ref tileRec.Fringe2);
            FileSystem.FileGet(fileNum, ref tileRec.F2Anim);
            FileSystem.FileGet(fileNum, ref tileRec.Type);
            FileSystem.FileGet(fileNum, ref tileRec.Data1);
            FileSystem.FileGet(fileNum, ref tileRec.Data2);
            FileSystem.FileGet(fileNum, ref tileRec.Data3);
            tileRec.String1 = ConvertDynamicLengthString(fileNum);
            tileRec.String2 = ConvertDynamicLengthString(fileNum);
            tileRec.String3 = ConvertDynamicLengthString(fileNum);
            FileSystem.FileGet(fileNum, ref tileRec.light);
            FileSystem.FileGet(fileNum, ref tileRec.GroundSet);
            FileSystem.FileGet(fileNum, ref tileRec.MaskSet);
            FileSystem.FileGet(fileNum, ref tileRec.AnimSet);
            FileSystem.FileGet(fileNum, ref tileRec.Mask2Set);
            FileSystem.FileGet(fileNum, ref tileRec.M2AnimSet);
            FileSystem.FileGet(fileNum, ref tileRec.FringeSet);
            FileSystem.FileGet(fileNum, ref tileRec.FAnimSet);
            FileSystem.FileGet(fileNum, ref tileRec.Fringe2Set);
            FileSystem.FileGet(fileNum, ref tileRec.F2AnimSet);

            return tileRec;
        }*/

        private static string ConvertDynamicLengthString(int fileNum)
        {
            //Get first 2 bytes for length information
            byte[] dStr = new byte[2];
            Array array = dStr;

            FileSystem.FileGet(fileNum, ref array);

            //The 0th index tells the length of the string
            dStr = (byte[])array;

            return ConvertFixedLengthString(fileNum, dStr[0]);
        }

        private static string ConvertFixedLengthString(int fileNum, int length)
        {
            //Fixed length strings have a two-byte descriptor
            byte[] strArray = new byte[length];
            Array array = strArray;

            FileSystem.FileGet(fileNum, ref array, StringIsFixedLength: true);
            strArray = (byte[])array;
            //string thing = BitConverter.ToString(strArray);
            //Console.WriteLine(thing);

            string na = System.Text.Encoding.UTF8.GetString(strArray);

            return na;
        }

        private static T[] ConvertArray<T>(int fileNum, T[] array, bool isArrayDynamic)
        {
            Array arr = array;
            FileSystem.FileGet(fileNum, ref arr, ArrayIsDynamic: isArrayDynamic);

            return (T[])arr;
        }

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
                    Console.WriteLine(array[i, j]);
                }
            }
        }

        private static void PrintArray<T>(T[] array)
        {
            if (array == null) return;

            //Console.WriteLine();

            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }

            //Console.WriteLine();
        }
    }
}
