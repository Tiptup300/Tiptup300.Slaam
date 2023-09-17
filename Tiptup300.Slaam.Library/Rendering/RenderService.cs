using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Tiptup300.Primitives;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.Library.Rendering;

public interface IRenderService
{
   void Render(Action<SpriteBatch> batch);
   void RenderText(string text, Vector2 position, SpriteFont font, Color? color = null, Alignment alignment = Alignment.TopLeft, bool addShadow = false);
   void RenderRectangle(Rectangle destinationRectangle, Color? color = null, Alignment alignment = Alignment.TopLeft);
}
public class RenderService : IRenderService
{
   public static IRenderService Instance;

   private SpriteBatch _batch;
   private List<TextEntry> _textToDraw = new List<TextEntry>();
   private List<Box> _boxesToDraw = new List<Box>();

   private readonly Color _shadowColor = new Color(0, 0, 0, 127);
   private readonly Vector2 _shadowOffset1 = new Vector2(1, 2);
   private readonly Vector2 _shadowOffset2 = new Vector2(2, 1);

   private readonly IResolver<WhitePixelRequest, Texture2D> _whitePixelResolver;
   private readonly IGraphicsStateService _graphicsState;

   public RenderService(
       IResolver<WhitePixelRequest, Texture2D> whitePixelResolver,
       IGraphicsStateService graphicsState)
   {
      _whitePixelResolver = whitePixelResolver;
      _graphicsState = graphicsState;
      Instance = this;
   }

   public void Initialize()
   {
      _batch = new SpriteBatch(_graphicsState.Get().GraphicsDevice);
   }

   public void RenderRectangle(Rectangle destinationRectangle, Color? color = null, Alignment alignment = Alignment.TopLeft)
   {
      _boxesToDraw.Add(
          new Box(
              destination: destinationRectangle,
              color: color.HasValue ? color.Value : Color.White,
              alignment: alignment));
   }

   public void RenderText(string text, Vector2 position, SpriteFont font, Color? color = null, Alignment alignment = Alignment.TopLeft, bool addShadow = false)
   {
      if (addShadow)
      {
         drawShadow(text, position, font, alignment);
      }
      _textToDraw.Add(new TextEntry(font, position, text, alignment, color.HasValue ? color.Value : Color.White));
   }

   private void drawShadow(string text, Vector2 position, SpriteFont font, Alignment alignment)
   {
      _textToDraw.Add(new TextEntry(font, position + _shadowOffset1, text, alignment, _shadowColor));
      _textToDraw.Add(new TextEntry(font, position + _shadowOffset2, text, alignment, _shadowColor));
   }

   public void Update()
   {
      _batch = new SpriteBatch(_graphicsState.Get().GraphicsDevice);
   }

   public void Draw()
   {
      _batch.Begin(
          sortMode: SpriteSortMode.Immediate,
          blendState: BlendState.AlphaBlend,
          transformMatrix: Matrix.Identity);
      _boxesToDraw.ForEach(box => drawBox(box));
      _textToDraw.ForEach(textLine => drawTextLine(textLine));
      _batch.End();
      _textToDraw.Clear();
      _boxesToDraw.Clear();
   }

   private void drawBox(Box box)
   {
      _batch.Draw(
          texture: _whitePixelResolver.Resolve(new WhitePixelRequest()),
          destinationRectangle: box.Destination,
          color: box.Color);
   }

   private void drawTextLine(TextEntry textLine)
   {
      _batch.DrawString(textLine.Font, textLine.Text, textLine.Position, textLine.Color);
   }

   public void LoadContent() { }
   public void UnloadContent() { }

   public void Render(Action<SpriteBatch> batch)
   {
      batch.Invoke(_batch);
   }
}