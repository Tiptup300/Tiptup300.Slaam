using System;
using System.Collections.Generic;
using System.Text;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class CharacterSelectionScreenState : IState
    {
        public CharSelectBox[] SelectBoxes;
        public int _peopleDone = 0;
        public int _peopleIn = 0;
    }
}
