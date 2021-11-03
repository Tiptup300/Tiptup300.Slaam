﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Rendering
{
    internal struct TextEntry
    {
        public SpriteFont Fnt;
        public Vector2 Pos;
        public string Str;
        public RenderAlignment Alignment;
        public Color Col;

        public TextEntry(SpriteFont fnt, Vector2 pos, string str, RenderAlignment alignment, Color col)
        {
            Fnt = fnt;
            Pos = pos;
            Str = str;
            Alignment = alignment;
            Col = col;

            Vector2 size = fnt.MeasureString(str);
            Pos.Y -= size.Y / 2f;

            switch (alignment)
            {
                case RenderAlignment.Centered:
                    Pos.X -= size.X / 2f;
                    break;

                case RenderAlignment.Right:
                    Pos.X -= size.X;
                    break;
            }
        }
    }
}