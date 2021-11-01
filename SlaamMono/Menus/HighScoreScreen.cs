using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.StatsBoards;
using SlaamMono.x_;

namespace SlaamMono.Menus
{
    public class HighScoreScreen : IScreen
    {
        public const int MAX_HIGHSCORES = 7;
        private readonly ILogger _logger;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraph;
        private SurvivalStatsBoard _statsboard;

        public HighScoreScreen(ILogger logger, IScreenManager screenDirector, IResources resources, IRenderGraph renderGraph)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _renderGraph = renderGraph;
        }

        public void Open()
        {
            _statsboard = new SurvivalStatsBoard(null, new Rectangle(10, 68, GameGlobals.DRAWING_GAME_WIDTH - 20, GameGlobals.DRAWING_GAME_WIDTH - 20), new Color(0, 0, 0, 150), MAX_HIGHSCORES, _logger, _resources, _renderGraph);
            BackgroundManager.SetRotation(.5f);
            BackgroundManager.ChangeBG(BackgroundType.Menu);
            _statsboard.CalculateStats();
            _statsboard.ConstructGraph(25);
        }

        public void Update()
        {
            if (InputComponent.Players[0].PressedAction2)
            {
                _screenDirector.ChangeTo<IMainMenuScreen>();
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