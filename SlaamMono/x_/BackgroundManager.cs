using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.ResourceManagement;

namespace SlaamMono.x_
{
    /// <summary>
    /// Handles all the work of backgrounds.
    /// </summary>
    static class BackgroundManager
    {
        private static BackgroundType CurrentType = BackgroundType.Normal;
        private static float BGOffset = 0f;
        private static float _multiplier = 1f;

        public static void Update()
        {
            if (CurrentType == BackgroundType.BattleScreen)
            {
                BGOffset += FrameRateDirector.MovementFactor * (10f / 100f);
                if (BGOffset >= GameGlobals.DRAWING_GAME_HEIGHT)
                    BGOffset = 0;
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            if (CurrentType == BackgroundType.Normal)
            {
                batch.Draw(ResourceManager.Instance.GetTexture("Background").Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            }
            else if (CurrentType == BackgroundType.Menu)
            {
                batch.Draw(ResourceManager.Instance.GetTexture("Background").Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
                batch.Draw(ResourceManager.Instance.GetTexture("MenuTop").Texture, Vector2.Zero, Color.White);
            }
            else if (CurrentType == BackgroundType.Credits)
            {
                // Do nothing, we want black.
            }
            else if (CurrentType == BackgroundType.BattleScreen)
            {
                batch.Draw(ResourceManager.Instance.GetTexture("BattleBG").Texture, new Vector2(0, BGOffset - ResourceManager.Instance.GetTexture("BattleBG").Height), Color.White);
                batch.Draw(ResourceManager.Instance.GetTexture("BattleBG").Texture, new Vector2(0, BGOffset), Color.White);
            }
        }

        public static void DrawMenu(SpriteBatch batch)
        {
            BackgroundType temp = CurrentType;
            ChangeBG(BackgroundType.Menu);
            Draw(batch);
            ChangeBG(temp);
        }

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
            _multiplier = rotation;
        }
    }
}
