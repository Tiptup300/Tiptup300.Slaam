using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.MatchCreation;
using SlaamMono.MatchCreation.CharacterSelection.CharacterSelectBoxes;
using System;
using System.Collections.Generic;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.States.CharacterSelect;

 public class CharacterSelectionScreenRequestResolver : IResolver<CharacterSelectionScreenRequest, IState>
 {

     private const float _verticalOffset = 195f;
     private const float _horizontalOffset = 40f;
     private readonly Vector2[] _boxPositions = new Vector2[]
     {
         new Vector2(_horizontalOffset + 0, _verticalOffset + 0),
         new Vector2(_horizontalOffset + 0, _verticalOffset + 256),
         new Vector2(_horizontalOffset + 600, _verticalOffset + 0),
         new Vector2(_horizontalOffset + 600, _verticalOffset + 256),
         new Vector2(_horizontalOffset + 600, _verticalOffset + 512),
         new Vector2(600, 768)
     };

     private readonly ILogger _logger;
     private readonly IInputService _inputService;
     private readonly PlayerCharacterSelectBoxPerformer _playerCharacterSelectBox;
     private readonly IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState> _selectBoxStateResolver;


     public CharacterSelectionScreenRequestResolver(
         ILogger logger,
         IInputService inputService,
         PlayerCharacterSelectBoxPerformer playerCharacterSelectBox,
         IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState> selectBoxStateResolver)
     {
         _logger = logger;
         _inputService = inputService;
         _playerCharacterSelectBox = playerCharacterSelectBox;
         _selectBoxStateResolver = selectBoxStateResolver;
     }

     public IState Resolve(CharacterSelectionScreenRequest request)
     {
         CharacterSelectionScreenState output;

         output = new CharacterSelectionScreenState();
         init(output);

         return output;
     }

     private void init(CharacterSelectionScreenState output)
     {
         _logger.Log("----------------------------------");
         _logger.Log("     Char.acter Select Screen      ");
         _logger.Log("----------------------------------");
         _logger.Log("Attemping to load in all skins...");

         SkinLoadingFunctions.LoadAllSkins(_logger);

         _logger.Log("Listing of skins complete;");

         if (SkinLoadingFunctions.Skins.Count < 1)
         {
             _logger.Log("0 Skins were found, Program Abort");
             throw new Exception("0 Skins were found, Program Abort");
         }
         else
         {
             resetBoxes(output);
         }

         for (int x = 0; x < output.SelectBoxes.Length; x++)
         {
             if (output.SelectBoxes[x] != null)
             {
                 if (output.SelectBoxes[x].Status == PlayerCharacterSelectBoxStatus.Done)
                 {
                     output.SelectBoxes[x].Status = PlayerCharacterSelectBoxStatus.CharSelect;
                 }
                 _playerCharacterSelectBox.ResetState(output.SelectBoxes[x]);
             }
         }

     }

     private void resetBoxes(CharacterSelectionScreenState state)
     {
         if (state.isForSurvival)
         {
             state.SelectBoxes = new PlayerCharacterSelectBoxState[1];
             state.SelectBoxes[0] = buildCharacterSelectBoxState(
                 position: new Vector2(340, 427),
                 parentcharskins: SkinLoadingFunctions.SkinTexture,
                 playeridx: ExtendedPlayerIndex.One,
                 parentskinstrings: SkinLoadingFunctions.Skins,
                 isSurvival: true);
         }
         else
         {
             state.SelectBoxes = new PlayerCharacterSelectBoxState[_inputService.GetPlayers().Length];
             for (int x = 0; x < _inputService.GetPlayers().Length; x++)
             {
                 state.SelectBoxes[0] = buildCharacterSelectBoxState(
                     position: _boxPositions[x],
                     parentcharskins: SkinLoadingFunctions.SkinTexture,
                     playeridx: (ExtendedPlayerIndex)x,
                     parentskinstrings: SkinLoadingFunctions.Skins);
             }
         }
     }

     private PlayerCharacterSelectBoxState buildCharacterSelectBoxState(
        Vector2 position,
        Texture2D[] parentcharskins,
        ExtendedPlayerIndex playeridx,
        List<string> parentskinstrings,
        bool isSurvival = false)
     {
         PlayerCharacterSelectBoxState output;

         output = _selectBoxStateResolver.Resolve(new PlayerCharacterSelectBoxRequest()
         {
             Position = position,
             parentcharskins = parentcharskins,
             playeridx = playeridx,
             parentskinstrings = parentskinstrings,
             IsSurvival = isSurvival
         });

         return output;
     }
 }
