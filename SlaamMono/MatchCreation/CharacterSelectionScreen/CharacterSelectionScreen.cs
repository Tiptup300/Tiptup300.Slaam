using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
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
    public class CharacterSelectionScreen : IStatePerformer
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

        protected CharacterSelectionScreenState _state = new CharacterSelectionScreenState();

        private readonly ILogger _logger;

        public CharacterSelectionScreen(ILogger logger)
        {
            _logger = logger;
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
                ResetBoxes();
            }

            for (int x = 0; x < _state.SelectBoxes.Length; x++)
            {
                if (_state.SelectBoxes[x] != null)
                {
                    if (_state.SelectBoxes[x].CurrentState == PlayerCharacterSelectBoxStatus.Done)
                    {
                        _state.SelectBoxes[x].CurrentState = PlayerCharacterSelectBoxStatus.CharSelect;
                    }
                    _state.SelectBoxes[x].Reset();
                }
            }

        }

        public IState Perform()
        {
            _state._peopleDone = 0;
            _state._peopleIn = 0;

            if (
                _state._peopleIn == 0 &&
                InputComponent.Players[0].PressedAction2 &&
                _state.SelectBoxes[0].CurrentState == PlayerCharacterSelectBoxStatus.Computer)
            {
                return GoBack();
            }

            for (int idx = 0; idx < _state.SelectBoxes.Length; idx++)
            {
                _state.SelectBoxes[idx].Update();
                if (_state.SelectBoxes[idx].CurrentState == PlayerCharacterSelectBoxStatus.Done)
                {
                    _state._peopleDone++;
                }

                if (_state.SelectBoxes[idx].CurrentState != PlayerCharacterSelectBoxStatus.Computer)
                {
                    _state._peopleIn++;
                }
            }
            if (_state._peopleIn > 0 && _state._peopleDone == _state._peopleIn)
            {
                return GoForward();
            }
            return _state;
        }

        public virtual IState GoBack()
        {
            return new MainMenuScreenState();
        }

        public virtual IState GoForward()
        {
            if (_state.isForSurvival)
            {
                MatchSettings matchSettings = new MatchSettings()
                {
                    GameType = GameType.Survival
                };
                List<CharacterShell> list = new List<CharacterShell>();
                list.Add(_state.SelectBoxes[0].GetShell());

                return new GameScreenRequestState(list, matchSettings);
            }
            else
            {
                var characterShells = _state.SelectBoxes
                    .Where(selectBox => selectBox.CurrentState == PlayerCharacterSelectBoxStatus.Done)
                    .Select(selectBox => selectBox.GetShell())
                    .ToList();

                return new LobbyScreenRequestState(characterShells);
            }
        }

        public void RenderState(SpriteBatch batch)
        {
            for (int idx = 0; idx < _state.SelectBoxes.Length; idx++)
                if (_state.SelectBoxes[idx] != null)
                    _state.SelectBoxes[idx].Draw(batch);
        }

        public void Close()
        {
            _state.SelectBoxes = null;
        }

        public virtual void ResetBoxes()
        {
            if (_state.isForSurvival)
            {
                _state.SelectBoxes = new PlayerCharacterSelectBox[1];
                _state.SelectBoxes[0] = new PlayerCharacterSelectBox(
                    new Vector2(340, 427),
                    SkinLoadingFunctions.SkinTexture,
                    ExtendedPlayerIndex.One,
                    SkinLoadingFunctions.Skins,
                    x_Di.Get<PlayerColorResolver>(),
                    x_Di.Get<IResources>());
                _state.SelectBoxes[0].Survival = true;
            }
            else
            {
                _state.SelectBoxes = new PlayerCharacterSelectBox[InputComponent.Players.Length];

                for (int x = 0; x < InputComponent.Players.Length; x++)
                {
                    _state.SelectBoxes[x] = new PlayerCharacterSelectBox(
                        _boxPositions[x],
                        SkinLoadingFunctions.SkinTexture,
                        (ExtendedPlayerIndex)x,
                        SkinLoadingFunctions.Skins,
                        x_Di.Get<PlayerColorResolver>(),
                        x_Di.Get<IResources>());
                }
            }
        }
    }
}
