using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SlaamMono.Input;

namespace SlaamMono
{
    class SurvivalScreen : GameScreen
    {

        private Timer _timeToAddBot = new Timer(new TimeSpan(0, 0, 10));
        private int _botsToAdd = 1;
        private int _botsAdded = 0;

        private readonly ILogger _logger; 

        public SurvivalScreen(List<CharacterShell> shell, ILogger logger)
            : base(shell)
        {
            _logger = logger;
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
            Scoreboards.Add(new GameScreenScoreboard(new Vector2(-250,10),Characters[0],ThisGameType));
        }

        public override void Update()
        {
            if (CurrentGameStatus == GameStatus.Playing)
            {
                _timeToAddBot.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_timeToAddBot.Active)
                {
                    for (int x = 0; x < _botsToAdd+1; x++)
                    {
                        AddNewBot();
                        _botsAdded++;

                        if (rand.Next(0, _botsAdded-1) == _botsAdded)
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
            if(Killer == 0)
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
                SlaamGame.Content.Load<Texture2D>("content\\skins\\" + CharSelectScreen.ReturnRandSkin(_logger))//Texture2D.FromFile(Game1.Graphics.GraphicsDevice, CharSelectScreen.Instance.ReturnRandSkin())
                , ProfileManager.GetBotProfile(), new Vector2(-200, -200), this, Color.Black, Characters.Count));
            ProfileManager.ResetAllBots();
            base.RespawnChar(Characters.Count - 1);
        }

        public override void EndGame()
        {
            if (ProfileManager.AllProfiles[Characters[0].ProfileIndex].BestGame < Timer.CurrentGameTime)
                ProfileManager.AllProfiles[Characters[0].ProfileIndex].BestGame = Timer.CurrentGameTime;
            ProfileManager.SaveProfiles();
            ScreenHelper.ChangeScreen(new StatsScreen(ScoreKeeper));
        }
    }

    public class SurvivalCharSelectScreen : CharSelectScreen
    {
        private ILogger _logger;

        public SurvivalCharSelectScreen(ILogger logger)
            : base(logger)
        {
            _logger = logger;
        }

        public override void ResetBoxes()
        {
            SelectBoxes = new CharSelectBox[1];
            SelectBoxes[0] = new CharSelectBox(new Vector2(340, 427), SkinTexture, ExtendedPlayerIndex.One, Skins);
            SelectBoxes[0].Survival = true;
        }

        public override void GoBack()
        {
            base.GoBack();
        }

        public override void GoForward()
        {
            List<CharacterShell> list = new List<CharacterShell>();
            list.Add(SelectBoxes[0].GetShell());
            GameScreen.Instance = new SurvivalScreen(list, _logger);
            ScreenHelper.ChangeScreen(GameScreen.Instance);
        }
    }
}
