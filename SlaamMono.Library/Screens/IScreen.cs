using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IScreen
    {
        void Open();

        void UpdateState();

        void Draw(SpriteBatch batch);

        void Close();
    }
}
