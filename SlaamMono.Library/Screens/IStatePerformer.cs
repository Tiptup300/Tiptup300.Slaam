using Microsoft.Xna.Framework.Graphics;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Library.Screens
{
    public interface IStatePerformer
    {
        void InitializeState();

        IState Perform();

        void RenderState();
    }
}
