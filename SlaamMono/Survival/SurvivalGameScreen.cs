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
using System;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Survival
{
    public class SurvivalGameScreen : GameScreen
    {
        private SurvivalGameScreenState _survivalState = new SurvivalGameScreenState();

        private readonly ILogger _logger;
        private readonly IResolver<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;

        public SurvivalGameScreen(
            ILogger logger,
            IResources resources,
            IGraphicsState graphics,
            IResolver<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver)
            : base(resources, graphics, gameScreenScoreBoardResolver)
        {
            _logger = logger;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
        }

        protected override void SetupTheBoard(string BoardLoc)
        {
            _state.GameType = GameType.Survival;
            MatchSettings.CurrentMatchSettings.GameType = GameType.Survival;
            MatchSettings.CurrentMatchSettings.SpeedMultiplyer = 1f;
            MatchSettings.CurrentMatchSettings.RespawnTime = new TimeSpan(0, 0, 8);
            MatchSettings.CurrentMatchSettings.LivesAmt = 1;
            _state.Tileset = LobbyScreen.LoadQuickBoard();

            _state.Characters.Add(new CharacterActor(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + _state.SetupCharacters[0].SkinLocation) /*Texture2D.FromFile(Game1.Graphics.GraphicsDevice, SetupChars[0].SkinLocation)*/, _state.SetupCharacters[0].CharacterProfileIndex, new Vector2(-100, -100), InputComponent.Players[0], Color.White, 0, x_Di.Get<IResources>()));
            _state.Scoreboards.Add(
                _gameScreenScoreBoardResolver.Resolve(
                    new ScoreboardRequest(
                        new Vector2(-250, 10),
                        _state.Characters[0],
                        _state.GameType)));
        }

        public override IState Perform()
        {
            if (_state.CurrentGameStatus == GameStatus.Playing)
            {
                _survivalState.TimeToAddBot.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_survivalState.TimeToAddBot.Active)
                {
                    for (int x = 0; x < _survivalState.BotsToAdd + 1; x++)
                    {
                        AddNewBot(_state);
                        _survivalState.BotsAdded++;

                        if (_state.Rand.Next(0, _survivalState.BotsAdded - 1) == _survivalState.BotsAdded)
                        {
                            _survivalState.BotsToAdd++;
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
                AddNewBot(_state);
            }
            return base.Perform();
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

        private void AddNewBot(GameScreenState gameScreenState)
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
            RespawnCharacter(gameScreenState, _state.Characters.Count - 1);
        }

        protected override void EndGame()
        {
            if (ProfileManager.AllProfiles[_state.Characters[0].ProfileIndex].BestGame < _state.Timer.CurrentGameTime)
            {
                ProfileManager.AllProfiles[_state.Characters[0].ProfileIndex].BestGame = _state.Timer.CurrentGameTime;
            }
            ProfileManager.SaveProfiles();
            new StatsScreenRequestState(_state.ScoreKeeper, _state.GameType);
        }
    }
}
