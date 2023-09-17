using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.Configurations;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;

namespace Tiptup300.Slaam.Metrics;

public class FpsRenderer
{
   private bool _loaded = false;
   private string _fpsText;
   private Rectangle _boxDestination;
   private Vector2 _textPosition;
   private SpriteFont _font;

   private readonly Color _backBoxColor = new Color(0, 0, 0, 100);
   private readonly IResources _resources;
   private readonly IRenderService _renderGraph;
   private readonly GameConfig _gameConfig;
   private readonly FrameTimeService _frameTimeService;

   public FpsRenderer(
       IResources resources,
       IRenderService renderGraph,
       GameConfig gameConfiguration,
       FrameTimeService frameTimeService
       )
   {
      _resources = resources;
      _renderGraph = renderGraph;
      _gameConfig = gameConfiguration;
      _frameTimeService = frameTimeService;
   }

   public void Initialize()
   {
      _fpsText = "";
   }

   public void LoadContent()
   {
      if (_loaded)
      {
         return;
      }
      _font = _resources.GetFont("SegoeUIx14pt");
   }

   public void UnloadContent()
   {
   }

   public void Update()
   {
      _fpsText = _frameTimeService.GetLatestFrame().FUPS.ToString();
      Vector2 textSize = _font.MeasureString(_fpsText);
      _boxDestination = new Rectangle(0, 0, (int)textSize.X + 2, (int)textSize.Y);
      _textPosition = new Vector2(0, 0);
   }
   public void Draw()
   {
      if (_gameConfig.ShowFPS && _fpsText != null)
      {
         _renderGraph.RenderRectangle(
             destinationRectangle: _boxDestination,
             color: _backBoxColor);

         _renderGraph.RenderText(
             text: _fpsText,
             position: _textPosition,
             alignment: Alignment.TopLeft,
             font: _font,
             addShadow: true);
      }
   }
}
