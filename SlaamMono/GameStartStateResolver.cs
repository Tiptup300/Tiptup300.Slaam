using SlaamMono.Menus;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono
{
    public class GameStartStateResolver : IResolver<GameStartRequest, IState>
    {
        private readonly IResolver<IRequest, IState> _stateResolver;

        public GameStartStateResolver(IResolver<IRequest, IState> stateResolver)
        {
            _stateResolver = stateResolver;
        }

        public IState Resolve(GameStartRequest request)
        {
            return _stateResolver.Resolve(new LogoScreenRequest());
        }
    }
}
