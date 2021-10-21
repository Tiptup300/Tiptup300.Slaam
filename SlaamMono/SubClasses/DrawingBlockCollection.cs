using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SlaamMono.SubClasses
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
                this[x].Draw(batch, Position);
        }
    }
}
