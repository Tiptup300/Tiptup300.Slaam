using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Slaam
{
    class ScreenieTaker : GameComponent
    {
        #region Variables

        public int PicCount = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Used for taking screenshots of the game.
        /// </summary>
        /// <param name="game">The game itself</param>
        public ScreenieTaker(Game game)
            : base(game)
        {

        }

        #endregion

        #region Update

        /// <summary>
        /// Check to see if their pressing taking a screenshot, then process it if they are.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            /*
            if ((Input.GetGamepad().PressingRightShoulder && Input.GetGamepad().PressingLeftShoulder && Input.GetGamepad().PressedRightTrigger) || Input.GetKeyboard().PressedKey(Keys.PrintScreen))
            {
                if (!System.IO.Directory.Exists("Screens"))
                    System.IO.Directory.CreateDirectory("Screens");

                using (Texture2D dstTexture = new Texture2D(
                    Game1.Graphics.GraphicsDevice,
                    Game1.Graphics.PreferredBackBufferWidth, Game1.Graphics.PreferredBackBufferHeight, 1,
                    ResourceUsage.ResolveTarget,
                    SurfaceFormat.Bgr32,
                    ResourceManagementMode.Manual))
                {
                    // Get data with help of the resolve method
                    Game1.Graphics.GraphicsDevice.ResolveBackBuffer(dstTexture);

                    dstTexture.Save(GetNextScreenShotName(),ImageFileFormat.Png);
                }

                LogHelper.Write("Screenshot Taken");
            }
             * */
            base.Update(gameTime);
        }

        #endregion

        #region Screenshot Name Method

        /// <summary>
        /// Retreives the name of the next screenshot to be made.
        /// </summary>
        private string GetNextScreenShotName()
        {
            while (System.IO.File.Exists("Screens/Screenie" + PicCount + ".png"))
                PicCount++;

            return "Screens/Screenie" + PicCount + ".png";
        }

        #endregion

    }
}
