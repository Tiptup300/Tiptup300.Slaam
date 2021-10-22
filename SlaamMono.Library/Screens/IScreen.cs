using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IScreen
    {
        void Open();

        void Update();

        void Draw(SpriteBatch batch);

        void Close();
    }
}
