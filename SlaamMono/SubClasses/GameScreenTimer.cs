using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Resources;
using SlaamMono.Screens;
using System;

namespace SlaamMono.SubClasses
{
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
        private const float MovementSpeed = 10f / 10f;
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
                Position.X -= FrameRateDirector.MovementFactor * MovementSpeed;
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
                    CurrentGameTime += FrameRateDirector.MovementFactorTimeSpan;
                    TimeRemaining -= FrameRateDirector.MovementFactorTimeSpan;
                }

                if (TimeRemaining < TimeSpan.Zero)
                    TimeRemaining = TimeSpan.Zero;

                if (ParentGameScreen.ThisGameType == GameType.TimedSpree)
                {
                    CurrentStep += FrameRateDirector.MovementFactor;

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
            batch.Draw(ResourceManager.TopGameBoard.Texture, new Vector2(1280 - ResourceManager.TopGameBoard.Width + Position.X, 0), Color.White);
            TextManager.Instance.AddTextToRender(ZeroImpress(GameMatchTime.Minutes), new Vector2(1181.5f + Position.X, 64), ResourceManager.SegoeUIx14pt, Color.Black, TextAlignment.Centered, false);
            TextManager.Instance.AddTextToRender(ZeroImpress(GameMatchTime.Seconds), new Vector2(1219.5f + Position.X, 64), ResourceManager.SegoeUIx14pt, Color.Black, TextAlignment.Centered, false);
            TextManager.Instance.AddTextToRender(ZeroImpress(GameMatchTime.Milliseconds), new Vector2(1257.5f + Position.X, 64), ResourceManager.SegoeUIx14pt, Color.Black, TextAlignment.Centered, false);
            if (ParentGameScreen.ThisGameType == GameType.Classic || ParentGameScreen.ThisGameType == GameType.Spree || ParentGameScreen.ThisGameType == GameType.Survival)
            {
                TextManager.Instance.AddTextToRender("Time Elapsed", new Vector2(Position.X + 1270, 30), ResourceManager.SegoeUIx32pt, Color.White, TextAlignment.Right, true);
            }
            else if (ParentGameScreen.ThisGameType == GameType.TimedSpree)
            {
                TextManager.Instance.AddTextToRender("Time Remaining", new Vector2(Position.X + 1270, 30), ResourceManager.SegoeUIx32pt, Color.White, TextAlignment.Right, true);
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
