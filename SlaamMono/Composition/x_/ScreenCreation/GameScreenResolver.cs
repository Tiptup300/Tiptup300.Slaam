using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class GameScreenResolver : IResolver<GameScreenRequest, GameScreen>
    {
        private readonly GameScreen _gameScreen;

        public GameScreenResolver(GameScreen gameScreen)
        {
            _gameScreen = gameScreen;
        }

        public GameScreen Resolve(GameScreenRequest request)
        {
            GameScreen output;

            output = _gameScreen;
            output.Initialize(request);

            return output;
        }
    }
}
