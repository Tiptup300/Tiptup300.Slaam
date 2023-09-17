using SlaamMono.Menus.ZibithLogo;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono;

 public class GameStartStateResolver : IResolver<GameStartRequest, IState>
 {
     private readonly IResolver<ZibithLogoRequest, IState> _stateResolver;

     public GameStartStateResolver(IResolver<ZibithLogoRequest, IState> logoScreenResolver)
     {
         _stateResolver = logoScreenResolver;
     }

     public IState Resolve(GameStartRequest request)
     {
         return _stateResolver.Resolve(new ZibithLogoRequest());
     }
 }
