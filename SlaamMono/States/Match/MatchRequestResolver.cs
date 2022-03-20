using Microsoft.Xna.Framework;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;
using ZBlade;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.States.Match
{
    public class MatchRequestResolver : IResolver<MatchRequest, IState>
    {
        private readonly IResources _resources;
        private readonly ITimeSer

        public IState Resolve(MatchRequest request)
        {
            MatchState _state = new MatchState();

            _state.SetupCharacters = request.SetupCharacters;
            _state.CurrentMatchSettings = request.MatchSettings;


            _state.PowerupTime = new Timer(new TimeSpan(0, 0, 0, 15));
            _state.ReadySetGoThrottle = new Timer(new TimeSpan(0, 0, 0, 0, 325));
            _state.Tiles = new Tile[GameGlobals.BOARD_WIDTH, GameGlobals.BOARD_HEIGHT];
            _state.CurrentGameStatus = GameStatus.Waiting;
            _state.GameType = _state.CurrentMatchSettings.GameType;
            SetupTheBoard(_state.CurrentMatchSettings.BoardLocation);
            _state.CurrentGameStatus = GameStatus.MovingBoard;
            _resources.GetTexture("ReadySetGo").Load();
            _resources.GetTexture("BattleBG").Load();

            _state.Boardpos = new Vector2(calcFinalBoardPosition().X, -_state.Tileset.Height);

            _state.Timer = new MatchTimer(
                x_Di.Get<IResources>(),
                new Vector2(1024, 0),
                _state.CurrentMatchSettings, _frameTimeService);

            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                {
                    _state.Tiles[x, y] = new Tile(
                        _state.Boardpos,
                        new Vector2(x, y),
                        _state.Tileset,
                        x_Di.Get<IResources>(),
                        x_Di.Get<IRenderService>(),
                        _frameTimeService);
                }
            }
            _state.ScoreKeeper = new MatchScoreCollection(this, _state.GameType, _state);
            _state.ReadySetGoThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            if (_state.GameType == GameType.Classic)
            {
                _state.StepsRemaining = _state.SetupCharacters.Count - 1;
            }
            else if (_state.GameType == GameType.TimedSpree)
            {
                _state.StepsRemaining = 7;
            }
            else if (_state.GameType == GameType.Spree)
            {
                _state.StepsRemaining = 100;
                _state.KillsToWin = _state.CurrentMatchSettings.KillsToWin;
                _state.SpreeStepSize = 10;
                _state.SpreeCurrentStep = 0;
            }

            setupPauseMenu(_state);

        }
        private void setupPauseMenu(MatchState _state)
        {
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;
            MenuTextItem resume = new MenuTextItem("Resume Game");
            resume.Activated += delegate
            {
                _state.IsPaused = false;
                SlaamGame.mainBlade.Status = BladeStatus.Hidden;
            };
            MenuTextItem quit = new MenuTextItem("Quit Game");

            quit.Activated += delegate
            {
                SlaamGame.mainBlade.Status = BladeStatus.Hidden;
                _state.EndGameSelected = true;
            };

            _state.main.Nodes.Add(resume);
            _state.main.Nodes.Add(quit);

            SlaamGame.mainBlade.TopMenu = _state.main;
        }
    }
}
