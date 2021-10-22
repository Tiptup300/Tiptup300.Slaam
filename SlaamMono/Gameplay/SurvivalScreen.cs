using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Profiles;
using SlaamMono.SubClasses;
using System;
using System.Collections.Generic;

namespace SlaamMono.Screens
{
    class SurvivalScreen : GameScreen
    {
        private Timer _timeToAddBot = new Timer(new TimeSpan(0, 0, 10));
        private int _botsToAdd = 1;
        private int _botsAdded = 0;

        private readonly ILogger _logger;
        private readonly IScreenDirector _screenDirector;

        public SurvivalScreen(List<CharacterShell> shell, ILogger logger, IScreenDirector screenDirector)
            : base(shell, logger, screenDirector)
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

            Characters.Add(new Character(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SetupChars[0].SkinLocation) /*Texture2D.FromFile(Game1.Graphics.GraphicsDevice, SetupChars[0].SkinLocation)*/, SetupChars[0].CharProfile, new Vector2(-100, -100), InputComponent.Players[0], Color.White, 0));
            Scoreboards.Add(new GameScreenScoreboard(new Vector2(-250, 10), Characters[0], ThisGameType));
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

                        if (rand.Next(0, _botsAdded - 1) == _botsAdded)
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
            Characters.Add(new BotPlayer(
                SlaamGame.Content.Load<Texture2D>("content\\skins\\" + ClassicCharSelectScreen.ReturnRandSkin(_logger))//Texture2D.FromFile(Game1.Graphics.GraphicsDevice, CharSelectScreen.Instance.ReturnRandSkin())
                , ProfileManager.GetBotProfile(), new Vector2(-200, -200), this, Color.Black, Characters.Count));
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
                    DiImplementer.Instance.Get<ILogger>(),
                    DiImplementer.Instance.Get<MainMenuScreen>(),
                    DiImplementer.Instance.Get<IScreenDirector>()));
        }
    }
}
