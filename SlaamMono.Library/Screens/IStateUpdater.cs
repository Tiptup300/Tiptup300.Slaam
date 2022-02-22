using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IStateUpdater
    {
        void InitializeState();

        void UpdateState();

        void Draw(SpriteBatch batch);

        void Close();
    }
}
