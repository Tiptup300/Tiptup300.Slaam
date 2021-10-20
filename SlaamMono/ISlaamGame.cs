using Microsoft.Xna.Framework;

namespace SlaamMono
{
    public interface ISlaamGame
    {
        void Run();
        Game Game { get; }
    }
}