using Microsoft.Xna.Framework;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.Graphing;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.PlayerProfiles;

namespace Tiptup300.Slaam.States.FirstTime;

public class FirstTimeScreenPerformer : IPerformer<FirstTimeScreenState>, IRenderer<FirstTimeScreenState>
{
   public FirstTimeScreenState _state = new FirstTimeScreenState();

   private readonly IResources _resources;
   private readonly IRenderService _renderService;
   private readonly IInputService _inputService;
   private readonly GameConfiguration _gameConfiguration;

   public FirstTimeScreenPerformer(
       IResources resources,
       IRenderService renderGraphManager,
       IInputService inputService,
       GameConfiguration gameConfiguration)
   {
      _resources = resources;
      _renderService = renderGraphManager;
      _inputService = inputService;
      _gameConfiguration = gameConfiguration;
   }

   public void InitializeState()
   {
      _state.ControlsGraph = buildGraph();
   }

   private Graph buildGraph()
   {
      Graph output;

      output = new Graph(
          new Rectangle(50, 350, _gameConfiguration.DRAWING_GAME_WIDTH - 100, 500), 2,
          new Color(0, 0, 0, 150),
          _resources,
          _renderService);

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

      return _state.ControlsGraph;
   }

   public IState Perform(FirstTimeScreenState state)
   {
      if (_inputService.GetPlayers()[0].PressedAction)
      {
         new ProfileEditScreenRequest() { CreateNewProfile = true };
      }
      return state;
   }

   public void Render(FirstTimeScreenState state)
   {
      _renderService.Render(batch =>
      {
         batch.Draw(_resources.GetTexture("FirstTime").Texture, Vector2.Zero, Color.White);
         state.ControlsGraph.Draw(batch);
      });
   }

   private void unloadContent()
   {
      _resources.GetTexture("FirstTime").Unload();
   }
}
