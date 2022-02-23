using Microsoft.Xna.Framework;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;

namespace SlaamMono.PlayerProfiles
{
    public class CharacterShell
    {
        public string SkinLocation;
        public int CharacterProfileIndex;
        public ExtendedPlayerIndex PlayerIndex;
        public PlayerType PlayerType;
        public Color PlayerColor;

        public CharacterShell(
            string skinLocation,
            int characterProfileIndex,
            ExtendedPlayerIndex playerIndex,
            PlayerType playerType,
            Color playerColor)
        {
            SkinLocation = skinLocation;
            CharacterProfileIndex = characterProfileIndex;
            PlayerIndex = playerIndex;
            PlayerType = playerType;
            PlayerColor = playerColor;
        }
    }
}
