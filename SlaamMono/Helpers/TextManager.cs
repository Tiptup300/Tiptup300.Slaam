using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SlaamMono
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public partial class TextManager : DrawableGameComponent
    {
        private SpriteBatch _batch;
        private List<TextEntry> _textToDraw = new List<TextEntry>();

        public TextManager(Game game)
            : base(game)
        {
            LoadContent();
        }

        protected override void LoadContent()
        {
            _batch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public void DrawString(SpriteFont fnt, Vector2 pos, String str, TextAlignment alignment, Color col)
        {
            _textToDraw.Add(new TextEntry(fnt, pos, str, alignment, col));
        }

        public override void Draw(GameTime gameTime)
        { 
            _batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);

            for (int x = 0; x < _textToDraw.Count; x++)
            {
                _batch.DrawString(_textToDraw[x].Fnt, _textToDraw[x].Str, _textToDraw[x].Pos, _textToDraw[x].Col);
            }

            _batch.End();

            _textToDraw.Clear();

            base.Draw(gameTime);
        }
    }
}