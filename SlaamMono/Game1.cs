#region Using Statements
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#if ZUNE
using ZBlade;
#endif 
#endregion

// 

namespace Slaam
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        new public static ContentManager Content;
        SpriteBatch gamebatch;

#if ZUNE
        public static ZuneBlade mainBlade;
#endif
        public static Game1 Instance { get { return instance; } }

        public static GraphicsDeviceManager Graphics { get { return instance.graphics; } }

        public static Game1 instance;

        public static bool ShowFPS = false;

        #endregion

        #region Constructor

        public Game1()
        {
            LogHelper.Write("XNA Started;");
            graphics = new GraphicsDeviceManager(this);
            LogHelper.Write("Graphics Device Manager Created;");
            Content = new ContentManager(Services);
            LogHelper.Write("Content Manager Created;");
            this.Exiting += Game1_Exiting;

            graphics.PreferredBackBufferWidth = GameGlobals.DRAWING_GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GameGlobals.DRAWING_GAME_HEIGHT;
            this.IsFixedTimeStep = false;

            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Insert(0, new FPSManager(this));
            Components.Add(new AudioManager(this));
            Components.Add(new Input(this));

#if ZUNE
            SetupZuneBlade();
#endif
            // TODO: Add your initialization logic here
            LogHelper.Write("Creating SpriteBatch...");
            gamebatch = new SpriteBatch(graphics.GraphicsDevice);
            LogHelper.Write("Created SpriteBatch;");
            base.Initialize();
            
            LogHelper.Write("Set Graphics Settings (1280x1024 No MultiSampling);");
            instance = this;
            Resources.LoadAll();
            Qwerty.CurrentPlayer = Input.Players[0];
            XNAContentManager.Initialize();

            GameGlobals.SetupGame();
        }
#if ZUNE
        public void SetupZuneBlade()
        {
            mainBlade = new ZuneBlade(this);

            mainBlade.Opacity = 1f;
            mainBlade.CurrentGameInfo = new GameInfo("Slaam! Mobile", "Zibith Games", null);
            mainBlade.UserCanNavigateMenu = true;
            mainBlade.UserCanCloseMenu = false;
            mainBlade.Status = BladeStatus.Hidden;

            Components.Add(mainBlade);
        }
#endif
        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Console.WriteLine("dog");

#if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.PrintScreen))
            { 
                ResolveTexture2D renderTarget = new ResolveTexture2D(GraphicsDevice,240,320,1,SurfaceFormat.Color);
                GraphicsDevice.ResolveBackBuffer(renderTarget);

                renderTarget.Save("save.bmp", ImageFileFormat.Bmp);
            }
#endif

            if (XNAContentManager.NeedsDevice)
                XNAContentManager.Update();
            else
            {

                BackgroundManager.Update();
                FeedManager.Update();
                
#if !ZUNE
                // Allows the default game to exit on Xbox 360 and Windows
                if (Input.GetGamepad().PressedBack || Input.GetKeyboard().PressedKey(Keys.Escape))
                    this.Exit();

                if ((Input.GetGamepad().PressingLeftShoulder && Input.GetGamepad().PressingRightShoulder && Input.GetGamepad().PressingPadDown && Input.GetGamepad().PressedY) || Input.GetKeyboard().PressedKey(Keys.F))
                {
                    graphics.ToggleFullScreen();
                    LogHelper.Write("Fullscreen Toggled");
                }

                if (Input.GetKeyboard().PressingKey(Keys.S) && Input.GetKeyboard().PressedKey(Keys.P))
                {
                    ShowFPS = !ShowFPS;
                }
#endif
                if (Qwerty.Active)
                {
                    Qwerty.Update();
                }
                else
                {
                    ScreenHelper.Update();
                }
            }

        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            //gamebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Matrix.Identity /* * Matrix.CreateScale(new Vector3(1f,720f/1024f,1f))*/);

            gamebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);

            BackgroundManager.Draw(gamebatch);

            ScreenHelper.Draw(gamebatch);

            FeedManager.Draw(gamebatch);

            if (Qwerty.Active)
                Qwerty.Draw(gamebatch);

            if (ShowFPS)
            {
                string temp = ""+FPSManager.FUPS;
                Vector2 fpsBack = Resources.SegoeUIx32pt.MeasureString(temp);
                gamebatch.Draw(Resources.Dot, new Rectangle(0, 0, (int)fpsBack.X + 10, (int)fpsBack.Y), new Color(0, 0, 0, 100));
                Resources.DrawString(temp, new Vector2(5, fpsBack.Y / 2f), Resources.SegoeUIx32pt, FontAlignment.Left, Color.White, true);
            }

            gamebatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Exiting

        void Game1_Exiting(object sender, EventArgs e)
        {
            GC.Collect();
        }
        
#endregion
    }
}