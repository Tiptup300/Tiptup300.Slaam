using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SlaamMono.Library.Drawing.Text
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TextManager : DrawableGameComponent
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

        public void DrawText(SpriteFont fnt, Vector2 pos, string str, TextAlignment alignment, Color col)
        {
            _textToDraw.Add(new TextEntry(fnt, pos, str, alignment, col));
        }

        public override void Draw(GameTime gameTime)
        {
            _batch.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.AlphaBlend,
                transformMatrix: Matrix.Identity);

            _textToDraw.ForEach(textLine => drawTextLine(textLine));

            _batch.End();

            _textToDraw.Clear();

            base.Draw(gameTime);
        }

        private void drawTextLine(TextEntry textLine)
        {
            _batch.DrawString(textLine.Fnt, textLine.Str, textLine.Pos, textLine.Col);
        }
    }
}