using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SMBOMapViewer
{
    /// <summary>
    /// Handles all time
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// The number of milliseconds in a second.
        /// </summary>
        public const double MsPerS = 1000d;

        private static TimeSpan TotalTime = default(TimeSpan);
        private static TimeSpan ElapsedTime = default(TimeSpan);

        /// <summary>
        /// The frames per second the game runs at
        /// </summary>
        public static readonly double FPS = 60d;

        /// <summary>
        /// The total number of frames since the game booted up.
        /// </summary>
        public static long TotalFrames { get; private set; } = 0;

        /// <summary>
        /// The total amount of time, in milliseconds, since the game booted up
        /// </summary>
        public static double TotalMilliseconds => TotalTime.TotalMilliseconds;

        /// <summary>
        /// The amount of time since the previous frame
        /// </summary>
        public static double ElapsedMilliseconds => ElapsedTime.TotalMilliseconds;

        /// <summary>
        /// Determines if the game is running slowly or not
        /// </summary>
        public static bool RunningSlowly { get; private set; } = false;

        /// <summary>
        /// Updates the game time
        /// </summary>
        /// <param name="gameTime">Provides a snapshop of timing values</param>
        public static void UpdateTime(GameTime gameTime)
        {
            TotalTime = gameTime.TotalGameTime;
            ElapsedTime = gameTime.ElapsedGameTime;
            RunningSlowly = gameTime.IsRunningSlowly;

            TotalFrames++;
        }
    }
}
