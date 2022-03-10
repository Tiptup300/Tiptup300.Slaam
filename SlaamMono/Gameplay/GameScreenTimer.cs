using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using System;

namespace SlaamMono.Gameplay
{
    /// <summary>
    /// Scoreboards for the players in a game.
    /// </summary>
    public class GameScreenTimer
    {
        public TimeSpan CurrentGameTime { get => _state.CurrentGameTime; }
        public bool Moving { get => _state.Moving; set { _state.Moving = value; } }
        private GameScreenTimerState _state = new GameScreenTimerState();

        private readonly IResources _resources;

        public GameScreenTimer(IResources resources, Vector2 position, GameType gameType)
        {
            _resources = resources;
            _state = InitializeState(position, gameType);
        }

        public static GameScreenTimerState InitializeState(Vector2 position, GameType gameType)
        {
            GameScreenTimerState output;

            output = new GameScreenTimerState();
            output.Position = position;
            output.GameType = gameType;
            output.TimeRemaining = output.EndingTime;
            output.StepSize = (float)output.TimeRemaining.TotalMilliseconds / 7f;
            output.GameMatchTime = getGameMatchTime(output);

            return output;
        }

        public void Update(GameScreenState gameScreenState) => updateState(gameScreenState, _state);

        public static void updateState(GameScreenState gameScreenState, GameScreenTimerState state)
        {
            if (state.Moving)
            {
                state.Position.X -= FrameRateDirector.MovementFactor * state.MovementSpeed;
                if (state.Position.X <= 0)
                {
                    state.Position.X = 0;
                    state.Moving = false;
                }
            }
            if (gameScreenState.IsTiming)
            {
                if (state.TimeRemaining > TimeSpan.Zero || state.GameType == GameType.Spree || state.GameType == GameType.Classic || state.GameType == GameType.Survival)
                {
                    state.CurrentGameTime += FrameRateDirector.MovementFactorTimeSpan;
                    state.TimeRemaining -= FrameRateDirector.MovementFactorTimeSpan;
                }

                if (state.TimeRemaining < TimeSpan.Zero)
                {
                    state.TimeRemaining = TimeSpan.Zero;
                }

                if (state.GameType == GameType.TimedSpree)
                {
                    state.CurrentStep += FrameRateDirector.MovementFactor;

                    if (state.CurrentStep >= state.StepSize)
                    {
                        state.CurrentStep -= state.StepSize;
                        GameScreenFunctions.ShortenBoard(gameScreenState);
                    }

                }
            }
            state.GameMatchTime = getGameMatchTime(state);
        }

        public void Draw(SpriteBatch batch) => drawState(batch, _state);

        public void drawState(SpriteBatch batch, GameScreenTimerState state)
        {
            batch.Draw(_resources.GetTexture("TopGameBoard").Texture, new Vector2(1280 - _resources.GetTexture("TopGameBoard").Width + state.Position.X, 0), Color.White);
            RenderGraph.Instance.RenderText(state.GameMatchTime.Minutes.ToString("00"), new Vector2(1181.5f + state.Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            RenderGraph.Instance.RenderText(state.GameMatchTime.Seconds.ToString("00"), new Vector2(1219.5f + state.Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            RenderGraph.Instance.RenderText(state.GameMatchTime.Milliseconds.ToString("00"), new Vector2(1257.5f + state.Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            if (state.GameType == GameType.Classic || state.GameType == GameType.Spree || state.GameType == GameType.Survival)
            {
                RenderGraph.Instance.RenderText("Time Elapsed", new Vector2(state.Position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopRight, true);
            }
            else if (state.GameType == GameType.TimedSpree)
            {
                RenderGraph.Instance.RenderText("Time Remaining", new Vector2(state.Position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopRight, true);
            }
        }

        private static TimeSpan getGameMatchTime(GameScreenTimerState state)
        {
            switch (state.GameType)
            {
                case GameType.Classic:
                case GameType.Spree:
                case GameType.Survival:
                    return state.CurrentGameTime;
                case GameType.TimedSpree:
                    return state.TimeRemaining;

            }
            throw new Exception("");
        }
    }
}
