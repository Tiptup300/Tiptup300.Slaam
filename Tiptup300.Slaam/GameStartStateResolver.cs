using System.Tiptup300.Primitives;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.States.ZibithLogo;

namespace Tiptup300.Slaam;

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
