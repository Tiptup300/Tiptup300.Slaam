using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    internal static class Helpers
    {
        internal static void DrawString(SpriteBatch batch, SpriteFont font, string str, Vector2 pos, Vector2 origin)
        {
            DrawString(batch, font, str, pos, origin, Color.White);
        }

        internal static void DrawString(SpriteBatch batch, SpriteFont font, string str, Vector2 pos, Vector2 origin, Color color)
        {
            Vector2 position = (pos - origin);
            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);
            batch.DrawString(font, str, position + new Vector2(1), new Color((byte)0, (byte)0, (byte)0, color.A));
            batch.DrawString(font, str, position, color);
        }
    }
}
