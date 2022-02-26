using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public interface IStatePerformer
    {
        void InitializeState();

        void Perform();

        void RenderState(SpriteBatch batch);

        void Close();
    }
}
