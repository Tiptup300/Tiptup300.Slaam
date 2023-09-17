using Microsoft.Xna.Framework;

namespace Tiptup300.Slaam.Library.Graphing;

public struct GraphWritingString
{
   public string Str;
   public Vector2 Pos;

   public GraphWritingString(string str, Vector2 pos)
   {
      Str = str;
      Pos = pos;
      Pos.Y += 3;
   }
}
