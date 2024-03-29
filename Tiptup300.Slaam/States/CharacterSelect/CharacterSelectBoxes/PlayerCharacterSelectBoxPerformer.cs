using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.PostGameStats.StatsBoards;

namespace Tiptup300.Slaam.States.CharacterSelect.CharacterSelectBoxes;


public class PlayerCharacterSelectBoxPerformer
{
   private readonly PlayerColorResolver _playerColorResolver;
   private readonly IResources _resources;
   private readonly IInputService _inputService;
   private readonly IFrameTimeService _frameTimeService;
   private readonly IRenderService _renderService;

   public PlayerCharacterSelectBoxPerformer(
         PlayerColorResolver playerColorResolver,
         IResources resources,
         IInputService inputService,
         IFrameTimeService frameTimeService,
         IRenderService renderService)
   {
      _playerColorResolver = playerColorResolver;
      _resources = resources;
      _inputService = inputService;
      _frameTimeService = frameTimeService;
      _renderService = renderService;
   }

   public void ResetState(PlayerCharacterSelectBoxState state)
   {
      if (state.ChosenProfile == null || ProfileManager.Instance.State_PlayableProfiles.Count - 1 != state.ChosenProfile.Max)
      {
         state.ChosenProfile = new IntRange(0, 0, ProfileManager.Instance.State_PlayableProfiles.Count - 1);
      }
   }

   public PlayerCharacterSelectBoxState Update(PlayerCharacterSelectBoxState state)
   {
      switch (state.Status)
      {
         case PlayerCharacterSelectBoxStatus.Computer:
            {
               if (_inputService.GetPlayers()[state.PlayerIndex].PressedStart)
               {
                  state.Status = PlayerCharacterSelectBoxStatus.ProfileSelect;
                  state.MessageLines[1] = DialogStrings._["SelectAProfile"];
                  state.MessageLines[0] = ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].Name;
                  _resetStats(state);
               }
            }
            break;

         case PlayerCharacterSelectBoxStatus.ProfileSelect:
            {
               if (_inputService.GetPlayers()[state.PlayerIndex].PressedAction2)
               {
                  state.MessageLines[0] = DialogStrings._["Player"] + (ExtendedPlayerIndex)state.PlayerIndex;
                  state.MessageLines[1] = DialogStrings._["PressStartToJoin"];
                  state.MessageLines[2] = "";
                  state.MessageLines[3] = "";
                  state.MessageLines[4] = "";
                  state.MessageLines[5] = "";
                  state.Status = PlayerCharacterSelectBoxStatus.Computer;
               }

               if (_inputService.GetPlayers()[state.PlayerIndex].PressedUp)
               {
                  state.ChosenProfile.Add(1);
                  state.MessageLines[0] = ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].Name;
                  _resetStats(state);
               }

               if (_inputService.GetPlayers()[state.PlayerIndex].PressedDown)
               {
                  state.ChosenProfile.Sub(1);
                  state.MessageLines[0] = ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].Name;
                  _resetStats(state);
               }

               if (_inputService.GetPlayers()[state.PlayerIndex].PressedAction)
               {
                  state.Status = PlayerCharacterSelectBoxStatus.CharSelect;
                  _findSkin(ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].Skin, state);
                  state.MessageLines[1] = DialogStrings._["PlayingAs"] + state.ParentSkinStrings[state.ChosenSkin.Value].Substring(state.ParentSkinStrings[state.ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
               }
            }
            break;

         case PlayerCharacterSelectBoxStatus.CharSelect:
            {
               if (_inputService.GetPlayers()[state.PlayerIndex].PressedAction2)
               {
                  state.MessageLines[1] = DialogStrings._["SelectAProfile"];
                  state.Status = PlayerCharacterSelectBoxStatus.ProfileSelect;
               }

               if (_inputService.GetPlayers()[state.PlayerIndex].PressingUp && state.MovementStatus == PlayerCharacterSelectBoxMovementStatus.Stationary)
               {
                  state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Lowering;
               }
               else if (_inputService.GetPlayers()[state.PlayerIndex].PressingDown && state.MovementStatus == PlayerCharacterSelectBoxMovementStatus.Stationary)
               {
                  state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Raising;
               }

               if (_inputService.GetPlayers()[state.PlayerIndex].PressedAction && state.Status == PlayerCharacterSelectBoxStatus.CharSelect)
               {
                  ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].Skin = state.ParentSkinStrings[state.ChosenSkin.Value];
                  ProfileManager.Instance.SaveProfiles();
                  state.Status = PlayerCharacterSelectBoxStatus.Done;
               }

