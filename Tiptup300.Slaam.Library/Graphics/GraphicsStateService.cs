namespace Tiptup300.Slaam.Library.Graphics;

using Microsoft.Xna.Framework;
using System;
using System.Tiptup300.Primitives;

public interface IGraphicsStateService
{
   void ApplyChanges(Action<GraphicsDeviceManager> graphicsStateChanges);
   GraphicsDeviceManager Get();
   void Set(GraphicsDeviceManager graphicsDeviceManager);
}
public class GraphicsStateService : IGraphicsStateService
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
