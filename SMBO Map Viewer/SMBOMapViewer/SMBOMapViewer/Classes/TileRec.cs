using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SMBOMapViewer
{
    /// <summary>
    /// Tile Data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 86)]
    public struct TileRec
    {
        public int Ground; //VB6 integers are 16 bits and longs are 32 bits //Ground layer
        public int Mask; //Mask layer
        public int Anim; //Anim layer
        public int Mask2; //Mask2 layer
        public int M2Anim; //Mask2 anim layer
        public int Fringe; //Fringe layer
        public int FAnim; //Fringe anim layer
        public int Fringe2; //Fringe2 layer
        public int F2Anim; //Fringe2 anim layer
        public byte Type; //Tile type
        public int Data1; 
        public int Data2;
        public int Data3;
        public string String1; //String used for signs
        public string String2; //String used for signs
        public string String3; //String used for signs
        public int light;
        public byte GroundSet; //Ground tileset
        public byte MaskSet; //Mask tileset
        public byte AnimSet; //Anim tileset
        public byte Mask2Set; //Mask2 tileset
        public byte M2AnimSet; //Mask2 anim tileset
        public byte FringeSet; //Fringe tileset
        public byte FAnimSet; //Fringe anim tileset
        public byte Fringe2Set; //Fringe2 tileset
        public byte F2AnimSet; //Fringe2 anim tileset

        public override string ToString()
        {
            return $"GROUND: {Ground}\n" +
                $"MASK: {Mask}\n" +
                $"ANIM: {Anim}\n" +
                $"MASK2: {Mask2}\n" +
                $"M2ANIM: {M2Anim}\n" +
                $"FRINGE: {Fringe}\n" +
                $"FANIM: {FAnim}\n" +
                $"FRINGE2: {Fringe2}\n" +
                $"F2ANIM: {F2Anim}\n" +
                $"TYPE: {Type}\n" +
                $"DATA1: {Data1}\n" +
                $"DATA2: {Data2}\n" +
                $"DATA3: {Data3}\n" +
                $"STRING1: {String1}\n" +
                $"STRING2: {String2}\n" +
                $"STRING3: {String3}\n" +
                $"LIGHT: {light}\n" +
                $"GROUNDSET: {GroundSet}\n" +
                $"MASKSET: {MaskSet}\n" +
                $"ANIMSET: {AnimSet}\n" +
                $"MASK2SET: {Mask2Set}\n" +
                $"M2ANIMSET: {M2AnimSet}\n" +
                $"FRINGESET: {FringeSet}\n" +
                $"FANIMSET: {FAnimSet}\n" +
                $"FRINGE2SET: {Fringe2Set}\n" +
                $"F2ANIMSET: {F2AnimSet}\n";
        }
    }
}
