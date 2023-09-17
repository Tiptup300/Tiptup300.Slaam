namespace ZzziveGameEngine.StateManagement;

 public interface IRenderer<TState> where TState : IState
 {
     void Render(TState state);
 }
