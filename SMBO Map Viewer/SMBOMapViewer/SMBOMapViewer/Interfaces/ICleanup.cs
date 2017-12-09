﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBOMapViewer
{
    /// <summary>
    /// An interface for objects that perform cleanup logic.
    /// This is where they can clear events and other data.
    /// </summary>
    public interface ICleanup
    {
        /// <summary>
        /// Performs clean up on the object, clearing events and other data.
        /// </summary>
        void CleanUp();
    }
}
