using Microsoft.Xna.Framework;

namespace SlaamMono.Library
{
    public interface IGameWidget
    {
        void Load();
        void Unload();
        void Update(GameTime gameTime);
        void Draw();
    }
}
