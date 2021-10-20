using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SlaamMono.Library.Drawing.Text
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TextManager : DrawableGameComponent, ITextRenderer
    {
        private SpriteBatch _batch;
        private List<TextEntry> _textToDraw = new List<TextEntry>();

        private readonly Color _shadowColor = new Color(0, 0, 0, 127);
        private readonly Vector2 _shadowOffset1 = new Vector2(1, 2);
        private readonly Vector2 _shadowOffset2 = new Vector2(2, 1);

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

        public void AddTextToRender(string text, Vector2 position, SpriteFont font, Color color, TextAlignment alignment = TextAlignment.Default, bool addShadow = false)
        {
            _textToDraw.Add(new TextEntry(font, position, text, alignment, color));

            if (addShadow)
            {
                drawShadow(text, position, font, alignment);
            }
        }

        private void drawShadow(string text, Vector2 position, SpriteFont font, TextAlignment alignment)
        {
            _textToDraw.Add(new TextEntry(font, position + _shadowOffset1, text, alignment, _shadowColor));
            _textToDraw.Add(new TextEntry(font, position + _shadowOffset2, text, alignment, _shadowColor));
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