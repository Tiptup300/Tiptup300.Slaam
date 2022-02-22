using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IScreenManager
    {
        void ChangeTo<TScreen>() where TScreen : IStateUpdater;
        void ChangeTo(IStateUpdater scrn);
        void Draw(SpriteBatch batch);
        void Update();
    }
}