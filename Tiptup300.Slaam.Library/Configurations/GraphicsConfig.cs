namespace Tiptup300.Slaam.Library.Configurations;

public class GraphicsConfig
{
   public int RenderWidth { get; private set; }
   public int RenderHeight { get; private set; }
   public bool MultiSampling { get; private set; }
   public bool IsFullScreen { get; private set; }

   public GraphicsConfig(int gameWidth, int gameHeight, bool multiSampling, bool isFullScreen)
   {
      RenderWidth = gameWidth;
      RenderHeight = gameHeight;
      MultiSampling = multiSampling;
      IsFullScreen = isFullScreen;
   }
}
