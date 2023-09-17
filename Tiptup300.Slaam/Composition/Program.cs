using Tiptup300.Slaam.Composition.x_;

namespace Tiptup300.Slaam.Composition;

static class Program
{

   /// <summary>
   /// The main entry point for the application.
   /// </summary>
   static void Main(string[] args)
   {
      var graphicsConfigurer =
      ServiceLocator.Instance.GetService<SlamGameConfigurer>();
      graphicsConfigurer.Run();
   }

}

