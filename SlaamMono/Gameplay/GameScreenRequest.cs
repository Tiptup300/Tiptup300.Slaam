using SlaamMono.PlayerProfiles;
using System.Collections.Generic;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay
{
    public class GameScreenRequest : IRequest
    {
        public List<CharacterShell> SetupCharacters { get; private set; }

        public GameScreenRequest(List<CharacterShell> chars)
        {
            this.SetupCharacters = chars;
        }
    }
}