               if (state.MovementStatus != PlayerCharacterSelectBoxMovementStatus.Stationary)
               {
                  if (state.MovementStatus == PlayerCharacterSelectBoxMovementStatus.Lowering)
                  {
                     state.Offset -= _frameTimeService.GetLatestFrame().MovementFactor * state.ScrollSpeed;
                  }
                  else
                  {
                     state.Offset += _frameTimeService.GetLatestFrame().MovementFactor * state.ScrollSpeed;
                  }

                  state.Positions[1] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset - 70);
                  state.Positions[2] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset);
                  state.Positions[3] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset + 70);

               }

               if (state.Offset >= 70)
               {
                  state.ChosenSkin.Add(1, 0, state.ParentCharSkins.Length - 1);
                  _refreshSkins(state);
                  state.Offset = 0;
                  state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Stationary;
                  state.MessageLines[1] = DialogStrings._["PlayingAs"] + state.ParentSkinStrings[state.ChosenSkin.Value].Substring(state.ParentSkinStrings[state.ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                  state.Positions[1] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset - 70);
                  state.Positions[2] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset);
                  state.Positions[3] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset + 70);
               }
               else if (state.Offset <= -70)
               {

                  state.ChosenSkin.Sub(1, 0, state.ParentCharSkins.Length - 1);
                  _refreshSkins(state);
                  state.Offset = 0;
                  state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Stationary;
                  state.MessageLines[1] = DialogStrings._["PlayingAs"] + state.ParentSkinStrings[state.ChosenSkin.Value].Substring(state.ParentSkinStrings[state.ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                  state.Positions[1] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset - 70);
                  state.Positions[2] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset);
                  state.Positions[3] = new Vector2(state.Positions[0].X + 75, state.Positions[0].Y + 125 - 30 + state.Offset + 70);
               }
            }
            break;
      }
      return state;
   }

   public void Draw(PlayerCharacterSelectBoxState state, SpriteBatch batch)
   {
      batch.Draw(_resources.GetTexture("ProfileShell").Texture, new Vector2(13, 96), Color.White);

      var temp = state.MessageLines[1];

      if (state.Status == PlayerCharacterSelectBoxStatus.CharSelect)
      {
         batch.Draw(state.DisplayResources[1], new Vector2(152, 239), new Rectangle(0, 0, 50, 60), Color.White);
         temp = temp.Substring(DialogStrings._["PlayingAs"].Length);
      }

      _renderService.RenderText(temp, new Vector2(31, 141), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
      _renderService.RenderText(state.MessageLines[0], new Vector2(20, 70), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
   }

   public static void _refreshSkins(PlayerCharacterSelectBoxState state)
   {
      state.DisplayResources[0] = state.ParentCharSkins[state.ChosenSkin.NextValue(1, 0, state.ParentCharSkins.Length - 1)];
      state.DisplayResources[1] = state.ParentCharSkins[state.ChosenSkin.Value];
      state.DisplayResources[2] = state.ParentCharSkins[state.ChosenSkin.PreviousValue(1, 0, state.ParentCharSkins.Length - 1)];
   }

   private void _resetStats(PlayerCharacterSelectBoxState state)
   {
      if (state.IsSurvival)
      {
         state.MessageLines[2] = DialogStrings._["BestTime"] + NormalStatsBoard.TimeSpanToString(ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].BestGame);
         state.MessageLines[3] = "";
         state.MessageLines[4] = "";
         state.MessageLines[5] = "";
      }
      else
      {
         state.MessageLines[2] = "" + ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].TotalGames;
         state.MessageLines[3] = "" + ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].TotalDeaths;
         state.MessageLines[4] = "" + ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].TotalPowerups;
         state.MessageLines[5] = "" + ProfileManager.Instance.State_PlayableProfiles[state.ChosenProfile.Value].TotalKills;
      }
   }

   private void _findSkin(string str, PlayerCharacterSelectBoxState state)
   {
      for (int x = 0; x < state.ParentSkinStrings.Count; x++)
      {
         if (state.ParentSkinStrings[x] == str)
         {
            state.ChosenSkin.Value = x;
            break;
         }
      }
      _refreshSkins(state);
   }

   public CharacterShell GetShell(PlayerCharacterSelectBoxState state)
   {
      PlayerType type = PlayerType.Computer;
      if (state.Status != PlayerCharacterSelectBoxStatus.Computer)
      {
         type = PlayerType.Player;
      }
      return new CharacterShell(
          skinLocation: state.ParentSkinStrings[state.ChosenSkin.Value],
          characterProfileIndex: ProfileManager.Instance.State_PlayableProfiles.GetRealIndex(state.ChosenProfile.Value),
          playerIndex: (ExtendedPlayerIndex)state.PlayerIndex,
          playerType: type,
          playerColor: _playerColorResolver.GetColorByIndex(state.PlayerIndex));
   }


}
