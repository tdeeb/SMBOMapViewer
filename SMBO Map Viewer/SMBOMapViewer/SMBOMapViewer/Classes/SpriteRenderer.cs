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
        public GraphicsDevice graphicsDevice => graphicsDeviceManager.GraphicsDevice;
        private SpriteBatch spriteBatch = null;

        public RenderTarget2D RenderTarget { get; private set; } = null;

        private SpriteRenderer()
        {

        }

        public void CleanUp()
        {
            graphicsDeviceManager.Dispose();
            spriteBatch.Dispose();
            RenderTarget.Dispose();

            instance = null;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            graphicsDeviceManager = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);

            RenderTarget = new RenderTarget2D(graphicsDevice, Constants.PIC_X * Constants.MAX_MAPX, Constants.PIC_Y * Constants.MAX_MAPY);
        }

        /// <summary>
        /// Adjusts the size of the game window
        /// </summary>
        /// <param name="newWindowSize">The new size of the window</param>
        public void AdjustWindowSize(Vector2 newWindowSize)
        {
            int width = (int)newWindowSize.X;
            int height = (int)newWindowSize.Y;

            graphicsDeviceManager.PreferredBackBufferWidth = width;
            graphicsDeviceManager.PreferredBackBufferHeight = height;

            //Adjust RenderTarget size if the screen size changed
            if (width != RenderTarget.Width || height != RenderTarget.Height)
            {
                RenderTarget.Dispose();
                RenderTarget = new RenderTarget2D(graphicsDevice, width, height);
            }

            graphicsDeviceManager.ApplyChanges();
        }
        
        public void BeginDrawing()
        {
            //Set RenderTarget
            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Black);

            //Render without the matrix so the RenderTarget has the raw map data on it
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null);
        }

        public void EndDrawing()
        {
            spriteBatch.End();

            //Render to the back buffer
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);

            //Draw the RenderTarget with the matrix so the modifications are shown on the back buffer but not on the RenderTarget
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Opaque, SamplerState.PointClamp, transformMatrix: Camera.Instance.CalculateTransformation());

            Draw(RenderTarget, Vector2.Zero, null);

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
