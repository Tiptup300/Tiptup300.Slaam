using SlaamMono.Library;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class ScreenResolver : IRequest<ScreenRequest, IScreen>
    {
        private readonly IResolver _resolver;
        private readonly IRequest<ScreenNameLookup> _screenNameLookupResolver;

        public ScreenResolver(IResolver resolver, IRequest<ScreenNameLookup> screenNameLookupResolver)
        {
            _resolver = resolver;
            _screenNameLookupResolver = screenNameLookupResolver;
        }

        public IScreen Execute(ScreenRequest request)
            => (IScreen)_resolver.x_Get(_screenNameLookupResolver.Execute().ScreenNames[request.Name]);
    }
}
