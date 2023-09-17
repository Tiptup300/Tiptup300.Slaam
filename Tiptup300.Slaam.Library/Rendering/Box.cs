using Microsoft.Xna.Framework;

namespace Tiptup300.Slaam.Library.Rendering;

internal class Box
{
   public Alignment Alignment { get; private set; }
   public Rectangle Destination { get; private set; }
   public Color Color { get; private set; }

   public Box(Rectangle destination, Color color, Alignment alignment)
   {
      Destination = destination;
      Color = color;
      Alignment = alignment;
   }
}
