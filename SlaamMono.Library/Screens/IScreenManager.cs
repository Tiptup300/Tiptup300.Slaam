using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IScreenManager
    {
        void ChangeTo<TScreen>();
        void ChangeTo(IScreen scrn);
        void Draw(SpriteBatch batch);
        void Update();
    }
}