using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus;

 public class MainMenuRequestResolver : IResolver<MainMenuRequest, IState>
 {
     public IState Resolve(MainMenuRequest request)
     {
         return new MainMenuScreenState();
     }
 }
