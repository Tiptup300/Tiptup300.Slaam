using SlaamMono.Menus;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono
{
    public class GameStartStateResolver : IResolver<GameStartRequest, IState>
    {
        private readonly IResolver<LogoScreenRequest, IState> _stateResolver;

        public GameStartStateResolver(IResolver<LogoScreenRequest, IState> logoScreenResolver)
        {
            _stateResolver = logoScreenResolver;
        }

        public IState Resolve(GameStartRequest request)
        {
            return _stateResolver.Resolve(new LogoScreenRequest());
        }
    }
}
