using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Screens;

namespace SlaamMono
{
    public class HighScoreScreen : IScreen
    {
#if !ZUNE
        public const int MAX_HIGHSCORES = 25;
#else
        public const int MAX_HIGHSCORES = 7;
        private readonly MainMenuScreen _menuScreen;
#endif
        private SurvivalStatsBoard _statsboard;

        public HighScoreScreen(ILogger logger, MainMenuScreen menuScreen)
        {
            _statsboard = new SurvivalStatsBoard(null, new Rectangle(10, 68, GameGlobals.DRAWING_GAME_WIDTH - 20, GameGlobals.DRAWING_GAME_WIDTH - 20), new Color(0, 0, 0, 150), MAX_HIGHSCORES, logger);
            BackgroundManager.SetRotation(.5f);
            _menuScreen = menuScreen;
        }

        public void Initialize()
        {
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
            _statsboard.CalculateStats();
            _statsboard.ConstructGraph(25);
        }

        public void Update()
        {
            if (InputComponent.Players[0].PressedAction2)
            {
                ScreenHelper.ChangeScreen(_menuScreen);
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
