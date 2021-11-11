using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class GameScreenResolver : IRequest<GameScreenRequest, GameScreen>
    {
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphics;

        public GameScreenResolver(IScreenManager screenDirector, IResources resources, IGraphicsState graphics)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphics;
        }

        public GameScreen Execute(GameScreenRequest request)
        {
            GameScreen output;

            output = new GameScreen(_screenDirector, _resources, _graphics);
            output.Initialize(request);

            return output;
        }
    }
}
