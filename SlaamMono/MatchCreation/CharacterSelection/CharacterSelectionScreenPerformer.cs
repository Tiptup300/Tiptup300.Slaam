using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation.CharacterSelection.CharacterSelectBoxes;
using SlaamMono.Menus;
using SlaamMono.PlayerProfiles;
using SlaamMono.x_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class CharacterSelectionScreenPerformer : IStatePerformer
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

        private CharacterSelectionScreenState _state = new CharacterSelectionScreenState();

        private readonly ILogger _logger;
        private readonly PlayerCharacterSelectBoxPerformer _playerCharacterSelectBox;
        private readonly IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState> _selectBoxStateResolver;
        private readonly IInputService _inputService;

        public CharacterSelectionScreenPerformer(
            ILogger logger,
            PlayerCharacterSelectBoxPerformer playerCharacterSelectBoxPerformer,
            IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState> selectBoxStateResolver, IInputService inputService)
        {
            _logger = logger;
            _playerCharacterSelectBox = playerCharacterSelectBoxPerformer;
            _selectBoxStateResolver = selectBoxStateResolver;
            _inputService = inputService;
        }

        public void Initialize(CharacterSelectionScreenRequestState request)
        {
            // do nothing
        }

        public void InitializeState()
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
                resetBoxes();
            }

            for (int x = 0; x < _state.SelectBoxes.Length; x++)
            {
                if (_state.SelectBoxes[x] != null)
                {
                    if (_state.SelectBoxes[x].Status == PlayerCharacterSelectBoxStatus.Done)
                    {
                        _state.SelectBoxes[x].Status = PlayerCharacterSelectBoxStatus.CharSelect;
                    }
                    _playerCharacterSelectBox.ResetState(_state.SelectBoxes[x]);
                }
            }

        }
        private void resetBoxes()
        {
            if (_state.isForSurvival)
            {
                _state.SelectBoxes = new PlayerCharacterSelectBoxState[1];
                _state.SelectBoxes[0] = buildCharacterSelectBoxState(
                    position: new Vector2(340, 427),
                    parentcharskins: SkinLoadingFunctions.SkinTexture,
                    playeridx: ExtendedPlayerIndex.One,
                    parentskinstrings: SkinLoadingFunctions.Skins,
                    isSurvival: true);
            }
            else
            {
                _state.SelectBoxes = new PlayerCharacterSelectBoxState[_inputService.GetPlayers().Length];
                for (int x = 0; x < _inputService.GetPlayers().Length; x++)
                {
                    _state.SelectBoxes[0] = buildCharacterSelectBoxState(
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

        public IState Perform()
        {
            _state._peopleDone = 0;
            _state._peopleIn = 0;

            if (
                _state._peopleIn == 0 &&
                _inputService.GetPlayers()[0].PressedAction2 &&
                _state.SelectBoxes[0].Status == PlayerCharacterSelectBoxStatus.Computer)
            {
                return goBack();
            }

            for (int idx = 0; idx < _state.SelectBoxes.Length; idx++)
            {
                _playerCharacterSelectBox.Update(_state.SelectBoxes[idx]);
                if (_state.SelectBoxes[idx].Status == PlayerCharacterSelectBoxStatus.Done)
                {
                    _state._peopleDone++;
                }

                if (_state.SelectBoxes[idx].Status != PlayerCharacterSelectBoxStatus.Computer)
                {
                    _state._peopleIn++;
                }
            }
            if (_state._peopleIn > 0 && _state._peopleDone == _state._peopleIn)
            {
                return goForward();
            }
            return _state;
        }
        private IState goBack()
        {
            return new MainMenuScreenState();
        }
        private IState goForward()
        {
            if (_state.isForSurvival)
            {
                MatchSettings matchSettings = new MatchSettings()
                {
                    GameType = GameType.Survival
                };
                List<CharacterShell> list = new List<CharacterShell>();
                list.Add(_playerCharacterSelectBox.GetShell(_state.SelectBoxes[0]));

                return new GameScreenRequestState(list, matchSettings);
            }
            else
            {
                var characterShells = _state.SelectBoxes
                    .Where(selectBox => selectBox.Status == PlayerCharacterSelectBoxStatus.Done)
                    .Select(selectBox => _playerCharacterSelectBox.GetShell(selectBox))
                    .ToList();

                return new LobbyScreenRequestState(characterShells);
            }
        }

        public void RenderState(SpriteBatch batch)
        {
            for (int idx = 0; idx < _state.SelectBoxes.Length; idx++)
            {
                if (_state.SelectBoxes[idx] != null)
                {
                    _playerCharacterSelectBox.Draw(_state.SelectBoxes[idx], batch);
                }
            }
        }

        public void Close()
        {
            _state.SelectBoxes = null;
        }


    }
}
