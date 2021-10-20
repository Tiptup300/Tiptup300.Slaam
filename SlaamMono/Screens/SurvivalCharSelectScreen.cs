using Microsoft.Xna.Framework;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Screens;
using System.Collections.Generic;

namespace SlaamMono
{
    public class SurvivalCharSelectScreen : ClassicCharSelectScreen
    {
        private ILogger _logger;

        public SurvivalCharSelectScreen(ILogger logger, MainMenuScreen menuScreen)
            : base(logger, menuScreen)
        {
            _logger = logger;
        }

        public override void ResetBoxes()
        {
            SelectBoxes = new CharSelectBox[1];
            SelectBoxes[0] = new CharSelectBox(new Vector2(340, 427), SkinTexture, ExtendedPlayerIndex.One, Skins);
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
            GameScreen.Instance = new SurvivalScreen(list, DI.Instance.Get<ILogger>());
            ScreenDirector.ChangeScreen(GameScreen.Instance);
        }
    }
}
