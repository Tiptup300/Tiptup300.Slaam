using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Rendering.Text
{
    public interface ITextRenderer
    {
        void RenderText(string text, Vector2 position, SpriteFont font, Color color, TextAlignment alignment = TextAlignment.Default, bool addShadow = false);
    }
}