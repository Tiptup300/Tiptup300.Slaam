using SlaamMono.PlayerProfiles;
using System.Collections.Generic;
using ZzziveGameEngine;

namespace SlaamMono.MatchCreation
{
    public class LobbyScreenRequest : IRequest
    {
        public List<CharacterShell> CharacterShells { get; private set; }

        public LobbyScreenRequest(List<CharacterShell> chars)
        {
            CharacterShells = chars;
        }
    }
}
