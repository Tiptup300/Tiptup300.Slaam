using System;

namespace SlaamMono.Library
{
    public class Mut<TStateType>
    {
        public event EventHandler StateChanged;

        private TStateType _state;

        public Mut(TStateType state = default(TStateType))
        {
            _state = state;
        }

        public void Mutate(TStateType newState)
        {
            _state = newState;
            MarkAsMutated();
        }

        public static implicit operator TStateType(Mut<TStateType> value)
        {
            return value._state;
        }

        public void MarkAsMutated()
        {
            StateChanged?.Invoke(this, null);
        }

        public TStateType Get()
            => _state;
    }
}
