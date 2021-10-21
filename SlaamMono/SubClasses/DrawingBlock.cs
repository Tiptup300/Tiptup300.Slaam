using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Resources;

namespace SlaamMono.SubClasses
{

    public class DrawingBlock
    {
        private Rectangle DrawingRectangle;
        private Color DrawingColor;

        public DrawingBlock(Rectangle drawingrect, Color drawingcol)
        {
            DrawingRectangle = drawingrect;
            DrawingColor = drawingcol;
        }

        public void ChangeColor(Color newcol)
        {
            DrawingColor = newcol;
        }

        public void Draw(SpriteBatch batch, Vector2 Offset)
        {

            batch.Draw(ResourceManager.WhitePixel, new Rectangle(DrawingRectangle.X + (int)Offset.X, DrawingRectangle.Y + (int)Offset.Y, DrawingRectangle.Width, DrawingRectangle.Height), DrawingColor);
        }

    }
}
