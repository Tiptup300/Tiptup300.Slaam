using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tiptup300.Slaam.Library.Widgets;

namespace Tiptup300.Slaam.States.CharacterSelect.CharacterSelectBoxes;

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
   public bool IsSurvival = false;
   public PlayerCharacterSelectBoxStatus Status = PlayerCharacterSelectBoxStatus.Computer;
}
