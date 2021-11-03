using Microsoft.Xna.Framework;

namespace SlaamMono.Library
{
    public interface IGameWidget
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw();
    }
}
