using SlaamMono.Library.Logging;
using System;

namespace SlaamMono
{
    public class SlamGameConfigurer
    {
        private readonly IGraphicsConfigurer _graphicsConfigurer;
        private readonly SlaamGame _game;

        public SlamGameConfigurer(SlaamGame game, ILogger logger, IGraphicsConfigurer graphicsConfigurer)
        {
            _game = game;
            _graphicsConfigurer = graphicsConfigurer;
        }

        public void Run()
        {
            _graphicsConfigurer.ConfigureGraphics();
            _game.Run();
        }
    }
}
