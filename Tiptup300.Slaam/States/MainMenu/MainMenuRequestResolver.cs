using System.Tiptup300.Primitives;
using System.Tiptup300.StateManagement;

namespace Tiptup300.Slaam.States.MainMenu;

public class MainMenuRequestResolver : IResolver<MainMenuRequest, IState>
{
   public IState Resolve(MainMenuRequest request)
   {
      return new MainMenuScreenState();
   }
}
