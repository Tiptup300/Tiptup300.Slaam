using SlaamMono.PlayerProfiles;
using System.Collections.Generic;

namespace SlaamMono.Gameplay
{
    public class GameScreenRequest
    {
        public List<CharacterShell> SetupCharacters { get; private set; }

        public GameScreenRequest(List<CharacterShell> chars)
        {
            this.SetupCharacters = chars;
        }
    }
}
