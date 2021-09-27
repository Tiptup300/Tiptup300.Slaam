using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Input;

namespace SlaamMono
{
    public class CharacterShell
    {
        #region Variables

        public string SkinLocation;
        public int CharProfile;
        public ExtendedPlayerIndex PlayerIDX;
        public PlayerType Type;
        public Color PlayerColor;

        #endregion

        #region Constructor

        public CharacterShell(String skinloc, int profile, ExtendedPlayerIndex idx, PlayerType type, Color col)
        {
            SkinLocation = skinloc;
            CharProfile = profile;
            PlayerIDX = idx;
            Type = type;
            PlayerColor = col;
        }

        #endregion
    }

    #region Enums

    public enum PlayerType
    {
        Player,
        Computer
    }

    #endregion
}
