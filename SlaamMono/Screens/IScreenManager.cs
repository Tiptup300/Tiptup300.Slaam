using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Screens
{
    public interface IScreenManager
    {
        void ChangeTo(IScreen scrn);
        void Draw(SpriteBatch batch);
        void Update();
    }
}