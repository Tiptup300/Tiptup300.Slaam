using System;

namespace SlaamMono.Library
{
    public class StateReference<TStateType>
    {
        public TStateType State { get; private set; }
        public event EventHandler StateChanged;

        public StateReference(TStateType state = default(TStateType))
        {
            State = state;
        }

        public void Change(TStateType newState)
        {
            State = newState;
            MarkAsChanged();
        }

        public static implicit operator TStateType(StateReference<TStateType> value)
        {
            return value.State;
        }

        public void MarkAsChanged()
        {
            StateChanged?.Invoke(this, null);
        }
    }
}
