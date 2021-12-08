using SlaamMono.Library;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class ScreenResolver : IResolver<ScreenRequest, IScreen>
    {
        private readonly x_Di _resolver;
        private readonly IResolver<ScreenNameLookup> _screenNameLookupResolver;

        public ScreenResolver(x_Di resolver, IResolver<ScreenNameLookup> screenNameLookupResolver)
        {
            _resolver = resolver;
            _screenNameLookupResolver = screenNameLookupResolver;
        }

        public IScreen Resolve(ScreenRequest request)
        {
            var screenLookups = _screenNameLookupResolver.Resolve();
            return (IScreen)_resolver.x_Get(screenLookups.ScreenNames[request.Name]);
        }
    }
}
