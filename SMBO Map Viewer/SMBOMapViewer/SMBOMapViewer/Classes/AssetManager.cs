using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SMBOMapViewer
{
    /// <summary>
    /// Helps manage content
    /// <para>This is a Singleton</para>
    /// </summary>
    public class AssetManager : ICleanup
    {
        #region Singleton Fields

        public static AssetManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AssetManager();
                }

                return instance;
            }
        }

        private static AssetManager instance = null;

        #endregion

        private ContentManager Content = null;
        private Dictionary<string, Texture2D> TileMaps = new Dictionary<string, Texture2D>();

        private AssetManager()
        {

        }

        public void Initialize(ContentManager content)
        {
            Content = content;
            Content.RootDirectory = Constants.ContentRoot;
        }

        public void CleanUp()
        {
            Content.Unload();
            Content = null;

            //Dispose all raw Texture2Ds we loaded
            foreach (KeyValuePair<string, Texture2D> tilemap in TileMaps.ToArray())
            {
                tilemap.Value.Dispose();
            }

            TileMaps.Clear();

            instance = null;
        }

        public Texture2D LoadRawTexture2D(string texturePath)
        {
            Texture2D tex = null;

            //Return the cached texture if we have it
            if (TileMaps.ContainsKey(texturePath) == true)
            {
                tex = TileMaps[texturePath];
            }
            else
            {
                //Load the raw texture
                try
                {
                    using (FileStream fileStream = new FileStream(texturePath, FileMode.Open))
                    {
                        tex = Texture2D.FromStream(SpriteRenderer.Instance.graphicsDeviceManager.GraphicsDevice, fileStream);

                        //Cache the texture for faster loading next time
                        if (tex != null)
                        {
                            TileMaps.Add(texturePath, tex);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                }
            }

            return tex;
        }
    }
}
