using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.Rendering;

namespace Tiptup300.Slaam.Library.Graphing;


public class GraphDrawingBlock
{
   private Rectangle DrawingRectangle;
   private Color DrawingColor;
   private readonly IRenderService _renderGraphManager;

   public GraphDrawingBlock(Rectangle drawingrect, Color drawingcol, IRenderService renderGraphManager)
   {
      DrawingRectangle = drawingrect;
      DrawingColor = drawingcol;
      _renderGraphManager = renderGraphManager;
   }

   public void ChangeColor(Color newcol)
   {
      DrawingColor = newcol;
   }

   public void Draw(SpriteBatch batch, Vector2 Offset)
   {
      _renderGraphManager.RenderRectangle(
          new Rectangle(DrawingRectangle.X + (int)Offset.X, DrawingRectangle.Y + (int)Offset.Y, DrawingRectangle.Width, DrawingRectangle.Height),
          DrawingColor);
   }

}
