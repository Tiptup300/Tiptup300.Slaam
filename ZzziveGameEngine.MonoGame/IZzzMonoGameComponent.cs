using Microsoft.Xna.Framework;

namespace ZzziveGameEngine.MonoGame
{
    public interface IZzzMonoGameComponent
    {
        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Dispose(bool disposing);
    }
}