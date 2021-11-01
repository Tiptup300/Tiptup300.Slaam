namespace SlaamMono.Library
{
    public class StateChanger<TStateType>
    {
        public TStateType State { get; private set; }

        public StateChanger(TStateType state = default(TStateType))
        {
            State = state;
        }

        public void ChangeState(TStateType newState)
        {
            State = newState;
        }
    }
}
