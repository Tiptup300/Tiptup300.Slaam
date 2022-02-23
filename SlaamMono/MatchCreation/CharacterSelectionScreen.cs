using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.x_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZzziveGameEngine;

namespace SlaamMono.MatchCreation
{
    public class CharacterSelectionScreen : IStateUpdater
    {
        private const float VOffset = 195f;
        private const float HOffset = 40f;
        private readonly Vector2[] _boxPositions = new Vector2[]
        {
            new Vector2(HOffset + 0, VOffset + 0),
            new Vector2(HOffset + 0, VOffset + 256),
            new Vector2(HOffset + 600, VOffset + 0),
            new Vector2(HOffset + 600, VOffset + 256),
            new Vector2(HOffset + 600, VOffset + 512),
            new Vector2(600, 768)
        };

        public static Texture2D[] SkinTexture;
        public static List<string> Skins = new List<string>();
        public static bool SkinsLoaded = false;
        private static Random rand = new Random();

        protected CharacterSelectionScreenState _state = new CharacterSelectionScreenState();

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResolver<LobbyScreenRequest, LobbyScreen> _lobbyScreenResolver;

        public CharacterSelectionScreen(ILogger logger, IScreenManager screenDirector, IResolver<LobbyScreenRequest, LobbyScreen> lobbyScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _lobbyScreenResolver = lobbyScreenResolver;
        }

        public void Initialize(CharacterSelectionScreenRequest request)
        {
            // do nothing
        }

        public void InitializeState()
        {
            _logger.Log("----------------------------------");
            _logger.Log("     Character Select Screen      ");
            _logger.Log("----------------------------------");
            _logger.Log("Attemping to load in all skins...");

            LoadAllSkins(_logger);

            _logger.Log("Listing of skins complete;");

            if (Skins.Count < 1)
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
                    if (_state.SelectBoxes[x].CurrentState == CharSelectBoxState.Done)
                    {
                        _state.SelectBoxes[x].CurrentState = CharSelectBoxState.CharSelect;
                    }
                    _state.SelectBoxes[x].Reset();
                }
            }

        }

        public void UpdateState()
        {
            _state._peopleDone = 0;
            _state._peopleIn = 0;

            if (
                _state._peopleIn == 0 &&
                InputComponent.Players[0].PressedAction2 &&
                _state.SelectBoxes[0].CurrentState == CharSelectBoxState.Computer)
            {
                GoBack();
            }

            for (int idx = 0; idx < _state.SelectBoxes.Length; idx++)
            {
                _state.SelectBoxes[idx].Update();
                if (_state.SelectBoxes[idx].CurrentState == CharSelectBoxState.Done)
                {
                    _state._peopleDone++;
                }

                if (_state.SelectBoxes[idx].CurrentState != CharSelectBoxState.Computer)
                {
                    _state._peopleIn++;
                }
            }
            if (_state._peopleIn > 0 && _state._peopleDone == _state._peopleIn)
            {
                GoForward();
            }
        }

        public virtual void GoBack()
        {
            _screenDirector.ChangeTo<IMainMenuScreen>();
        }

        public virtual void GoForward()
        {
            var characterShells = _state.SelectBoxes
                .Where(selectBox => selectBox.CurrentState == CharSelectBoxState.Done)
                .Select(selectBox => selectBox.GetShell())
                .ToList();

            _screenDirector.ChangeTo(
                _lobbyScreenResolver.Resolve(new LobbyScreenRequest(characterShells)));
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

        public static string ReturnRandSkin(ILogger logger)
        {
            if (!SkinsLoaded)
            {
                LoadAllSkins(logger);
            }
            return Skins[rand.Next(0, Skins.Count)];
        }

        private static void LoadAllSkins(ILogger logger)
        {
            if (!SkinsLoaded)
            {
                List<string> skins = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\content\\SkinList.txt").ToList();
                for (int x = 0; x < skins.Count; x++)
                {
                    Skins.Add(skins[x]);
                    logger.Log(" - \"" + skins[x] + "\" was added to listing.");
                }
                SkinTexture = new Texture2D[Skins.Count];
                for (int y = 0; y < Skins.Count; y++)
                {
                    SkinTexture[y] = SlaamGame.Content.Load<Texture2D>("content\\skins\\" + Skins[y]);
                    if (!(SkinTexture[y].Width == 250 && SkinTexture[y].Height == 180))
                    {
                        Skins.RemoveAt(y);
                        y--;
                    }
                }
                SkinsLoaded = true;
            }
        }

        public virtual void ResetBoxes()
        {
            _state.SelectBoxes = new CharSelectBox[InputComponent.Players.Length];

            for (int x = 0; x < InputComponent.Players.Length; x++)
            {
                _state.SelectBoxes[x] = new CharSelectBox(
                    _boxPositions[x],
                    SkinTexture,
                    (ExtendedPlayerIndex)x,
                    Skins,
                    x_Di.Get<PlayerColorResolver>(),
                    x_Di.Get<IResources>());
            }

        }
    }
}
