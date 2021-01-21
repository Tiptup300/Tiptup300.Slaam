using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SlaamMono
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
        private const float MovementSpeed = (20f / 10f);
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
                Position.X += FPSManager.MovementFactor * MovementSpeed;
                if (Position.X >= 0)
                {
                    Position.X = 0;
                    Moving = false;
                }

            }
            if (Character.CurrentPowerup != null && Character.CurrentPowerup.Active)
            {
                Alpha += (AlphaUp ? 1 : -1) * FPSManager.MovementFactor;

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
            batch.Draw(Resources.GameScreenScoreBoard.Texture, Position, Color.White);
            Resources.DrawString(Character.GetProfile().Name, new Vector2(8 + Position.X, 18 + Position.Y), Resources.SegoeUIx14pt, FontAlignment.Left, Color.White, true);
            Resources.DrawString(Character.Kills.ToString(), new Vector2(35 + Position.X, 68 + Position.Y), Resources.SegoeUIx14pt, FontAlignment.Center, Color.White, true);
            if (CurrentGametype == GameType.Classic || CurrentGametype == GameType.Survival)
                Resources.DrawString(Character.Lives.ToString(), new Vector2(73 + Position.X, 68 + Position.Y), Resources.SegoeUIx14pt, FontAlignment.Center, Color.White, true);
            else if(CurrentGametype == GameType.Spree || CurrentGametype == GameType.TimedSpree)
                Resources.DrawString("inf.", new Vector2(73 + Position.X, 68 + Position.Y), Resources.SegoeUIx14pt, FontAlignment.Center, Color.White, true);
            Character.Draw(batch, new Vector2(184 + Position.X, 61 + Position.Y));
            batch.Draw(Resources.Dot, new Rectangle((int)Math.Round(12 + Position.X), (int)Math.Round(30 + Position.Y),5,33), Character.MarkingColor);
            if (Character.CurrentPowerup != null && !Character.CurrentPowerup.Used)
                batch.Draw(Character.CurrentPowerup.SmallTex, new Vector2(125+Position.X - Character.CurrentPowerup.SmallTex.Width/2, 42 + Position.Y - Character.CurrentPowerup.SmallTex.Height/2), new Color((byte)255, (byte)255, (byte)255,(byte)Alpha));
        }

        #endregion
    }

    /// <summary>
    /// Scoreboards for the players in a game.
    /// </summary>
    public class GameScreenTimer
    {
        #region Variables

        private Vector2 Position;
        public TimeSpan GameMatchTime;
        public TimeSpan TimeRemaining;
        public TimeSpan CurrentGameTime;
        public readonly TimeSpan EndingTime = CurrentMatchSettings.TimeOfMatch;
        private const float MovementSpeed = (10f / 10f);
        public bool Moving = false;
        private GameScreen ParentGameScreen;
        private float StepSize;
        private float CurrentStep;

        #endregion

        #region Constructor

        public GameScreenTimer(Vector2 position, GameScreen parentgamescreen)
        {
            TimeRemaining = EndingTime;
            CurrentGameTime = new TimeSpan();
            Position = position;
            ParentGameScreen = parentgamescreen;
            StepSize = (float)TimeRemaining.TotalMilliseconds / 7f;
            SetGameMatchTime(ParentGameScreen.ThisGameType);

        }

        #endregion

        #region Update

        public void Update(bool StartTiming)
        {
            if (Moving)
            {
                Position.X -= FPSManager.MovementFactor * MovementSpeed;
                if (Position.X <= 0)
                {
                    Position.X = 0;
                    Moving = false;
                }

            }
            if (StartTiming)
            {
                if (TimeRemaining > TimeSpan.Zero || ParentGameScreen.ThisGameType == GameType.Spree || ParentGameScreen.ThisGameType == GameType.Classic || ParentGameScreen.ThisGameType == GameType.Survival)
                {
                    CurrentGameTime += FPSManager.MovementFactorTimeSpan;
                    TimeRemaining -= FPSManager.MovementFactorTimeSpan;
                }

                if (TimeRemaining < TimeSpan.Zero)
                    TimeRemaining = TimeSpan.Zero;

                if (ParentGameScreen.ThisGameType == GameType.TimedSpree)
                {
                    CurrentStep += FPSManager.MovementFactor;

                    if (CurrentStep >= StepSize)
                    {
                        CurrentStep -= StepSize;
                        ParentGameScreen.ShortenBoard();
                    }

                }
            }

            SetGameMatchTime(ParentGameScreen.ThisGameType);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the board on top of the gamescreen with various information
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Resources.TopGameBoard.Texture, new Vector2(1280 - Resources.TopGameBoard.Width + Position.X, 0), Color.White);
            Resources.DrawString(ZeroImpress(GameMatchTime.Minutes), new Vector2(1181.5f + Position.X, 64), Resources.SegoeUIx14pt, FontAlignment.Center, Color.Black, false);
            Resources.DrawString(ZeroImpress(GameMatchTime.Seconds), new Vector2(1219.5f + Position.X, 64), Resources.SegoeUIx14pt, FontAlignment.Center, Color.Black, false);
            Resources.DrawString(ZeroImpress(GameMatchTime.Milliseconds), new Vector2(1257.5f + Position.X, 64), Resources.SegoeUIx14pt, FontAlignment.Center, Color.Black, false);
            if (ParentGameScreen.ThisGameType == GameType.Classic || ParentGameScreen.ThisGameType == GameType.Spree || ParentGameScreen.ThisGameType == GameType.Survival)
            {
                Resources.DrawString("Time Elapsed", new Vector2(Position.X + 1270, 30), Resources.SegoeUIx32pt, FontAlignment.Right, Color.White, true);
            }
            else if (ParentGameScreen.ThisGameType == GameType.TimedSpree)
            {
                Resources.DrawString("Time Remaining", new Vector2(Position.X + 1270, 30), Resources.SegoeUIx32pt, FontAlignment.Right, Color.White, true);
            }
        }

        #endregion

        private void SetGameMatchTime(GameType type)
        {
            if (type == GameType.Classic || type == GameType.Spree || type == GameType.Survival)
                GameMatchTime = CurrentGameTime;
            else if (type == GameType.TimedSpree)
                GameMatchTime = TimeRemaining;
        }

        #region ZeroImpress Method

        /// <summary>
        /// Takes an int and gives it leading leading zeros if it needs it.
        /// </summary>
        /// <param name="x">Int to convert</param>
        /// <returns></returns>
        private string ZeroImpress(int x)
        {
            if (x < 10)
                return "0" + x;
            else
                return x.ToString().Substring(0, 2);
        }

        #endregion
    }
}
