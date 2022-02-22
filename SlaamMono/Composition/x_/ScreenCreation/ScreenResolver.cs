using SlaamMono.Library.Screens;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class ScreenResolver : IResolver<ScreenRequest, IStateUpdater>
    {
        private readonly x_Di _resolver;
        private readonly IResolver<ScreenNameLookup> _screenNameLookupResolver;

        public ScreenResolver(x_Di resolver, IResolver<ScreenNameLookup> screenNameLookupResolver)
        {
            _resolver = resolver;
            _screenNameLookupResolver = screenNameLookupResolver;
        }

        public IStateUpdater Resolve(ScreenRequest request)
        {
            var screenLookups = _screenNameLookupResolver.Resolve();
            return (IStateUpdater)_resolver.x_Get(screenLookups.ScreenNames[request.Name]);
        }
    }
}
