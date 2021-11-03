using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Rendering
{
    public interface IRenderGraph : IGameWidget
    {
        void RenderText(string text, Vector2 position, SpriteFont font, Color? color = null, RenderAlignment alignment = RenderAlignment.Default, bool addShadow = false);
        void RenderBox(Rectangle destinationRectangle, Color? color = null);
    }
}