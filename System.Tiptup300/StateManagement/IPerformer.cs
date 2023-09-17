namespace System.Tiptup300.StateManagement;

public interface IPerformer<TState> where TState : IState
{
   void InitializeState();

   IState Perform(TState state);
}
