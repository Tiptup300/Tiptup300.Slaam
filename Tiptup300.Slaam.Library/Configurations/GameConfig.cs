namespace Tiptup300.Slaam.Library.Configurations;

public class GameConfig
{
   public GameConfig(bool showFPS)
   {
      ShowFPS = showFPS;
   }

   public bool ShowFPS { get; private set; }
}
