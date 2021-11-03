using System;

namespace SlaamMono.Library
{
    public class State<TStateType>
    {
        public event EventHandler StateChanged;

        private TStateType _state;

        public State(TStateType state = default(TStateType))
        {
            _state = state;
        }

        public void Change(TStateType newState)
        {
            _state = newState;
            MarkAsChanged();
        }

        public static implicit operator TStateType(State<TStateType> value)
        {
            return value._state;
        }

        public void MarkAsChanged()
        {
            StateChanged?.Invoke(this, null);
        }

        public TStateType Get()
            => _state;
    }
}
