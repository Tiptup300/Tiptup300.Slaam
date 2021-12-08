using Microsoft.Xna.Framework;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using SlaamMono.PlayerProfiles;
using System.Collections.Generic;

namespace SlaamMono.Survival
{
    public class SurvivalCharacterSelectionScreen : CharacterSelectionScreen
    {
        private readonly IScreenManager _screenDirector;
        private readonly IResolver<GameScreenRequest, SurvivalGameScreen> _survivalGameScreenRequest;

        public SurvivalCharacterSelectionScreen(ILogger logger, IScreenManager screenDirector, IResolver<GameScreenRequest, SurvivalGameScreen> survivalGameScreenRequest)
            : base(logger, screenDirector)
        {
            _screenDirector = screenDirector;
            _survivalGameScreenRequest = survivalGameScreenRequest;
        }

        public override void ResetBoxes()
        {
            SelectBoxes = new CharSelectBox[1];
            SelectBoxes[0] = new CharSelectBox(
                new Vector2(340, 427),
                SkinTexture,
                ExtendedPlayerIndex.One,
                Skins,
                x_Di.Get<PlayerColorResolver>(),
                x_Di.Get<IResources>());
            SelectBoxes[0].Survival = true;
        }

        public override void GoBack()
        {
            base.GoBack();
        }

        public override void GoForward()
        {
            List<CharacterShell> list = new List<CharacterShell>();
            list.Add(SelectBoxes[0].GetShell());
            GameScreen.Instance = _survivalGameScreenRequest.Resolve(new GameScreenRequest(list));
            _screenDirector.ChangeTo(GameScreen.Instance);
        }
    }
}
