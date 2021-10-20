using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Drawing.Text
{
    public interface ITextRenderer
    {
        void AddTextToRender(string text, Vector2 position, SpriteFont font, Color color, TextAlignment alignment = TextAlignment.Default, bool addShadow = false);
    }
}