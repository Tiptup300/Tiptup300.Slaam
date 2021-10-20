using Microsoft.Xna.Framework;

namespace SlaamMono.Library
{
    public interface ISlaamGame
    {
        void Run();
        Game Game { get; }
    }
}