using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.Rendering.Text;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement;
using System;

namespace SlaamMono.Gameplay
{
    /// <summary>
    /// Scoreboards for the players in a game.
    /// </summary>
    public class GameScreenTimer
    {
        private Vector2 Position;
        public TimeSpan GameMatchTime;
        public TimeSpan TimeRemaining;
        public TimeSpan CurrentGameTime = new TimeSpan();
        public readonly TimeSpan EndingTime = CurrentMatchSettings.TimeOfMatch;
        private const float MovementSpeed = 10f / 10f;
        public bool Moving = false;
        private GameScreen ParentGameScreen;
        private readonly IResources _resources;
        private float StepSize;
        private float CurrentStep;

        public GameScreenTimer(Vector2 position, GameScreen parentgamescreen, IResources resources)
        {
            Position = position;
            ParentGameScreen = parentgamescreen;
            _resources = resources;

            TimeRemaining = EndingTime;
            StepSize = (float)TimeRemaining.TotalMilliseconds / 7f;
            SetGameMatchTime(ParentGameScreen.ThisGameType);

        }

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
                {
                    TimeRemaining = TimeSpan.Zero;
                }

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

        /// <summary>
        /// Draws the board on top of the gamescreen with various information
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("TopGameBoard").Texture, new Vector2(1280 - _resources.GetTexture("TopGameBoard").Width + Position.X, 0), Color.White);
            RenderGraphManager.Instance.RenderText(padNumber(GameMatchTime.Minutes), new Vector2(1181.5f + Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, TextAlignment.Centered, false);
            RenderGraphManager.Instance.RenderText(padNumber(GameMatchTime.Seconds), new Vector2(1219.5f + Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, TextAlignment.Centered, false);
            RenderGraphManager.Instance.RenderText(padNumber(GameMatchTime.Milliseconds), new Vector2(1257.5f + Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, TextAlignment.Centered, false);
            if (ParentGameScreen.ThisGameType == GameType.Classic || ParentGameScreen.ThisGameType == GameType.Spree || ParentGameScreen.ThisGameType == GameType.Survival)
            {
                RenderGraphManager.Instance.RenderText("Time Elapsed", new Vector2(Position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, TextAlignment.Right, true);
            }
            else if (ParentGameScreen.ThisGameType == GameType.TimedSpree)
            {
                RenderGraphManager.Instance.RenderText("Time Remaining", new Vector2(Position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, TextAlignment.Right, true);
            }
        }

        private void SetGameMatchTime(GameType type)
        {
            switch (type)
            {
                case GameType.Classic:
                case GameType.Spree:
                case GameType.Survival:
                    GameMatchTime = CurrentGameTime;
                    return;
                case GameType.TimedSpree:
                    GameMatchTime = TimeRemaining;
                    return;

            }
        }

        /// <summary>
        /// Takes an int and gives it leading leading zeros if it needs it.
        /// </summary>
        /// <param name="x">Int to convert</param>
        /// <returns></returns>
        private string padNumber(int x) => x.ToString("00");
    }
}
