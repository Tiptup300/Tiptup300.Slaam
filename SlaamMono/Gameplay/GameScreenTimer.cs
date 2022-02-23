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
            _state.Position = position;
            _state.GameType = gameType;

            _state.TimeRemaining = _state.EndingTime;
            _state.StepSize = (float)_state.TimeRemaining.TotalMilliseconds / 7f;
            setGameMatchTime(gameType);

        }

        public void Update(GameScreenState gameScreenState)
        {
            if (Moving)
            {
                _state.Position.X -= FrameRateDirector.MovementFactor * _state.MovementSpeed;
                if (_state.Position.X <= 0)
                {
                    _state.Position.X = 0;
                    _state.Moving = false;
                }
            }
            if (gameScreenState.IsTiming)
            {
                if (_state.TimeRemaining > TimeSpan.Zero || _state.GameType == GameType.Spree || _state.GameType == GameType.Classic || _state.GameType == GameType.Survival)
                {
                    _state.CurrentGameTime += FrameRateDirector.MovementFactorTimeSpan;
                    _state.TimeRemaining -= FrameRateDirector.MovementFactorTimeSpan;
                }

                if (_state.TimeRemaining < TimeSpan.Zero)
                {
                    _state.TimeRemaining = TimeSpan.Zero;
                }

                if (_state.GameType == GameType.TimedSpree)
                {
                    _state.CurrentStep += FrameRateDirector.MovementFactor;

                    if (_state.CurrentStep >= _state.StepSize)
                    {
                        _state.CurrentStep -= _state.StepSize;
                        GameScreen.ShortenBoard(gameScreenState);
                    }

                }
            }
            setGameMatchTime(_state.GameType);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("TopGameBoard").Texture, new Vector2(1280 - _resources.GetTexture("TopGameBoard").Width + _state.Position.X, 0), Color.White);
            RenderGraph.Instance.RenderText(_state.GameMatchTime.Minutes.ToString("00"), new Vector2(1181.5f + _state.Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            RenderGraph.Instance.RenderText(_state.GameMatchTime.Seconds.ToString("00"), new Vector2(1219.5f + _state.Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            RenderGraph.Instance.RenderText(_state.GameMatchTime.Milliseconds.ToString("00"), new Vector2(1257.5f + _state.Position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            if (_state.GameType == GameType.Classic || _state.GameType == GameType.Spree || _state.GameType == GameType.Survival)
            {
                RenderGraph.Instance.RenderText("Time Elapsed", new Vector2(_state.Position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopRight, true);
            }
            else if (_state.GameType == GameType.TimedSpree)
            {
                RenderGraph.Instance.RenderText("Time Remaining", new Vector2(_state.Position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopRight, true);
            }
        }

        private void setGameMatchTime(GameType type)
        {
            switch (type)
            {
                case GameType.Classic:
                case GameType.Spree:
                case GameType.Survival:
                    _state.GameMatchTime = CurrentGameTime;
                    return;
                case GameType.TimedSpree:
                    _state.GameMatchTime = _state.TimeRemaining;
                    return;

            }
        }
    }
}
