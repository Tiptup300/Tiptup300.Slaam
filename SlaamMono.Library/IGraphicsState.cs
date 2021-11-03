using Microsoft.Xna.Framework;
using System;

namespace SlaamMono.Library
{
    public interface IGraphicsState
    {
        void ApplyChanges(Action<GraphicsDeviceManager> graphicsStateChanges);
        GraphicsDeviceManager Get();
        void Set(GraphicsDeviceManager graphicsDeviceManager);
    }
}