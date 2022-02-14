using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using SlaamMono.PlayerProfiles;
using SlaamMono.SubClasses;
using System;
using ZzziveGameEngine;

namespace SlaamMono.Survival
{
    public class SurvivalGameScreen : GameScreen
    {
        private Timer _timeToAddBot = new Timer(new TimeSpan(0, 0, 10));
        private int _botsToAdd = 1;
        private int _botsAdded = 0;

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResolver<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;
        private readonly IResolver<StatsScreenRequest, StatsScreen> _statsScreenResolver;

        public SurvivalGameScreen(
            ILogger logger,
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphics,
            IResolver<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver,
            IResolver<StatsScreenRequest, StatsScreen> statsScreenResolver)
            : base(screenDirector, resources, graphics, gameScreenScoreBoardResolver, statsScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
            _statsScreenResolver = statsScreenResolver;
        }

        public override void SetupTheBoard(string BoardLoc)
        {
            _state.ThisGameType = GameType.Survival;
            CurrentMatchSettings.GameType = GameType.Survival;
            CurrentMatchSettings.SpeedMultiplyer = 1f;
            CurrentMatchSettings.RespawnTime = new TimeSpan(0, 0, 8);
            CurrentMatchSettings.LivesAmt = 1;
            _state.Tileset = LobbyScreen.LoadQuickBoard();

            _state.Characters.Add(new CharacterActor(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + _state.SetupCharacters[0].SkinLocation) /*Texture2D.FromFile(Game1.Graphics.GraphicsDevice, SetupChars[0].SkinLocation)*/, _state.SetupCharacters[0].CharProfile, new Vector2(-100, -100), InputComponent.Players[0], Color.White, 0, x_Di.Get<IResources>()));
            _state.Scoreboards.Add(
                _gameScreenScoreBoardResolver.Resolve(
                    new ScoreboardRequest(
                        new Vector2(-250, 10),
                        _state.Characters[0],
                        _state.ThisGameType)));
        }

        public override void UpdateState()
        {
            if (_state.CurrentGameStatus == GameStatus.Playing)
            {
                _timeToAddBot.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_timeToAddBot.Active)
                {
                    for (int x = 0; x < _botsToAdd + 1; x++)
                    {
                        AddNewBot();
                        _botsAdded++;

                        if (_state.Rand.Next(0, _botsAdded - 1) == _botsAdded)
                        {
                            _botsToAdd++;
                        }
                    }
                }

                for (int x = 0; x < _state.Characters.Count; x++)
                {
                    if (_state.Characters[x] != null && _state.Characters[x].Lives == 0)
                    {
                        _state.Characters[x] = null;
                        _state.NullChars++;
                    }
                }
            }

            bool temp = _state.CurrentGameStatus == GameStatus.Waiting;
            if (_state.CurrentGameStatus == GameStatus.Playing && temp)
            {
                AddNewBot();
            }
            base.UpdateState();
        }

        public override void ReportKilling(int Killer, int Killee)
        {
            if (Killer == 0)
            {
                _state.Characters[Killer].Kills++;
            }

            if (Killee == 0)
            {
                _state.CurrentGameStatus = GameStatus.Over;
                _state.ReadySetGoPart = 3;
                _state.ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            }
        }

        private void AddNewBot()
        {
            _state.Characters.Add(
                new BotActor(
                    SlaamGame.Content.Load<Texture2D>("content\\skins\\" + CharacterSelectionScreen.ReturnRandSkin(_logger)),
                    ProfileManager.GetBotProfile(),
                    new Vector2(-200, -200),
                    this,
                    Color.Black,
                    _state.Characters.Count,
                    x_Di.Get<IResources>()));

            ProfileManager.ResetAllBots();
            RespawnChar(_state.Characters.Count - 1);
        }

        public override void EndGame()
        {
            if (ProfileManager.AllProfiles[_state.Characters[0].ProfileIndex].BestGame < _state.Timer.CurrentGameTime)
            {
                ProfileManager.AllProfiles[_state.Characters[0].ProfileIndex].BestGame = _state.Timer.CurrentGameTime;
            }
            ProfileManager.SaveProfiles();
            _screenDirector.ChangeTo(
                _statsScreenResolver.Resolve(new StatsScreenRequest(_state.ScoreKeeper)));
        }
    }
}
