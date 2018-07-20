using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SMBOMapViewer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private enum MapChangingMode
        {
            /// <summary>
            /// Linearly goes through each map.
            /// </summary>
            Linear,

            /// <summary>
            /// Goes through the maps as if you were playing the game.
            /// Up, Down, Left, and Right will go to the maps that connect to the current one, if a valid one exists.
            /// </summary>
            GameLike
        }

        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The current map number.
        /// </summary>
        private int MapNum = 1;
    
        /// <summary>
        /// The current map loaded.
        /// </summary>
        private MapRec CurMap = null;

        /// <summary>
        /// Whether we should take a screenshot this frame or not.
        /// </summary>
        private bool ShouldScreenshot = false;

        /// <summary>
        /// The current map changing mode.
        /// </summary>
        private MapChangingMode MapMode = MapChangingMode.Linear;

        private double MapAnimTracker = 0d;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            AssetManager.Instance.Initialize(Content);
            SpriteRenderer.Instance.Initialize(graphics);
            Camera.Instance.Initialize(Vector2.Zero, 0f, 1);

            SpriteRenderer.Instance.AdjustWindowSize(new Vector2((Constants.PIC_X * (Constants.MAX_MAPX - 10)), (Constants.PIC_Y * (Constants.MAX_MAPY / 2))));

            LoadMapWrapper(MapNum);
        }

        /// <summary>
        /// Loads a map with the <see cref="MapLoader"/>, setting the window title to the map number and name.
        /// </summary>
        /// <param name="mapNum">The map number to load.</param>
        private void LoadMapWrapper(int mapNum)
        {
            CurMap = MapLoader.LoadMap(mapNum);

            //Show map name and number in the window title
            Window.Title = "SMBO Map Viewer";

            if (CurMap != null)
            {
                Window.Title += $" - Map {mapNum} - {CurMap.Name}";
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            AssetManager.Instance.CleanUp();
            SpriteRenderer.Instance.CleanUp();
        }

        private void HandleMapChanging()
        {
            //Handle changing maps based on the mode
            if (MapMode == MapChangingMode.Linear)
            {
                //Change maps with left and right
                //Up and Down progress 10 maps at a time
                if (Input.GetKeyDown(Keys.Left) == true)
                {
                    MapNum = Utility.Wrap(MapNum - 1, Constants.MIN_MAPNUM, Constants.MAX_MAPNUM);

                    LoadMapWrapper(MapNum);
                }
                else if (Input.GetKeyDown(Keys.Up) == true)
                {
                    MapNum = Utility.Wrap(MapNum - 10, Constants.MIN_MAPNUM, Constants.MAX_MAPNUM);

                    LoadMapWrapper(MapNum);
                }
                else if (Input.GetKeyDown(Keys.Right) == true)
                {
                    MapNum = Utility.Wrap(MapNum + 1, Constants.MIN_MAPNUM, Constants.MAX_MAPNUM);

                    LoadMapWrapper(MapNum);
                }
                else if (Input.GetKeyDown(Keys.Down) == true)
                {
                    MapNum = Utility.Wrap(MapNum + 10, Constants.MIN_MAPNUM, Constants.MAX_MAPNUM);

                    LoadMapWrapper(MapNum);
                }
            }
            else
            {
                //This mode doesn't work if we don't have a map loaded
                if (CurMap == null)
                {
                    return;
                }

                int newMapNum = 0;

                if (Input.GetKeyDown(Keys.Up) == true)
                {
                    newMapNum = CurMap.Up;
                }
                else if (Input.GetKeyDown(Keys.Left) == true)
                {
                    newMapNum = CurMap.Left;
                }
                else if (Input.GetKeyDown(Keys.Down) == true)
                {
                    newMapNum = CurMap.Down;
                }
                else if (Input.GetKeyDown(Keys.Right) == true)
                {
                    newMapNum = CurMap.Right;
                }

                //Load the new map
                if (newMapNum > 0)
                {
                    MapNum = newMapNum;
                    LoadMapWrapper(MapNum);
                }
            }
        }

        private void HandleInput()
        {
            //Space tells to take a screenshot
            if (Input.GetKeyDown(Keys.Space) == true)
            {
                ShouldScreenshot = true;
            }

            //Handle input for changing maps
            HandleMapChanging();

            //Toggle between map modes
            if (Input.GetKeyDown(Keys.G) == true)
            {
                MapMode = MapChangingMode.GameLike;
            }
            else if (Input.GetKeyDown(Keys.L) == true)
            {
                MapMode = MapChangingMode.Linear;
            }

            //Toggle roof tiles with O
            else if (Input.GetKeyDown(Keys.O) == true)
            {
                MapControlSettings.ShowHiddenRoofTiles = !MapControlSettings.ShowHiddenRoofTiles;
            }

            //WASD pans the camera
            if (Input.GetKey(Keys.A) == true)
            {
                Camera.Instance.Translate(new Vector2(-2, 0));
            }
            if (Input.GetKey(Keys.D) == true)
            {
                Camera.Instance.Translate(new Vector2(2, 0));
            }
            if (Input.GetKey(Keys.W) == true)
            {
                Camera.Instance.Translate(new Vector2(0, -2));
            }
            if (Input.GetKey(Keys.S) == true)
            {
                Camera.Instance.Translate(new Vector2(0, 2));
            }

            //+ and - zoom the camera
            if (Input.GetKeyDown(Keys.OemPlus) == true)
            {
                //Increase scale
                Camera.Instance.Zoom(.1f);
            }
            else if (Input.GetKeyDown(Keys.OemMinus) == true)
            {
                //Increase scale
                Camera.Instance.Zoom(-.1f);
            }

            //Reset the camera with R
            if (Input.GetKeyDown(Keys.R) == true)
            {
                Camera.Instance.SetTranslation(Vector2.Zero);
                Camera.Instance.SetRotation(0f);
                Camera.Instance.SetZoom(1);
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Time.UpdateTime(gameTime);

            //Handle changing the map animation
            MapAnimTracker += Time.ElapsedMilliseconds;
            if (MapAnimTracker >= Constants.MAP_ANIM_TIMER)
            {
                MapControlSettings.RenderAnimTiles = !MapControlSettings.RenderAnimTiles;
                MapAnimTracker = 0d;
            }

            HandleInput();

            Input.UpdateInputState();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SpriteRenderer.Instance.BeginDrawing();
                    
            //Render maps in a separate method
            MapRender();

            PostRender();

            SpriteRenderer.Instance.EndDrawing();

            //Capture a screenshot if so
            if (ShouldScreenshot == true)
            {
                ShouldScreenshot = false;

                //Don't bother if the map isn't loaded
                if (CurMap != null)
                {
                    //Save the RenderTexture
                    Texture2D tex = SpriteRenderer.Instance.RenderTarget;

                    //Save the map
                    SaveMapWindows(tex);
                    //SaveMapNonWindows(tex);
                }
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Renders the map.
        /// </summary>
        private void MapRender()
        {
            CurMap?.DrawMap();
        }

        /// <summary>
        /// Renders anything after the map. Use this for a UI; for screenshots, this won't show up.
        /// </summary>
        private void PostRender()
        {

        }

        private void SaveMapWindows(Texture2D tex)
        {
            //Open the file dialogue so you can name the file and place it wherever you want
            System.Windows.Forms.SaveFileDialog dialogue = new System.Windows.Forms.SaveFileDialog();

            //Several maps have question marks in their names
            //Questions marks can't be in filenames, so if you encounter one, replace it with a space
            //If you try to save an invalid filename, there's no feedback, so this is how we're avoiding that problem
            dialogue.FileName = $"Map {MapNum} - {CurMap.Name.TrimEnd(' ').Replace('?', ' ')}";
            dialogue.Filter = "PNG (*.png)|*.png";

            if (dialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream fstream = new FileStream(dialogue.FileName, FileMode.Create))
                {
                    tex.SaveAsPng(fstream, tex.Width, tex.Height);
                }
            }
        }

        private void SaveMapNonWindows(Texture2D tex)
        {
            /* Non-Windows platforms can't use System.Windows.Forms on Mono, so we'll put all screenshots in a dedicated folder */
            string screenshotFolderPath = Path.Combine(Environment.CurrentDirectory, Constants.SCREENSHOT_FOLDER_NAME);

            //Create the directory if it doesn't exist
            if (Directory.Exists(screenshotFolderPath) == false)
            {
                Directory.CreateDirectory(screenshotFolderPath);
            }

            string fileName = $"Map {MapNum} - {CurMap.Name.TrimEnd(' ').Replace('?', ' ')}";
            string filePath = Path.Combine(screenshotFolderPath, fileName);

            int? index = null;

            string finalPath = GetNextPath(filePath, index);

            //Keep searching for the next file name
            while (File.Exists(finalPath) == true)
            {
                if (index == null) index = 0;
                else index++;

                finalPath = GetNextPath(filePath, index);
            }

            //Save the file
            using (FileStream fstream = new FileStream(finalPath, FileMode.Create))
            {
                tex.SaveAsPng(fstream, tex.Width, tex.Height);
            }
        }

        private string GetNextPath(string curPath, int? index)
        {
            if (index == null) return curPath + ".png";
            else return curPath + index + ".png";
        }
    }
}
