using Microsoft.Xna.Framework;
using System.Tiptup300.Primitives;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.Graphing;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;

namespace Tiptup300.Slaam.PlayerProfiles;

public class ProfileEditScreenRequestResolver : IResolver<ProfileEditScreenRequest, IState>
{
   private readonly IResources _resources;
   private readonly IRenderService _renderService;

   public ProfileEditScreenRequestResolver(IResources resources, IRenderService renderService)
   {
      _resources = resources;
      _renderService = renderService;
   }

   public IState Resolve(ProfileEditScreenRequest request)
   {
      ProfileEditScreenState output;

      output = new ProfileEditScreenState();
      InitializeState(output, request.width, request.height);

      return output;
   }

   public void InitializeState(ProfileEditScreenState _state, int width, int height)
   {
      _state.MainMenu = new Graph(new Rectangle(100, 200, width - 100, height - 624), 2, new Color(0, 0, 0, 150), _resources, _renderService);
      _state.SubMenu = new Graph(new Rectangle(100, 200, width - 100, height - 624), 2, new Color(0, 0, 0, 150), _resources, _renderService);
      setupMainMenu(_state);
      resetSubMenu(_state);
      if (_state.SetupNewProfile)
      {
         _state.SetupNewProfile = false;
         _state.CurrentMenu.Value = 0;
         _state.WaitingForQwerty = true;
      }
   }
   private void setupMainMenu(ProfileEditScreenState _state)
   {
      _state.MainMenu.Items.Columns.Clear();
      _state.MainMenu.Items.Columns.Add("PROFILES");
      _state.MainMenu.Items.Clear();
      for (int x = 1; x < ProfileManager.PlayableProfiles.Count; x++)
      {
         _state.MainMenu.Items.Add(true, new GraphItem(ProfileManager.PlayableProfiles[x].Name, x.ToString()));
      }
      _state.MainMenu.Items.Add(true, new GraphItem("Create New Profile...", "new"));
      _state.MainMenu.SetHighlight(0);
      _state.CurrentMenuChoice = new IntRange(0, 0, _state.MainMenu.Items.Count - 1);
   }
   private void resetSubMenu(ProfileEditScreenState _state)
   {
      _state.SubMenu.Items.Clear();
      _state.SubMenu.Items.Columns.Clear();
      _state.SubMenu.Items.Columns.Add("OPTIONS");
      _state.SubMenu.Items.Add(true, new GraphItem("Rename", "ren"), new GraphItem("Delete", "del"), new GraphItem("Clear Stats", "clr"));
      _state.SubMenu.CalculateBlocks();
   }
}
