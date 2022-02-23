namespace ZzziveGameEngine.StateManagement
{
    public interface IStateRenderer<TState> where TState : IState
    {
        IState RenderState(TState state);
    }
}
