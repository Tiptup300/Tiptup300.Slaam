namespace ZzziveGameEngine.StateManagement
{
    public interface IPerformer<TState> where TState : IState
    {
        void InitializeState();

        IState Perform(TState state);

        void Render(TState state);
    }
}
