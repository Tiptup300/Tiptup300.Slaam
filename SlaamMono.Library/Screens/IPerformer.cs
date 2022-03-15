using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Library.Screens
{
    public interface IPerformer<TState> where TState : IState
    {
        void InitializeState();

        IState Perform(TState state);

        void RenderState(TState state);
    }
}
