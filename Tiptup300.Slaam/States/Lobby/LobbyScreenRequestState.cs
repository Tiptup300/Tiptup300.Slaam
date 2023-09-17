using System.Collections.Generic;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.PlayerProfiles;

namespace Tiptup300.Slaam.States.Lobby;

public class LobbyScreenRequestState : IState
{
   public List<CharacterShell> CharacterShells { get; private set; }

   public LobbyScreenRequestState(List<CharacterShell> chars)
   {
      CharacterShells = chars;
   }
}
