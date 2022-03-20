using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SlaamMono.Library.Rendering
{
    public interface IRenderService
    {
        void Render(Action<SpriteBatch> batch);
        void RenderText(string text, Vector2 position, SpriteFont font, Color? color = null, Alignment alignment = Alignment.TopLeft, bool addShadow = false);
        void RenderRectangle(Rectangle destinationRectangle, Color? color = null, Alignment alignment = Alignment.TopLeft);
    }
}