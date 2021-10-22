using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Rendering.Text;

namespace SlaamMono.Library.Rendering
{
    public interface IRenderGraphManager
    {
        void RenderText(string text, Vector2 position, SpriteFont font, Color color, TextAlignment alignment = TextAlignment.Default, bool addShadow = false);
        void RenderBox(Rectangle destinationRectangle, Color? color = null);
    }
}