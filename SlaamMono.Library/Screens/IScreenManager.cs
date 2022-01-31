using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IScreenManager
    {
        void ChangeTo<TScreen>() where TScreen : ILogic;
        void ChangeTo(ILogic scrn);
        void Draw(SpriteBatch batch);
        void Update();
    }
}