using Microsoft.Xna.Framework;
using SlaamMono.CharacterSelection;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.SubClasses;
using System.Collections.Generic;

namespace SlaamMono.Screens
{
    public class SurvivalCharSelectScreen : ClassicCharSelectScreen
    {
        private readonly IScreenManager _screenDirector;

        public SurvivalCharSelectScreen(ILogger logger, MainMenuScreen menuScreen, IScreenManager screenDirector)
            : base(logger, menuScreen, screenDirector)
        {
            _screenDirector = screenDirector;
        }

        public override void ResetBoxes()
        {
            SelectBoxes = new CharSelectBox[1];
            SelectBoxes[0] = new CharSelectBox(new Vector2(340, 427), SkinTexture, ExtendedPlayerIndex.One, Skins, DiImplementer.Instance.Get<PlayerColorResolver>());
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
            GameScreen.Instance = new SurvivalScreen(
                list,
                DiImplementer.Instance.Get<ILogger>(),
                DiImplementer.Instance.Get<IScreenManager>());
            _screenDirector.ChangeTo(GameScreen.Instance);
        }
    }
}
