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
        public TimeSpan GameMatchTime;
        public TimeSpan TimeRemaining;
        public TimeSpan CurrentGameTime = new TimeSpan();
        public readonly TimeSpan EndingTime = MatchSettings.CurrentMatchSettings.TimeOfMatch;
        public bool Moving = false;

        private Vector2 _position;
        private const float _movementSpeed = 10f / 10f;
        private GameScreen _parentGameScreen;
        private float _stepSize;
        private float _currentStep;

        private readonly IResources _resources;

        public GameScreenTimer(Vector2 position, GameScreen parentgamescreen, IResources resources)
        {
            _position = position;
            _parentGameScreen = parentgamescreen;
            _resources = resources;

            TimeRemaining = EndingTime;
            _stepSize = (float)TimeRemaining.TotalMilliseconds / 7f;
            setGameMatchTime(_parentGameScreen.ThisGameType);

        }

        public void Update(bool StartTiming)
        {
            if (Moving)
            {
                _position.X -= FrameRateDirector.MovementFactor * _movementSpeed;
                if (_position.X <= 0)
                {
                    _position.X = 0;
                    Moving = false;
                }
            }
            if (StartTiming)
            {
                if (TimeRemaining > TimeSpan.Zero || _parentGameScreen.ThisGameType == GameType.Spree || _parentGameScreen.ThisGameType == GameType.Classic || _parentGameScreen.ThisGameType == GameType.Survival)
                {
                    CurrentGameTime += FrameRateDirector.MovementFactorTimeSpan;
                    TimeRemaining -= FrameRateDirector.MovementFactorTimeSpan;
                }

                if (TimeRemaining < TimeSpan.Zero)
                {
                    TimeRemaining = TimeSpan.Zero;
                }

                if (_parentGameScreen.ThisGameType == GameType.TimedSpree)
                {
                    _currentStep += FrameRateDirector.MovementFactor;

                    if (_currentStep >= _stepSize)
                    {
                        _currentStep -= _stepSize;
                        _parentGameScreen.ShortenBoard();
                    }

                }
            }
            setGameMatchTime(_parentGameScreen.ThisGameType);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("TopGameBoard").Texture, new Vector2(1280 - _resources.GetTexture("TopGameBoard").Width + _position.X, 0), Color.White);
            RenderGraph.Instance.RenderText(GameMatchTime.Minutes.ToString("00"), new Vector2(1181.5f + _position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            RenderGraph.Instance.RenderText(GameMatchTime.Seconds.ToString("00"), new Vector2(1219.5f + _position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            RenderGraph.Instance.RenderText(GameMatchTime.Milliseconds.ToString("00"), new Vector2(1257.5f + _position.X, 64), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopCenter, false);
            if (_parentGameScreen.ThisGameType == GameType.Classic || _parentGameScreen.ThisGameType == GameType.Spree || _parentGameScreen.ThisGameType == GameType.Survival)
            {
                RenderGraph.Instance.RenderText("Time Elapsed", new Vector2(_position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopRight, true);
            }
            else if (_parentGameScreen.ThisGameType == GameType.TimedSpree)
            {
                RenderGraph.Instance.RenderText("Time Remaining", new Vector2(_position.X + 1270, 30), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopRight, true);
            }
        }

        private void setGameMatchTime(GameType type)
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
    }
}
