using SlaamMono.Library;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class ScreenResolver : IRequest<ScreenRequest, IScreen>
    {
        private readonly x_Di _resolver;
        private readonly IRequest<ScreenNameLookup> _screenNameLookupResolver;

        public ScreenResolver(x_Di resolver, IRequest<ScreenNameLookup> screenNameLookupResolver)
        {
            _resolver = resolver;
            _screenNameLookupResolver = screenNameLookupResolver;
        }

        public IScreen Execute(ScreenRequest request)
            => (IScreen)_resolver.x_Get(_screenNameLookupResolver.Execute().ScreenNames[request.Name]);
    }
}
