using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Slaam
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TextManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch batch;
        List<TextEntry> TextToDraw = new List<TextEntry>();

        public TextManager(Game game)
            : base(game)
        {
            LoadContent();
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void DrawString(SpriteFont fnt, Vector2 pos, String str, TextAlignment alignment, Color col)
        {
            TextToDraw.Add(new TextEntry(fnt, pos, str, alignment, col));
        }

        public override void Draw(GameTime gameTime)
        {
            //batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Matrix.Identity/* * Matrix.CreateScale(new Vector3(1f, 720 / 1024f, 1f))*/);

            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);

            for (int x = 0; x < TextToDraw.Count; x++)
                batch.DrawString(TextToDraw[x].Fnt, TextToDraw[x].Str, TextToDraw[x].Pos, TextToDraw[x].Col);
            batch.End();

            TextToDraw.Clear();

            base.Draw(gameTime);
        }

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
                Pos.Y -= size.Y/2f;

                switch(alignment)
                {
                    case TextAlignment.Centered:
                        Pos.X -= size.X/2f;
                        break;

                    case TextAlignment.Right:
                        Pos.X -= size.X;
                        break;
                }
            }
        }
    }

    public enum TextAlignment
    {
        VerticallyCentered,
        Top,
        Right,
        HorizontallyCentered,
        Centered,
        Default
    }
}