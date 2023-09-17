using System;

namespace System.Tiptup300.Primitives;

public class Mut<TStateType>
{
   public event EventHandler Mutated;

   private TStateType _state;

   public Mut(TStateType state = default)
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
      Mutated?.Invoke(this, null);
   }

   public TStateType Get()
       => _state;
}
