using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tiptup300.Slaam.Library.Rendering;

internal class TextEntry
{
   public SpriteFont Font { get; private set; }
   public Vector2 Position { get; private set; }
   public string Text { get; private set; }
   public Alignment Alignment { get; private set; }
   public Color Color { get; private set; }

   public TextEntry(SpriteFont font, Vector2 position, string text, Alignment alignment, Color color)
   {
      Font = font;
      Position = position;
      Text = text;
      Alignment = alignment;
      Color = color;
   }
}