using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Graphing;
using SlaamMono.PlayerProfiles;
using System.Collections.Generic;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class LobbyScreenState : IState
    {
        public Texture2D CurrentBoardTexture { get; set; }
        public int PlayerAmt { get; set; }
        public string[] Dialogs { get; set; }
        public string BoardLocation { get; set; }
        public bool ViewingSettings { get; set; }
        public IntRange MenuChoice { get; set; }

        public Graph MainMenu { get; set; }
        public List<CharacterShell> SetupCharacters { get; set; }
    }
}
