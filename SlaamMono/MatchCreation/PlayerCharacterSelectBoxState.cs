using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using System.Collections.Generic;

namespace SlaamMono.MatchCreation
{
    public class PlayerCharacterSelectBoxState
    {
        public float ScrollSpeed = 4f / 35f;
        public List<string> ParentSkinStrings = new List<string>();
        public int PlayerIndex;
        public float Offset;
        public IntRange ChosenSkin = new IntRange(0);
        public IntRange ChosenProfile;
        public PlayerCharacterSelectBoxMovementStatus MovementStatus = PlayerCharacterSelectBoxMovementStatus.Stationary;
        public Texture2D[] DisplayResources = new Texture2D[3];
        public Texture2D[] ParentCharSkins;
        public Vector2[] Positions = new Vector2[10];
        public string[] MessageLines = new string[6];
        public bool Survival = false;
        public PlayerCharacterSelectBoxStatus Status = PlayerCharacterSelectBoxStatus.Computer;
    }
}
