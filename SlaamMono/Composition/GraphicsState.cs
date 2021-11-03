using Microsoft.Xna.Framework;
using SlaamMono.Library;
using System;

namespace SlaamMono.Composition
{
    public class GraphicsState : IGraphicsState
    {
        private State<GraphicsDeviceManager> _graphicsState = new State<GraphicsDeviceManager>();

        public GraphicsDeviceManager Get()
            => _graphicsState.Get();

        public void Set(GraphicsDeviceManager graphicsDeviceManager)
            => _graphicsState.Change(graphicsDeviceManager);

        public void ApplyChanges(Action<GraphicsDeviceManager> graphicsStateChanges)
        {
            graphicsStateChanges?.Invoke(_graphicsState.Get());
            _graphicsState.Get().ApplyChanges();
        }
    }
}
