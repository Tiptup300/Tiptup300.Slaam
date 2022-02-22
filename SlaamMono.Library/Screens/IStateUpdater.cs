using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IStateUpdater
    {
        void InitializeState();

        void UpdateState();

        void RenderState(SpriteBatch batch);

        void Close();
    }
}
