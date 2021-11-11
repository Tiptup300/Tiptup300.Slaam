using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.Composition.x_
{
    public class GameScreenScoreboardResolver : IRequest<GameScreenScoreboardRequest, GameScreenScoreboard>
    {
        private readonly IRequest<WhitePixelRequest, Texture2D> _whitePixelResolver;
        private readonly IResources _resources;

        public GameScreenScoreboardResolver(IRequest<WhitePixelRequest, Texture2D> whitePixelResolver, IResources resources)
        {
            _whitePixelResolver = whitePixelResolver;
            _resources = resources;
        }

        public GameScreenScoreboard Execute(GameScreenScoreboardRequest request)
        {
            GameScreenScoreboard output;

            output = new GameScreenScoreboard(_resources, _whitePixelResolver);
            output.Initialize(request);

            return output;
        }
    }
}
