using SlaamMono.MatchCreation.CharacterSelection.CharacterSelectBoxes;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation;

 public class CharacterSelectionScreenState : IState
 {
     public PlayerCharacterSelectBoxState[] SelectBoxes;
     public int _peopleDone = 0;
     public int _peopleIn = 0;

     public bool isForSurvival = false;
 }
