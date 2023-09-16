using SlaamMono.Composition.x_;

namespace SlaamMono.Composition
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var graphicsConfigurer = 
            ServiceLocator.GetService<SlamGameConfigurer>().Run();
        }

    }
}

