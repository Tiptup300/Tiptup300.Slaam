using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Menus;
using SlaamMono.x_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.MatchCreation
{
    public class ClassicCharSelectScreen : IScreen
    {
        public static Texture2D[] SkinTexture;

        private static Random rand = new Random();

        private int Peopledone = 0;
        private int PeopleIn = 0;

        public CharSelectBox[] SelectBoxes;

        private const float VOffset = 195f;
        private const float HOffset = 40f;

        private readonly ILogger _logger;

        private readonly IScreenManager _screenDirector;
        public ClassicCharSelectScreen(ILogger logger, IScreenManager screenDirector)
        {
            _logger = logger;
            _screenDirector = screenDirector;
        }

        public void Open()
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

        public void Update()
        {
            BackgroundManager.SetRotation(1f);
            Peopledone = 0;
            PeopleIn = 0;

            if (
                PeopleIn == 0 &&
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
                    Peopledone++;
                }

                if (SelectBoxes[idx].CurrentState != CharSelectBoxState.Computer)
                {
                    PeopleIn++;
                }
            }
            if (PeopleIn > 0 && Peopledone == PeopleIn)
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
                new LobbyScreen(
                    characterShells,
                    Di.Get<ILogger>(),
                    Di.Get<IScreenManager>(),
                    Di.Get<PlayerColorResolver>(),
                    Di.Get<IResources>(),
                    Di.Get<IRenderGraph>()));
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

        public static List<string> Skins = new List<string>();
        public static bool SkinsLoaded = false;

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
                    //SkinTexture[y] = Texture2D.FromFile(Game1.Graphics.GraphicsDevice, Skins[y]);
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
                SelectBoxes[x] = new CharSelectBox(BoxPositions[x], SkinTexture, (ExtendedPlayerIndex)x, Skins, Di.Get<PlayerColorResolver>());
            }

        }

        public Vector2[] BoxPositions = new Vector2[]
        {
            new Vector2(HOffset + 0, VOffset + 0),
            new Vector2(HOffset + 0, VOffset + 256),
            new Vector2(HOffset + 600, VOffset + 0),
            new Vector2(HOffset + 600, VOffset + 256),
            new Vector2(HOffset + 600, VOffset + 512),
            new Vector2(600, 768),

        };
    }
}
