using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SMBOMapViewer
{
    /// <summary>
    /// Handles sprite rendering.
    /// <para>This is a Singleton.</para>
    /// </summary>
    public class SpriteRenderer : ICleanup
    {
        #region Singleton Fields

        private static SpriteRenderer instance = null;

        public static SpriteRenderer Instance
        {
            get
            {
                if (instance == null)
                    instance = new SpriteRenderer();

                return instance;
            }
        }

        #endregion

        public Vector2 WindowSize => new Vector2(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);

        public GraphicsDeviceManager graphicsDeviceManager { get; private set; } = null;
        private SpriteBatch spriteBatch = null;

        private SpriteRenderer()
        {

        }

        public void CleanUp()
        {
            graphicsDeviceManager.Dispose();
            spriteBatch.Dispose();

            instance = null;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            graphicsDeviceManager = graphics;
            spriteBatch = new SpriteBatch(graphicsDeviceManager.GraphicsDevice);
        }

        /// <summary>
        /// Adjusts the size of the game window
        /// </summary>
        /// <param name="newWindowSize">The new size of the window</param>
        public void AdjustWindowSize(Vector2 newWindowSize)
        {
            graphicsDeviceManager.PreferredBackBufferWidth = (int)newWindowSize.X;
            graphicsDeviceManager.PreferredBackBufferHeight = (int)newWindowSize.Y;

            graphicsDeviceManager.ApplyChanges();
        }
        
        public void BeginDrawing()
        {
            graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Instance.CalculateTransformation());
        }

        public void EndDrawing()
        {
            spriteBatch.End();
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRect)
        {
            Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, Vector2.One, .5f);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, float layer)
        {
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, SpriteEffects.None, layer);
        }
    }
}
