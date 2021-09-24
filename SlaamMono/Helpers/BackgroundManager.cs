using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SlaamMono
{
    /// <summary>
    /// Handles all the work of backgrounds.
    /// </summary>
    static class BackgroundManager
    {
        #region Variables

        private static BackgroundType CurrentType = BackgroundType.Normal;
        private static float BGOffset = 0f;
        private static float Rotation = 0f;
        private const float RotationSpeed = MathHelper.Pi / 3000f;
        private static float Multiplier = 1f;
        //private static TimeSpan TimeElapsed = new TimeSpan();

        #endregion

        #region Update

        public static void Update()
        {
            if (CurrentType == BackgroundType.BattleScreen)
            {
                BGOffset += FrameRateDirector.MovementFactor * (10f / 100f);
                if (BGOffset >= GameGlobals.DRAWING_GAME_HEIGHT)
                    BGOffset = 0;
            }
            else if (CurrentType == BackgroundType.Menu)
            {
                Rotation += FrameRateDirector.MovementFactor * RotationSpeed * Multiplier;
            }
        }

        #endregion

        #region Draw

        public static void Draw(SpriteBatch batch)
        {
            if (CurrentType == BackgroundType.Normal)
            {
                batch.Draw(Resources.Background.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            }
            else if (CurrentType == BackgroundType.Menu)
            {
                batch.Draw(Resources.Background.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
#if !ZUNE
                batch.Draw(Resources.Gear.Texture, new Vector2(1280, 1024), new Rectangle(0, 0, Resources.Gear.Width, Resources.Gear.Height), Color.White, Rotation, new Vector2(Resources.Gear.Width / 2, Resources.Gear.Height / 2), 1f, SpriteEffects.None, 0);
                batch.Draw(Resources.Gear.Texture, new Vector2(0, 50), new Rectangle(0, 0, Resources.Gear.Width, Resources.Gear.Height), Color.White, -Rotation, new Vector2(Resources.Gear.Width / 2, Resources.Gear.Height / 2), 0.75f, SpriteEffects.FlipVertically, 0);
                batch.Draw(Resources.Gear.Texture, new Vector2(0, 1024), new Rectangle(0, 0, Resources.Gear.Width, Resources.Gear.Height), Color.White, Rotation, new Vector2(Resources.Gear.Width / 2, Resources.Gear.Height / 2), 0.5f, SpriteEffects.None, 0);
#endif
                batch.Draw(Resources.MenuTop.Texture, Vector2.Zero, Color.White);
            }
            else if (CurrentType == BackgroundType.Credits)
            {
                // Do nothing, we want black.
            }
            else if (CurrentType == BackgroundType.BattleScreen)
            {
                batch.Draw(Resources.BattleBG.Texture, new Vector2(0, BGOffset - Resources.BattleBG.Height), Color.White);
                batch.Draw(Resources.BattleBG.Texture, new Vector2(0, BGOffset), Color.White);
            }
        }

        public static void DrawMenu(SpriteBatch batch)
        {
            BackgroundType temp = CurrentType;
            ChangeBG(BackgroundType.Menu);
            Draw(batch);
            ChangeBG(temp);
        }

        #endregion

        #region ChangeBG Method

        /// <summary>
        /// Sets the background type and does calculations for the according type.
        /// </summary>
        /// <param name="type"></param>
        public static void ChangeBG(BackgroundType type)
        {
            CurrentType = type;
        }

        public static void SetRotation(float rotation)
        {
            Multiplier = rotation;
        }

        #endregion

        #region Enums

        public enum BackgroundType
        {
            Normal,
            Menu,
            Credits,
            BattleScreen,
        }

        #endregion
    }
}
