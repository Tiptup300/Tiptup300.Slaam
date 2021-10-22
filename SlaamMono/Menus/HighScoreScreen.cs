using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.StatsBoards;
using SlaamMono.x_;

namespace SlaamMono.Menus
{
    public class HighScoreScreen : IScreen
    {
        public const int MAX_HIGHSCORES = 7;
        private readonly ILogger _logger;
        private readonly MainMenuScreen _menuScreen;
        private readonly IScreenManager _screenDirector;
        private SurvivalStatsBoard _statsboard;

        public HighScoreScreen(ILogger logger, MainMenuScreen menuScreen, IScreenManager screenDirector)
        {
            _logger = logger;
            _menuScreen = menuScreen;
            _screenDirector = screenDirector;
        }

        public void Open()
        {
            _statsboard = new SurvivalStatsBoard(null, new Rectangle(10, 68, GameGlobals.DRAWING_GAME_WIDTH - 20, GameGlobals.DRAWING_GAME_WIDTH - 20), new Color(0, 0, 0, 150), MAX_HIGHSCORES, _logger);
            BackgroundManager.SetRotation(.5f);
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
            _statsboard.CalculateStats();
            _statsboard.ConstructGraph(25);
        }

        public void Update()
        {
            if (InputComponent.Players[0].PressedAction2)
            {
                _screenDirector.ChangeTo(_menuScreen);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            _statsboard.MainBoard.Draw(batch);
        }

        public void Close()
        {

        }
    }
}
