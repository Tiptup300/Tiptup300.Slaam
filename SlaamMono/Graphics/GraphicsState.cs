using Microsoft.Xna.Framework;
using SlaamMono.Library;
using System;

namespace SlaamMono.Composition
{
    public class GraphicsState : IGraphicsState
    {
        private Mut<GraphicsDeviceManager> _graphicsState = new Mut<GraphicsDeviceManager>();

        public GraphicsDeviceManager Get()
            => _graphicsState.Get();

        public void Set(GraphicsDeviceManager graphicsDeviceManager)
            => _graphicsState.Mutate(graphicsDeviceManager);

        public void ApplyChanges(Action<GraphicsDeviceManager> graphicsStateChanges)
        {
            graphicsStateChanges?.Invoke(_graphicsState.Get());
            _graphicsState.Get().ApplyChanges();
        }
    }
}
