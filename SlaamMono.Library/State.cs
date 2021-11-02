namespace SlaamMono.Library
{
    public class State<TStateType>
    {
        public TStateType Value { get; private set; }

        public State(TStateType state = default(TStateType))
        {
            Value = state;
        }

        public void ChangeState(TStateType newState)
        {
            Value = newState;
        }

        public static implicit operator TStateType(State<TStateType> value)
        {
            return value.Value;
        }
    }
}
