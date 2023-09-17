using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tiptup300.Slaam.Library.Graphing;

public class GraphDrawingBlockCollection : List<GraphDrawingBlock>
{
   public Vector2 Position;

   public GraphDrawingBlockCollection()
   {
   }

   public GraphDrawingBlockCollection(GraphDrawingBlock[] drawingblocks)
   {
      for (int x = 0; x < drawingblocks.Length; x++)
         Add(drawingblocks[x]);
   }

   public GraphDrawingBlockCollection(GraphDrawingBlock[] drawingblocks, Vector2 position)
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
