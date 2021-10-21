using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Resources;
using System;

namespace SlaamMono.SubClasses
{
    /// <summary>
    /// Scoreboards for the players in a game.
    /// </summary>
    public class GameScreenScoreboard
    {
        #region Variables

        private Vector2 Position;
        private Character Character;
        public bool Moving = false;
        private const float MovementSpeed = 20f / 10f;
        private GameType CurrentGametype;
        private bool AlphaUp = false;
        private float Alpha = 255f;

        #endregion

        #region Constructor

        public GameScreenScoreboard(Vector2 position, Character character, GameType type)
        {
            Position = position;
            Character = character;
            CurrentGametype = type;
        }

        #endregion

        #region Update

        public void Update()
        {
            if (Moving)
            {
                Position.X += FrameRateDirector.MovementFactor * MovementSpeed;
                if (Position.X >= 0)
                {
                    Position.X = 0;
                    Moving = false;
                }

            }
            if (Character.CurrentPowerup != null && Character.CurrentPowerup.Active)
            {
                Alpha += (AlphaUp ? 1 : -1) * FrameRateDirector.MovementFactor;

                if (AlphaUp && Alpha >= 255f)
                {
                    AlphaUp = !AlphaUp;
                    Alpha = 255f;
                }
                else if (!AlphaUp && Alpha <= 0f)
                {
                    AlphaUp = !AlphaUp;
                    Alpha = 0f;
                }
            }
            else
                Alpha = 255f;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.GetTexture("GameScreenScoreBoard").Texture, Position, Color.White);
            TextManager.Instance.AddTextToRender(Character.GetProfile().Name, new Vector2(8 + Position.X, 18 + Position.Y), ResourceManager.GetFont("SegoeUIx14pt"), Color.White, TextAlignment.Default, true);
            TextManager.Instance.AddTextToRender(Character.Kills.ToString(), new Vector2(35 + Position.X, 68 + Position.Y), ResourceManager.GetFont("SegoeUIx14pt"), Color.White, TextAlignment.Centered, true);
            if (CurrentGametype == GameType.Classic || CurrentGametype == GameType.Survival)
            {
                TextManager.Instance.AddTextToRender(Character.Lives.ToString(), new Vector2(73 + Position.X, 68 + Position.Y), ResourceManager.GetFont("SegoeUIx14pt"), Color.White, TextAlignment.Centered, true);
            }
            else if (CurrentGametype == GameType.Spree || CurrentGametype == GameType.TimedSpree)
            {
                TextManager.Instance.AddTextToRender("inf.", new Vector2(73 + Position.X, 68 + Position.Y), ResourceManager.GetFont("SegoeUIx14pt"), Color.White, TextAlignment.Centered, true);
            }
            Character.Draw(batch, new Vector2(184 + Position.X, 61 + Position.Y));
            batch.Draw(ResourceManager.WhitePixel, new Rectangle((int)Math.Round(12 + Position.X), (int)Math.Round(30 + Position.Y), 5, 33), Character.MarkingColor);
            if (Character.CurrentPowerup != null && !Character.CurrentPowerup.Used)
                batch.Draw(Character.CurrentPowerup.SmallTex, new Vector2(125 + Position.X - Character.CurrentPowerup.SmallTex.Width / 2, 42 + Position.Y - Character.CurrentPowerup.SmallTex.Height / 2), new Color((byte)255, (byte)255, (byte)255, (byte)Alpha));
        }

        #endregion
    }
}
