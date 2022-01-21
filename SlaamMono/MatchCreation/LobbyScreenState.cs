using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;

namespace SlaamMono.MatchCreation
{
    public class LobbyScreenState
    {
        public Texture2D CurrentBoardTexture { get; set; }
        public int PlayerAmt { get; set; }
        public string[] Dialogs { get; set; }
        public string CurrentBoardLocation { get; set; }
        public bool ViewingSettings { get; set; }
        public IntRange MenuChoice { get; set; }
    }
}
