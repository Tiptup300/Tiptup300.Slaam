using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.Library.Graphing;

public class Graph
{
   public GraphItemCollection Items = new GraphItemCollection();

   private GraphDrawingBlockCollection _drawings = new GraphDrawingBlockCollection();
   private List<GraphWritingString> _stringsToWrite = new List<GraphWritingString>();
   private Color _colorToDraw;
   private Rectangle _graphRectangle;
   private int _gap;

   private readonly IResources _resourceManager;
   private readonly IRenderService _renderService;

   public Graph(Rectangle graphrect, int gap, Color coltodraw, IResources resourceManager, IRenderService renderGraphManager)
   {
      _graphRectangle = graphrect;
      _gap = gap;
      _colorToDraw = coltodraw;
      _resourceManager = resourceManager;
      _renderService = renderGraphManager;
   }

   public void CalculateBlocks()
   {
      _drawings.Clear();
      _stringsToWrite.Clear();
      int SizeOfColumns = _graphRectangle.Width / Items.Columns.Count;
      int HeightOfRows = 30;

      int ColumnWidth = SizeOfColumns - _gap;
      int RowHeight = HeightOfRows - _gap;

      int XOffset = ColumnWidth + _gap;
      int YOffset = RowHeight + _gap;

      for (int x = 0; x < Items.Columns.Count; x++)
      {
         Rectangle NewBlock = new Rectangle(_graphRectangle.X + XOffset * x, _graphRectangle.Y, ColumnWidth, RowHeight);

         if (Items.Columns[x].Trim() != "")
         {
            _drawings.Add(new GraphDrawingBlock(NewBlock, _colorToDraw, _renderService));

            _stringsToWrite.Add(new GraphWritingString(Items.Columns[x],
                new Vector2(NewBlock.X + NewBlock.Width / 2, NewBlock.Y + NewBlock.Height / 2)));
         }
      }

      for (int x = 0; x < Items.Count; x++)
      {
         for (int y = 0; y < Items.Columns.Count; y++)
         {
            if (Items[x].Details.Count >= Items.Columns.Count)
            {
               if (Items[x].Details[y].Trim() != "")
               {
                  Rectangle NewBlock = new Rectangle(_graphRectangle.X + XOffset * y, _graphRectangle.Y + YOffset * (1 + x), ColumnWidth, RowHeight);
                  if (Items[x].Highlight)
                     _drawings.Add(new GraphDrawingBlock(NewBlock, new Color((byte)135, (byte)206, (byte)250, _colorToDraw.A), _renderService));
                  else
                     _drawings.Add(new GraphDrawingBlock(NewBlock, _colorToDraw, _renderService));

                  _stringsToWrite.Add(new GraphWritingString(Items[x].Details[y],
                      new Vector2(NewBlock.X + NewBlock.Width / 2, NewBlock.Y + NewBlock.Height / 2)));
               }
            }
         }
      }
   }

   public void SetHighlight(int index)
   {
      for (int x = 0; x < Items.Count; x++)
         Items[x].Highlight = false;
      Items[index].Highlight = true;
      CalculateBlocks();
   }

   public void Draw(SpriteBatch batch)
   {
      _drawings.Draw(batch);
      for (int x = 0; x < _stringsToWrite.Count; x++)
      {
         _renderService.RenderText(_stringsToWrite[x].Str, _stringsToWrite[x].Pos, _resourceManager.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
      }
   }
}
