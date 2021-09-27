using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SlaamMono
{
    public partial class TextManager
    {
        public struct TextEntry
        {
            public SpriteFont Fnt;
            public Vector2 Pos;
            public String Str;
            public TextAlignment Alignment;
            public Color Col;

            public TextEntry(SpriteFont fnt, Vector2 pos, String str, TextAlignment alignment, Color col)
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
                    case TextAlignment.Centered:
                        Pos.X -= size.X / 2f;
                        break;

                    case TextAlignment.Right:
                        Pos.X -= size.X;
                        break;
                }
            }
        }
    }
}