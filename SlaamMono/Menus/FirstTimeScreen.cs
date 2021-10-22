using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.Resources;
using SlaamMono.Library.Screens;
using SlaamMono.PlayerProfiles;
using SlaamMono.Resources;
using SlaamMono.x_;

namespace SlaamMono.Menus
{
    public class FirstTimeScreen : IScreen
    {
        private Graph _controlsgraph;
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resourcesManager;
        private readonly IRenderGraph _renderGraphManager;

        public FirstTimeScreen(IScreenManager screenDirector, IResources resourcesManager, IRenderGraph renderGraphManager)
        {
            _screenDirector = screenDirector;
            _resourcesManager = resourcesManager;
            _renderGraphManager = renderGraphManager;
        }

        public void Open()
        {
            BackgroundManager.ChangeBG(BackgroundType.Menu);
            FeedManager.InitializeFeeds("");

            _controlsgraph = buildGraph();
        }

        private Graph buildGraph()
        {
            Graph output;

            output = new Graph(
                new Rectangle(50, 350, GameGlobals.DRAWING_GAME_WIDTH - 100, 500), 2,
                new Color(0, 0, 0, 150),
                _resourcesManager,
                _renderGraphManager);

            output.Items.Columns.Add("");
            output.Items.Columns.Add("Gamepad");
            output.Items.Columns.Add("Keyboard");
            output.Items.Columns.Add("Keyboard 2");
            output.Items.Add(true, new GraphItem("Attack", "A", "Right Ctrl", "Left Ctrl"));
            output.Items.Add(true, new GraphItem("Back", "B", "Right Shift", "Left Shift"));
            output.Items.Add(true, new GraphItem("Start", "Start", "Enter", "Caps Lock"));
            output.Items.Add(true, new GraphItem("Exit", "Back", "Escape", "Tab"));
            output.Items.Add(true, new GraphItem("Fullscreen", "Secret :)", "F", "N/A"));
            output.Items.Add(true, new GraphItem("Take Screenshot", "Secret :P", "Print Scrn", "N/A"));
            output.Items.Add(true, new GraphItem("Toggle FPS", "None)", "Hold SP", "N/A"));
            output.CalculateBlocks();

            return _controlsgraph;
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
            _controlsgraph.Draw(batch);
        }

        public void Close()
        {
            ResourceManager.Instance.GetTexture("FirstTime").Dispose();
        }
    }
}
