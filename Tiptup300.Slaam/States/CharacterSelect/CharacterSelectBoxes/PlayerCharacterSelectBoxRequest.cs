using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Tiptup300.Primitives;
using Tiptup300.Slaam.Library.Input;

namespace Tiptup300.Slaam.States.CharacterSelect.CharacterSelectBoxes;

public class PlayerCharacterSelectBoxRequest : IRequest
{
   public Vector2 Position { get; set; }
   public Texture2D[] parentcharskins { get; set; }
   public ExtendedPlayerIndex playeridx { get; set; }
   public List<string> parentskinstrings { get; set; }
   public bool IsSurvival { get; set; }
}
