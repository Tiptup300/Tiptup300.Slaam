﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Rendering
{
    public interface IRenderService
    {
        void RenderText(string text, Vector2 position, SpriteFont font, Color? color = null, Alignment alignment = Alignment.TopLeft, bool addShadow = false);
        void RenderBox(Rectangle destinationRectangle, Color? color = null, Alignment alignment = Alignment.TopLeft);
    }
}