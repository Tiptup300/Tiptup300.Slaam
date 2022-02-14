using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.x_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.MatchCreation
{
    public class CharacterSelectionScreen : ILogic
    {
        public static Texture2D[] SkinTexture;

        private static Random rand = new Random();

        private int _peopleDone = 0;
        private int _peopleIn = 0;

        protected CharSelectBox[] SelectBoxes;

        private const float VOffset = 195f;
        private const float HOffset = 40f;

        public static List<string> Skins = new List<string>();
        public static bool SkinsLoaded = false;



        public Vector2[] BoxPositions = new Vector2[]
        {
            new Vector2(HOffset + 0, VOffset + 0),
            new Vector2(HOffset + 0, VOffset + 256),
            new Vector2(HOffset + 600, VOffset + 0),
            new Vector2(HOffset + 600, VOffset + 256),
            new Vector2(HOffset + 600, VOffset + 512),
            new Vector2(600, 768),

        };

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
                SlaamGame.Instance.Exit();
            }
            else
            {
                ResetBoxes();
            }
            BackgroundManager.ChangeBG(BackgroundType.Menu);
            FeedManager.InitializeFeeds(DialogStrings.CharacterSelectScreenFeed);

            for (int x = 0; x < SelectBoxes.Length; x++)
            {
                if (SelectBoxes[x] != null)
                {
                    if (SelectBoxes[x].CurrentState == CharSelectBoxState.Done)
                    {
                        SelectBoxes[x].CurrentState = CharSelectBoxState.CharSelect;
                    }
                    SelectBoxes[x].Reset();
                }
            }

        }

        public void UpdateState()
        {
            BackgroundManager.SetRotation(1f);
            _peopleDone = 0;
            _peopleIn = 0;

            if (
                _peopleIn == 0 &&
                InputComponent.Players[0].PressedAction2 &&
                SelectBoxes[0].CurrentState == CharSelectBoxState.Computer)
            {
                GoBack();
            }

            for (int idx = 0; idx < SelectBoxes.Length; idx++)
            {
                SelectBoxes[idx].Update();
                if (SelectBoxes[idx].CurrentState == CharSelectBoxState.Done)
                {
                    _peopleDone++;
                }

                if (SelectBoxes[idx].CurrentState != CharSelectBoxState.Computer)
                {
                    _peopleIn++;
                }
            }
            if (_peopleIn > 0 && _peopleDone == _peopleIn)
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
            var characterShells = SelectBoxes
                .Where(selectBox => selectBox.CurrentState == CharSelectBoxState.Done)
                .Select(selectBox => selectBox.GetShell())
                .ToList();

            _screenDirector.ChangeTo(
                _lobbyScreenResolver.Resolve(new LobbyScreenRequest(characterShells)));
        }

        public void Draw(SpriteBatch batch)
        {
            for (int idx = 0; idx < SelectBoxes.Length; idx++)
                if (SelectBoxes[idx] != null)
                    SelectBoxes[idx].Draw(batch);
        }

        public void Close()
        {
            SelectBoxes = null;
        }

        /// <summary>
        /// Returns a random skin string.
        /// </summary>
        /// <returns></returns>
        public static string ReturnRandSkin(ILogger logger)
        {
            if (!SkinsLoaded)
            {
                LoadAllSkins(logger);
            }
            return Skins[rand.Next(0, Skins.Count)];
        }

        /// <summary>
        /// Loads all skins and checks them for the correct height/width
        /// </summary>
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
            SelectBoxes = new CharSelectBox[InputComponent.Players.Length];

            for (int x = 0; x < InputComponent.Players.Length; x++)
            {
                SelectBoxes[x] = new CharSelectBox(
                    BoxPositions[x],
                    SkinTexture,
                    (ExtendedPlayerIndex)x,
                    Skins,
                    x_Di.Get<PlayerColorResolver>(),
                    x_Di.Get<IResources>());
            }

        }
    }
}
