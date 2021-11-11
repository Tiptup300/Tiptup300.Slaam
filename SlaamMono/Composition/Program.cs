using SlaamMono.Composition.x_;
using SlaamMono.Library;

namespace SlaamMono.Composition
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            x_Di.Get<IApp>().Run();
        }

    }
}

