namespace ZzziveGameEngine.StateManagement
{
    public interface IStateRenderer<TState> where TState : IState
    {
        void RenderState(TState state);
    }
}
