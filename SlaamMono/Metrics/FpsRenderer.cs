using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Configurations;
using SlaamMono.Library.Metrics;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.Metrics
{
    public class FpsRenderer : IFpsRenderer
    {
        private bool _loaded = false;
        private string _fpsText;
        private Rectangle _boxDestination;
        private Vector2 _textPosition;
        private SpriteFont _font;

        private readonly Color _backBoxColor = new Color(0, 0, 0, 100);
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraph;
        private readonly GameConfig _gameConfig;

        public FpsRenderer(IResources resources, IRenderGraph renderGraph, GameConfig gameConfiguration)
        {
            _resources = resources;
            _renderGraph = renderGraph;
            _gameConfig = gameConfiguration;
        }

        public void Initialize()
        {

        }

        public void LoadContent()
        {
            if (_loaded)
            {
                return;
            }
            _font = _resources.GetFont("SegoeUIx32pt");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            _fpsText = FrameRateDirector.FUPS.ToString();
            Vector2 textSize = _font.MeasureString(_fpsText);
            _boxDestination = new Rectangle(0, 0, (int)textSize.X + 10, (int)textSize.Y);
            _textPosition = new Vector2(5, _boxDestination.Height / 2f);
        }
        public void Draw()
        {
            if (_gameConfig.ShowFPS)
            {
                _renderGraph.RenderBox(
                    destinationRectangle: _boxDestination,
                    color: _backBoxColor);

                RenderGraph.Instance.RenderText(
                    text: _fpsText,
                    position: _textPosition,
                    alignment: RenderAlignment.Top,
                    font: _font,
                    addShadow: true);
            }
        }
    }
}
