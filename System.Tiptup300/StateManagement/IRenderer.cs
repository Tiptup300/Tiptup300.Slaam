namespace System.Tiptup300.StateManagement;

public interface IRenderer<TState> where TState : IState
{
   void Render(TState state);
}
