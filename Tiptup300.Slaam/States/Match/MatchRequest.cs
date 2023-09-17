using System.Collections.Generic;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.Match.Misc;

namespace Tiptup300.Slaam.States.Match;

public class MatchRequest : IState
{
   public List<CharacterShell> SetupCharacters { get; private set; }
   public MatchSettings MatchSettings { get; private set; }

   public MatchRequest(List<CharacterShell> chars, MatchSettings matchSettings)
   {
      SetupCharacters = chars;
      MatchSettings = matchSettings;
   }
}
