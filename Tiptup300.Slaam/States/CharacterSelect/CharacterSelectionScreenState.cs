using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.States.CharacterSelect.CharacterSelectBoxes;

namespace Tiptup300.Slaam.States.CharacterSelect;

public class CharacterSelectionScreenState : IState
{
   public PlayerCharacterSelectBoxState[] SelectBoxes;
   public int _peopleDone = 0;
   public int _peopleIn = 0;

   public bool isForSurvival = false;
}
