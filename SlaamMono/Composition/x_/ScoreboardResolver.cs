using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.Composition.x_
{
    public class ScoreboardResolver : IRequest<ScoreboardRequest, Scoreboard>
    {
        private readonly IRequest<WhitePixelRequest, Texture2D> _whitePixelResolver;
        private readonly IResources _resources;

        public ScoreboardResolver(IRequest<WhitePixelRequest, Texture2D> whitePixelResolver, IResources resources)
        {
            _whitePixelResolver = whitePixelResolver;
            _resources = resources;
        }

        public Scoreboard Execute(ScoreboardRequest request)
        {
            Scoreboard output;

            output = new Scoreboard(_resources, _whitePixelResolver);
            output.Initialize(request);

            return output;
        }
    }
}
