using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IScreenManager
    {
        void ChangeTo<TScreen>() where TScreen : IStatePerformer;
        void ChangeTo(IStatePerformer scrn);
        void Draw(SpriteBatch batch);
        void Update();
    }
}