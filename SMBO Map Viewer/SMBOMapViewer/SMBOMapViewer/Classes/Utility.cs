using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBOMapViewer
{
    public static class Utility
    {
        public static int Clamp(int val, int min, int max) => (val < min) ? min : (val > max) ? max : val;
        public static int Wrap(int val, int min, int max) => (val < min) ? max : (val > max) ? min : val;
    }
}
