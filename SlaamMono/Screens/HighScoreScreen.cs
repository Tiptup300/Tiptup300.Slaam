using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SlaamMono.Input;

namespace SlaamMono
{
    class HighScoreScreen : IScreen
    {
#if !ZUNE
        public const int MAX_HIGHSCORES = 25;
#else
        public const int MAX_HIGHSCORES = 7;
#endif
        SurvivalStatsBoard statsboard = new SurvivalStatsBoard(null, new Rectangle(10, 68, GameGlobals.DRAWING_GAME_WIDTH-20, GameGlobals.DRAWING_GAME_WIDTH-20), new Color(0, 0, 0, 150),MAX_HIGHSCORES);

        public HighScoreScreen()
        {
            BackgroundManager.SetRotation(.5f);
        }

        public void Initialize()
        {
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
            statsboard.CalculateStats();
            statsboard.ConstructGraph(25);
        }

        public void Update()
        {
            if (InputComponent.Players[0].PressedAction2)
            {
                ScreenHelper.ChangeScreen(MenuScreen.Instance);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            statsboard.MainBoard.Draw(batch);
        }

        public void Dispose()
        {

        }
    }
}
