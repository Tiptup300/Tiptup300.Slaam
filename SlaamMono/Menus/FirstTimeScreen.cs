using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Graphing;
using SlaamMono.Library.Input;
using SlaamMono.Library.Screens;
using SlaamMono.PlayerProfiles;
using SlaamMono.Resources;
using SlaamMono.x_;

namespace SlaamMono.Menus
{
    public class FirstTimeScreen : IScreen
    {
        Graph controlsgraph = new Graph(new Rectangle(50, 350, GameGlobals.DRAWING_GAME_WIDTH - 100, 500), 2, new Color(0, 0, 0, 150));
        private readonly IScreenManager _screenDirector;

        public FirstTimeScreen(IScreenManager screenDirector)
        {
            _screenDirector = screenDirector;
        }

        public void Open()
        {
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
            FeedManager.InitializeFeeds("");
            controlsgraph.Items.Columns.Add("");
            controlsgraph.Items.Columns.Add("Gamepad");
            controlsgraph.Items.Columns.Add("Keyboard");
            controlsgraph.Items.Columns.Add("Keyboard 2");
            controlsgraph.Items.Add(true, new GraphItem("Attack", "A", "Right Ctrl", "Left Ctrl"));
            controlsgraph.Items.Add(true, new GraphItem("Back", "B", "Right Shift", "Left Shift"));
            controlsgraph.Items.Add(true, new GraphItem("Start", "Start", "Enter", "Caps Lock"));
            controlsgraph.Items.Add(true, new GraphItem("Exit", "Back", "Escape", "Tab"));
            controlsgraph.Items.Add(true, new GraphItem("Fullscreen", "Secret :)", "F", "N/A"));
            controlsgraph.Items.Add(true, new GraphItem("Take Screenshot", "Secret :P", "Print Scrn", "N/A"));
            controlsgraph.Items.Add(true, new GraphItem("Toggle FPS", "None)", "Hold SP", "N/A"));
            controlsgraph.CalculateBlocks();
        }

        public void Update()
        {
            if (InputComponent.Players[0].PressedAction)
            {
                ProfileEditScreen.Instance.SetupNewProfile = true;
                _screenDirector.ChangeTo(ProfileEditScreen.Instance);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.Instance.GetTexture("FirstTime").Texture, Vector2.Zero, Color.White);
            controlsgraph.Draw(batch);
        }

        public void Close()
        {
            ResourceManager.Instance.GetTexture("FirstTime").Dispose();
        }
    }
}
