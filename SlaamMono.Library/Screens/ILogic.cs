using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface ILogic
    {
        void Open();

        void UpdateState();

        void Draw(SpriteBatch batch);

        void Close();
    }
}