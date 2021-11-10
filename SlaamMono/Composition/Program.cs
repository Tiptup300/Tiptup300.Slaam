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
            IApp app;

            app = Di.Get<IApp>();
            app.Run();
        }

    }
}

