using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.MatchCreation.CharacterSelection.CharacterSelectBoxes;
using SlaamMono.Menus;
using SlaamMono.PlayerProfiles;
using System.Collections.Generic;
using System.Linq;
using Tiptup300.Slaam.Library.Rendering;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation;

public class CharacterSelectionScreenPerformer : IPerformer<CharacterSelectionScreenState>, IRenderer<CharacterSelectionScreenState>
 {
     private readonly ILogger _logger;
     private readonly PlayerCharacterSelectBoxPerformer _playerCharacterSelectBox;
     private readonly IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState> _selectBoxStateResolver;
     private readonly IInputService _inputService;
     private readonly IRenderService _renderService;

     public CharacterSelectionScreenPerformer(
         ILogger logger,
         PlayerCharacterSelectBoxPerformer playerCharacterSelectBoxPerformer,
         IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState> selectBoxStateResolver,
         IInputService inputService,
         IRenderService renderService)
     {
         _logger = logger;
         _playerCharacterSelectBox = playerCharacterSelectBoxPerformer;
         _selectBoxStateResolver = selectBoxStateResolver;
         _inputService = inputService;
         _renderService = renderService;
     }

     public void InitializeState()
     {
     }



     public IState Perform(CharacterSelectionScreenState state)
     {
         state._peopleDone = 0;
         state._peopleIn = 0;

         if (
             state._peopleIn == 0 &&
             _inputService.GetPlayers()[0].PressedAction2 &&
             state.SelectBoxes[0].Status == PlayerCharacterSelectBoxStatus.Computer)
         {
             return goBack();
         }

         for (int idx = 0; idx < state.SelectBoxes.Length; idx++)
         {
             _playerCharacterSelectBox.Update(state.SelectBoxes[idx]);
             if (state.SelectBoxes[idx].Status == PlayerCharacterSelectBoxStatus.Done)
             {
                 state._peopleDone++;
             }

             if (state.SelectBoxes[idx].Status != PlayerCharacterSelectBoxStatus.Computer)
             {
                 state._peopleIn++;
             }
         }
         if (state._peopleIn > 0 && state._peopleDone == state._peopleIn)
         {
             return goForward(state);
         }
         return state;
     }
     private IState goBack()
     {
         return new MainMenuScreenState();
     }
     private IState goForward(CharacterSelectionScreenState state)
     {
         if (state.isForSurvival)
         {
             MatchSettings matchSettings = new MatchSettings()
             {
                 GameType = GameType.Survival
             };
             List<CharacterShell> list = new List<CharacterShell>();
             list.Add(_playerCharacterSelectBox.GetShell(state.SelectBoxes[0]));

             return new MatchRequest(list, matchSettings);
         }
         else
         {
             var characterShells = state.SelectBoxes
                 .Where(selectBox => selectBox.Status == PlayerCharacterSelectBoxStatus.Done)
                 .Select(selectBox => _playerCharacterSelectBox.GetShell(selectBox))
                 .ToList();

             return new LobbyScreenRequestState(characterShells);
         }
     }

     public void Render(CharacterSelectionScreenState state)
     {
         _renderService.Render(batch =>
         {
             for (int idx = 0; idx < state.SelectBoxes.Length; idx++)
             {
                 if (state.SelectBoxes[idx] != null)
                 {
                     _playerCharacterSelectBox.Draw(state.SelectBoxes[idx], batch);
                 }
             }
         });
     }
 }
