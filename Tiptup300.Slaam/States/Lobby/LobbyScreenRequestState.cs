using SlaamMono.PlayerProfiles;
using System.Collections.Generic;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation;

 public class LobbyScreenRequestState : IState
 {
     public List<CharacterShell> CharacterShells { get; private set; }

     public LobbyScreenRequestState(List<CharacterShell> chars)
     {
         CharacterShells = chars;
     }
 }
