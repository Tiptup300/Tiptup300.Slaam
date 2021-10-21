using Microsoft.Xna.Framework;
using SlaamMono.Library.Input;
using System;

namespace SlaamMono.SubClasses
{
    public class CharacterShell
    {
        public string SkinLocation;
        public int CharProfile;
        public ExtendedPlayerIndex PlayerIDX;
        public PlayerType Type;
        public Color PlayerColor;

        public CharacterShell(string skinloc, int profile, ExtendedPlayerIndex idx, PlayerType type, Color col)
        {
            SkinLocation = skinloc;
            CharProfile = profile;
            PlayerIDX = idx;
            Type = type;
            PlayerColor = col;
        }
    }

    public enum PlayerType
    {
        Player,
        Computer
    }
}
