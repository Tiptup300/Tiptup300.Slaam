using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition;
using SlaamMono.Gameplay.Actors;
using SlaamMono.GameplayStatistics;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using SlaamMono.Menus;
using SlaamMono.PlayerProfiles;
using SlaamMono.SubClasses;
using System;
using System.Collections.Generic;

namespace SlaamMono.Gameplay
{
    class SurvivalScreen : GameScreen
    {
        private Timer _timeToAddBot = new Timer(new TimeSpan(0, 0, 10));
        private int _botsToAdd = 1;
        private int _botsAdded = 0;

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;

        public SurvivalScreen(List<CharacterShell> shell, ILogger logger, IScreenManager screenDirector, IResources resources, IGraphicsState graphics)
            : base(shell, screenDirector, resources, graphics)
        {
            _logger = logger;
            _screenDirector = screenDirector;
        }

        public override void SetupTheBoard(string BoardLoc)
        {
            ThisGameType = GameType.Survival;
            CurrentMatchSettings.GameType = GameType.Survival;
            CurrentMatchSettings.SpeedMultiplyer = 1f;
            CurrentMatchSettings.RespawnTime = new TimeSpan(0, 0, 8);
            CurrentMatchSettings.LivesAmt = 1;
            Tileset = LobbyScreen.LoadQuickBoard();

            Characters.Add(new CharacterActor(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SetupChars[0].SkinLocation) /*Texture2D.FromFile(Game1.Graphics.GraphicsDevice, SetupChars[0].SkinLocation)*/, SetupChars[0].CharProfile, new Vector2(-100, -100), InputComponent.Players[0], Color.White, 0, x_Di.Get<IResources>()));
            Scoreboards.Add(
                new GameScreenScoreboard(
                    new Vector2(-250, 10),
                    Characters[0],
                    ThisGameType,
                    x_Di.Get<IResources>(),
                    x_Di.Get<IWhitePixelResolver>()));
        }

        public override void Update()
        {
            if (CurrentGameStatus == GameStatus.Playing)
            {
                _timeToAddBot.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_timeToAddBot.Active)
                {
                    for (int x = 0; x < _botsToAdd + 1; x++)
                    {
                        AddNewBot();
                        _botsAdded++;

                        if (Rand.Next(0, _botsAdded - 1) == _botsAdded)
                        {
                            _botsToAdd++;
                        }
                    }
                }

                for (int x = 0; x < Characters.Count; x++)
                {
                    if (Characters[x] != null && Characters[x].Lives == 0)
                    {
                        Characters[x] = null;
                        NullChars++;
                    }
                }
            }

            bool temp = CurrentGameStatus == GameStatus.Waiting;

            base.Update();

            if (CurrentGameStatus == GameStatus.Playing && temp)
                AddNewBot();
        }

        public override void ReportKilling(int Killer, int Killee)
        {
            if (Killer == 0)
                Characters[Killer].Kills++;

            if (Killee == 0)
            {
                CurrentGameStatus = GameStatus.Over;
                ReadySetGoPart = 3;
                ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            }

            //base.ReportKilling(Killer, Killee);
        }

        private void AddNewBot()
        {
            Characters.Add(new BotActor(
                SlaamGame.Content.Load<Texture2D>("content\\skins\\" + ClassicCharSelectScreen.ReturnRandSkin(_logger))//Texture2D.FromFile(Game1.Graphics.GraphicsDevice, CharSelectScreen.Instance.ReturnRandSkin())
                , ProfileManager.GetBotProfile(), new Vector2(-200, -200), this, Color.Black, Characters.Count, x_Di.Get<IResources>()));
            ProfileManager.ResetAllBots();
            RespawnChar(Characters.Count - 1);
        }

        public override void EndGame()
        {
            if (ProfileManager.AllProfiles[Characters[0].ProfileIndex].BestGame < Timer.CurrentGameTime)
                ProfileManager.AllProfiles[Characters[0].ProfileIndex].BestGame = Timer.CurrentGameTime;
            ProfileManager.SaveProfiles();
            _screenDirector.ChangeTo(
                new StatsScreen(
                    ScoreKeeper,
                    x_Di.Get<ILogger>(),
                    x_Di.Get<IScreenManager>(),
                    x_Di.Get<IResources>(),
                    x_Di.Get<IRenderGraph>()));
        }
    }
}
