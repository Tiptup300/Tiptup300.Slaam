using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
{

    public class DrawingBlockCollection : List<DrawingBlock>
    {
        public Vector2 Position;

        public DrawingBlockCollection()
        {
        }

        public DrawingBlockCollection(DrawingBlock[] drawingblocks)
        {
            for (int x = 0; x < drawingblocks.Length; x++)
                Add(drawingblocks[x]);
        }

        public DrawingBlockCollection(DrawingBlock[] drawingblocks, Vector2 position)
        {
            Position = position;
            for (int x = 0; x < drawingblocks.Length; x++)
                Add(drawingblocks[x]);
        }

        public void Draw(SpriteBatch batch)
        {
            for (int x = 0; x < Count; x++)
                this[x].Draw(batch,Position);
        }
    }

    public class DrawingBlock
    {
        #region Variables

        private Rectangle DrawingRectangle;
        private Color DrawingColor;

        #endregion

        #region Constructor

        public DrawingBlock(Rectangle drawingrect, Color drawingcol)
        {
            DrawingRectangle = drawingrect;
            DrawingColor = drawingcol;
        }

        public void ChangeColor(Color newcol)
        {
            DrawingColor = newcol;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch, Vector2 Offset)
        {

            batch.Draw(Resources.Dot, new Rectangle(DrawingRectangle.X + (int)Offset.X, DrawingRectangle.Y + (int)Offset.Y, DrawingRectangle.Width, DrawingRectangle.Height), DrawingColor);
        }

        #endregion

    }
}
