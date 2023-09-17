using System.Tiptup300.Primitives;
using Tiptup300.Slaam.States.Lobby;

namespace Tiptup300.Slaam.States.BoardSelect;

public class BoardSelectionScreenRequest : IRequest
{
   public LobbyScreenState ParentScreen { get; private set; }

   public BoardSelectionScreenRequest(LobbyScreenState parentScreen)
   {
      ParentScreen = parentScreen;
   }
}
