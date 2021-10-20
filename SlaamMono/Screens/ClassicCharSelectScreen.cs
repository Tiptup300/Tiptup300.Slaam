using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.SubClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.Screens
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
        private readonly MainMenuScreen _menuScreen;


        #region Constructor

        public ClassicCharSelectScreen(ILogger logger, MainMenuScreen menuScreen)
        {
            _logger = logger;
            _menuScreen = menuScreen;
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
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
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

        #endregion

        #region Update

        public void Update()
        {
            BackgroundManager.SetRotation(1f);
            Peopledone = 0;
            PeopleIn = 0;

            if (PeopleIn == 0 && InputComponent.Players[0].PressedAction2 && SelectBoxes[0].CurrentState == CharSelectBoxState.Computer)
                GoBack();

            for (int idx = 0; idx < SelectBoxes.Length; idx++)
            {
                SelectBoxes[idx].Update();
                if (SelectBoxes[idx].CurrentState == CharSelectBoxState.Done)
                    Peopledone++;

                if (SelectBoxes[idx].CurrentState != CharSelectBoxState.Computer)
                    PeopleIn++;
            }
            if (PeopleIn > 0 && Peopledone == PeopleIn)
            {
                GoForward();
            }
        }

        public virtual void GoBack()
        {
            ScreenDirector.ChangeScreen(_menuScreen);
        }

        public virtual void GoForward()
        {
            List<CharacterShell> templist = new List<CharacterShell>();
            for (int idx = 0; idx < SelectBoxes.Length; idx++)
                if (SelectBoxes[idx].CurrentState == CharSelectBoxState.Done)
                    templist.Add(SelectBoxes[idx].GetShell());

            ScreenDirector.ChangeScreen(new LobbyScreen(templist, DI.Instance.Get<ILogger>()));
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch)
        {
            for (int idx = 0; idx < SelectBoxes.Length; idx++)
                if (SelectBoxes[idx] != null)
                    SelectBoxes[idx].Draw(batch);
        }

        #endregion

        #region Dispose

        public void Close()
        {
            SelectBoxes = null;
        }

        #endregion

        #region Extra Methods

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
                SelectBoxes[x] = new CharSelectBox(BoxPositions[x], SkinTexture, (ExtendedPlayerIndex)x, Skins);
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

        #endregion
    }
}
