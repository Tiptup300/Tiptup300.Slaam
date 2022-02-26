using Microsoft.Xna.Framework;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.MatchCreation;
using SlaamMono.PlayerProfiles;
using System.Collections.Generic;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Survival
{
    public class SurvivalCharacterSelectionScreen : CharacterSelectionScreen
    {
        public SurvivalCharacterSelectionScreen(
            ILogger logger)
            : base(logger)
        {
        }

        public override void ResetBoxes()
        {
            _state.SelectBoxes = new CharSelectBox[1];
            _state.SelectBoxes[0] = new CharSelectBox(
                new Vector2(340, 427),
                SkinLoadingFunctions.SkinTexture,
                ExtendedPlayerIndex.One,
                SkinLoadingFunctions.Skins,
                x_Di.Get<PlayerColorResolver>(),
                x_Di.Get<IResources>());
            _state.SelectBoxes[0].Survival = true;
        }

        public override IState GoForward()
        {
            List<CharacterShell> list = new List<CharacterShell>();
            list.Add(_state.SelectBoxes[0].GetShell());

            return new GameScreenRequestState(list);
        }
    }
}
