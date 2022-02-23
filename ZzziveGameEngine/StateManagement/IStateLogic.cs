namespace ZzziveGameEngine.StateManagement
{
    public interface IStateLogic<TState> where TState : IState
    {
        IState UpdateState(TState state);
    }
}
