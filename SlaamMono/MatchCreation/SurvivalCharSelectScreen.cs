using Microsoft.Xna.Framework;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Menus;
using SlaamMono.PlayerProfiles;
using System.Collections.Generic;

namespace SlaamMono.MatchCreation
{
    public class SurvivalCharSelectScreen : ClassicCharSelectScreen
    {
        private readonly IScreenManager _screenDirector;

        public SurvivalCharSelectScreen(ILogger logger, IScreenManager screenDirector)
            : base(logger, screenDirector)
        {
            _screenDirector = screenDirector;
        }

        public override void ResetBoxes()
        {
            SelectBoxes = new CharSelectBox[1];
            SelectBoxes[0] = new CharSelectBox(
                new Vector2(340, 427),
                SkinTexture,
                ExtendedPlayerIndex.One,
                Skins,
                Di.Get<PlayerColorResolver>(),
                Di.Get<IResources>());
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
                Di.Get<ILogger>(),
                Di.Get<IScreenManager>(),
                Di.Get<IResources>());
            _screenDirector.ChangeTo(GameScreen.Instance);
        }
    }
}
