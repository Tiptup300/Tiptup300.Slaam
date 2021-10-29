using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.ResourceManagement;
using SlaamMono.StatsBoards;
using SlaamMono.x_;

namespace SlaamMono.GameplayStatistics
{
    class StatsScreen : IScreen
    {
        public MatchScoreCollection ScoreCollection;
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraph;
        private IntRange CurrentPage = new IntRange(0, 0, 2);
        private IntRange CurrentChar;
        private StatsBoard PlayerStats;
        private StatsBoard Kills;
        private StatsBoard PvP;
        private CachedTexture[] _statsButtons = new CachedTexture[3];
        private Rectangle StatsRect = new Rectangle(20, 110, GameGlobals.DRAWING_GAME_WIDTH - 40, GameGlobals.DRAWING_GAME_HEIGHT);
        public const int MAX_HIGHSCORES = 5;
        private Color StatsCol = new Color(0, 0, 0, 125);

        public StatsScreen(MatchScoreCollection scorecollection, ILogger logger, IScreenManager screenDirector, IResources resources, IRenderGraph renderGraph)
        {
            ScoreCollection = scorecollection;
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _renderGraph = renderGraph;
        }

        public void Open()
        {
            _statsButtons = setStatsButtons();
            BackgroundManager.ChangeBG(BackgroundType.Menu);
            if (ScoreCollection.ParentGameScreen.ThisGameType == GameType.Classic)
            {
                PlayerStats = new NormalStatsBoard(ScoreCollection, StatsRect, StatsCol, _resources, _renderGraph);
            }
            else if (ScoreCollection.ParentGameScreen.ThisGameType == GameType.Spree || ScoreCollection.ParentGameScreen.ThisGameType == GameType.TimedSpree)
            {
                PlayerStats = new SpreeStatsBoard(ScoreCollection, StatsRect, StatsCol, _resources, _renderGraph);
            }
            else if (ScoreCollection.ParentGameScreen.ThisGameType == GameType.Survival)
            {
                PlayerStats = new SurvivalStatsBoard(ScoreCollection, StatsRect, StatsCol, MAX_HIGHSCORES, _logger, _resources, _renderGraph);
            }

            PlayerStats.CalculateStats();
            PlayerStats.ConstructGraph(0);

            if (ScoreCollection.ParentGameScreen.ThisGameType != GameType.Survival)
            {

                Kills = new KillsStatsBoard(ScoreCollection, StatsRect, StatsCol, _resources, _renderGraph);
                Kills.CalculateStats();
                Kills.ConstructGraph(0);

                PvP = new PvPStatsBoard(ScoreCollection, StatsRect, StatsCol, _resources, _renderGraph);
                PvP.CalculateStats();
                PvP.ConstructGraph(0);

                string first = "First: ",
                       second = "Second: ",
                       third = "Third: ";

                for (int x = 0; x < PlayerStats.MainBoard.Items.Count; x++)
                {
                    if (PlayerStats.MainBoard.Items[x].Details[1] == "First")
                        first += PlayerStats.MainBoard.Items[x].Details[0] + ", ";

                    if (PlayerStats.MainBoard.Items[x].Details[1] == "Second")
                        second += PlayerStats.MainBoard.Items[x].Details[0] + ", ";

                    if (PlayerStats.MainBoard.Items[x].Details[1] == "Third")
                        third += PlayerStats.MainBoard.Items[x].Details[0] + ", ";

                }

                if (second == "Second: ")
                    second = "";
                else
                    second = second.Substring(0, second.Length - 2);

                if (third == "Third: ")
                    third = "";
                else
                    third = third.Substring(0, third.Length - 2);

                FeedManager.InitializeFeeds(first.Substring(0, first.Length - 2) + " " + second + " " + third);

                CurrentChar = new IntRange(0, 0, PvP.MainBoard.Items.Count - 1);

            }
        }

        private CachedTexture[] setStatsButtons()
        {
            CachedTexture[] output;

            output = new CachedTexture[3];
            output[0] = ResourceManager.Instance.GetTexture("StatsButton1");
            output[1] = ResourceManager.Instance.GetTexture("StatsButton2");
            output[2] = ResourceManager.Instance.GetTexture("StatsButton3");

            return output;
        }

        public void Update()
        {
            BackgroundManager.SetRotation(1f);

            if (ScoreCollection.ParentGameScreen.ThisGameType != GameType.Survival)
            {

                if (InputComponent.Players[0].PressedLeft)
                {
                    CurrentPage.Sub(1);
                }

                if (InputComponent.Players[0].PressedRight)
                {
                    CurrentPage.Add(1);
                }

                if (CurrentPage.Value == 2)
                {
                    if (InputComponent.Players[0].PressedUp)
                    {
                        CurrentChar.Sub(1);
                        PvP.ConstructGraph(CurrentChar.Value);
                    }
                    else if (InputComponent.Players[0].PressedDown)
                    {
                        CurrentChar.Add(1);
                        PvP.ConstructGraph(CurrentChar.Value);
                    }
                }

            }

            if (InputComponent.Players[0].PressedAction)
            {
                _screenDirector.ChangeTo<IMainMenuScreen>();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 Statsboard = new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - ResourceManager.Instance.GetTexture("StatsBoard").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - ResourceManager.Instance.GetTexture("StatsBoard").Height / 2);
            //MainBG.Draw(batch);
            for (int x = 0; x < 3; x++)
            {
                batch.Draw(_statsButtons[x].Texture, Statsboard, x == CurrentPage.Value ? Color.LightSkyBlue : ScoreCollection.ParentGameScreen.ThisGameType == GameType.Survival ? Color.DarkGray : Color.White);
            }
            batch.Draw(ResourceManager.Instance.GetTexture("StatsBoard").Texture, Statsboard, Color.White);
            //DrawingButton.Draw(batch);
            if (CurrentPage.Value == 0)
                PlayerStats.MainBoard.Draw(batch);
            else if (CurrentPage.Value == 1)
                Kills.MainBoard.Draw(batch);
            else
                PvP.MainBoard.Draw(batch);

#if !ZUNE
            Resources.DrawString(ScoreCollection.ParentGameScreen.ThisGameType != GameType.Survival ? "Player Stats" : "Survival High Scores", new Vector2(131+Statsboard.X, 255), Resources.SegoeUIx14pt, TextAlignment.Centered, Color.White, true);
            Resources.DrawString("Kills", new Vector2(352+Statsboard.X, 255), Resources.SegoeUIx14pt, TextAlignment.Centered, Color.White, true);
            Resources.DrawString("Player Vs. Player", new Vector2(573+Statsboard.X, 255), Resources.SegoeUIx14pt, TextAlignment.Centered, Color.White, true);
#endif
        }

        public void Close()
        {

        }
    }
}
