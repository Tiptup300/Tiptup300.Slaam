using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
{
    public class LobbyScreen : Screen
    {
        #region Variables
#if !ZUNE
        private const int MAX_PLAYERS = 8;
#else
        private const int MAX_PLAYERS = 4;
#endif
        public List<CharacterShell> SetupChars;
        private Texture2D CurrentBoardTexture;
        private int PlayerAmt;
        private string[] Dialogs = new string[2];
        private string CurrentBoardLocation;
#if !ZUNE
        private bool ButtonOnLeft = true;
#endif
        private bool ViewingSettings = false;
        public Graph MainMenu = new Graph(new Rectangle(10, 10, GameGlobals.DRAWING_GAME_WIDTH - 20, 624), 2, new Color(0, 0, 0, 150));
        private IntRange MenuChoice;
        #endregion

        #region Constructor

        public LobbyScreen(List<CharacterShell> chars)
        {
            SetupChars = chars;
            PlayerAmt = SetupChars.Count;

            MainMenu.Items.Columns.Add("SETTING");
            MainMenu.Items.Columns.Add("SETTING");
            MainMenu.Items.Add(true,
                new GraphItemSetting(0,"GameType", "Classic","Spree","Timed Spree"),
                new GraphItemSetting(1,"Lives (Classic Only)", "3", "5", "10", "20", "40", "50", "100"),
                new GraphItemSetting(2,"Speed", "0.50x", "0.75x", "1.00x", "1.25x", "1.50x"), 
                new GraphItemSetting(3,"Time of Match (Timed Spree Only)","1 Minute", "2 Minutes", "3 Minutes", "5 Minutes", "10 Minutes", "20 Minutes", "30 Minutes", "45 Minutes", "1 Hour"),
                new GraphItemSetting(4,"Respawn Time","Instant", "2 Seconds", "4 Seconds", "6 Seconds", "8 Seconds", "10 Seconds"),
                new GraphItemSetting(5,"Kills To Win (Spree Only)","5","10","15","20","25","30","40","50","100"),
                new GraphItem("","Choose Board..."),
                new GraphItem("", "Save"),
                new GraphItem("", "Cancel")
            );
            MenuChoice = new IntRange(0, 0, MainMenu.Items.Count - 1);
            CurrentMatchSettings.ReadValues(this);
            MainMenu.SetHighlight(MenuChoice.Value);

            if (CurrentMatchSettings.BoardLocation != null && CurrentMatchSettings.BoardLocation.Trim() != "" && File.Exists(CurrentMatchSettings.BoardLocation))
                LoadBoard(CurrentMatchSettings.BoardLocation);
            else
            {
                BoardThumbnailViewer viewer = new BoardThumbnailViewer(this);
                viewer.Initialize();
                while(!viewer.FoundBoard)
                {
                    viewer.Update();
                }
                LoadBoard(viewer.ValidBoard);
                viewer.Dispose();
            }
#if ZUNE
            SetupZune();
#endif
        }

        public static void SetupZune()
        {
#if ZUNE
            Game1.mainBlade.Status = ZBlade.BladeStatus.In;
            Game1.mainBlade.UserCanCloseMenu = false;

            ZBlade.InfoBlade.BladeHiddenSetup = ZBlade.InfoBlade.BladeInSetup;
            ZBlade.InfoBlade.BladeInSetup = new ZBlade.BladeSetup("Back", "Start", "Game Settings");
#endif
        }

        public static void ResetZune()
        {
#if ZUNE
            Game1.mainBlade.Status = ZBlade.BladeStatus.Hidden;
            ZBlade.InfoBlade.BladeInSetup = ZBlade.InfoBlade.BladeHiddenSetup;
#endif
        }

        public static Texture2D LoadQuickBoard()
        {
            if (Resources.DefaultBoard == null)
            {
                BoardThumbnailViewer viewer = new BoardThumbnailViewer(null);
                viewer.Initialize();
                while (!viewer.FoundBoard)
                {
                    viewer.Update();
                }
                Resources.DefaultBoard = Game1.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + viewer.ValidBoard);
                viewer.Dispose();
            }

            return Resources.DefaultBoard;
        }

        public void Initialize()
        {
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
            if (SetupChars.Count == 1)
            {
                AddComputer();
                PlayerAmt++;
            }
            FeedManager.InitializeFeeds(DialogStrings.LobbyScreenFeed);
        }

        #endregion

        #region Update

        public void Update()
        {
            BackgroundManager.SetRotation(1f);
            if (ViewingSettings)
            {
                if (Input.Players[0].PressedDown)
                {
                    MenuChoice.Add(1);
                    MainMenu.SetHighlight(MenuChoice.Value);
                }
                if (Input.Players[0].PressedUp)
                {
                    MenuChoice.Sub(1);
                    MainMenu.SetHighlight(MenuChoice.Value);
                }

                if (MainMenu.Items[MenuChoice.Value].GetType() == typeof(GraphItemSetting))
                {
                    if (Input.Players[0].PressedLeft)
                    {
                        MainMenu.Items[MenuChoice.Value].ToSetting().ChangeValue(false);
                        MainMenu.CalculateBlocks();
                    }
                    else if (Input.Players[0].PressedRight)
                    {
                        MainMenu.Items[MenuChoice.Value].ToSetting().ChangeValue(true);
                        MainMenu.CalculateBlocks();
                    }
                }
                else
                {
                    if (Input.Players[0].PressedAction)
                    {
                        if (MainMenu.Items[MenuChoice.Value].Details[1] == "Save")
                        {
                            CurrentMatchSettings.SaveValues(this, CurrentBoardLocation);
                            ViewingSettings = false;
#if ZUNE
                            SetupZune();
#endif
                        }
                        else if (MainMenu.Items[MenuChoice.Value].Details[1] == "Cancel")
                        {
                            CurrentMatchSettings.ReadValues(this);
                            ViewingSettings = false;
#if ZUNE
                            SetupZune();
#endif
                        }
                        else
                        {
                            ScreenHelper.ChangeScreen(new BoardThumbnailViewer(this));
                        }
                        BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
                    }
                }
            }
            else
            {
                if (Input.Players[0].PressedAction2)
                {
                    ScreenHelper.ChangeScreen(new CharSelectScreen());
                    ProfileManager.ResetAllBots();
#if ZUNE
                    ResetZune();
#endif
                }

                if (Input.Players[0].PressedUp && SetupChars.Count < MAX_PLAYERS)
                {
                    AddComputer();
                }


                if (Input.Players[0].PressedDown && SetupChars.Count > PlayerAmt)
                {
                    ProfileManager.ResetBot(SetupChars[SetupChars.Count - 1].CharProfile);
                    SetupChars.RemoveAt(SetupChars.Count - 1);
                }

#if ZUNE
                if (Input.Players[0].PressedStart)
                {
                    CurrentMatchSettings.SaveValues(this, CurrentBoardLocation);
                    GameScreen.Instance = new GameScreen(SetupChars);
                    ScreenHelper.ChangeScreen(GameScreen.Instance);
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (Input.Players[0].PressedAction)
                {
                    ViewingSettings = true;
                    ResetZune();
                    
                    BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Normal);
                }


#else
                if (Input.Players[0].PressedLeft || Input.Players[0].PressedRight)
                    ButtonOnLeft = !ButtonOnLeft;

                if (Input.Players[0].PressedAction)
                {
                    if (ButtonOnLeft)
                    {
                        CurrentMatchSettings.SaveValues(this, CurrentBoardLocation);
                        GameScreen.Instance = new GameScreen(SetupChars);
                        ScreenHelper.ChangeScreen(GameScreen.Instance);
                        ProfileManager.ResetAllBots();
                    }
                    else
                    {
                        ViewingSettings = true;
                    }
                }
#endif

            }
        }

        #endregion

        #region Draw
#if !ZUNE
        public void Draw(SpriteBatch batch)
        {
            if (ViewingSettings)
            {
                MainMenu.Draw(batch);
            }
            else
            {
                if (CurrentBoardTexture != null)
                    batch.Draw(CurrentBoardTexture, new Rectangle((int)(640 - Resources.LobbyUnderlay.Width / 2) + 17, (int)(512 - Resources.LobbyUnderlay.Height / 2) + 21, 400, 400), Color.White);
                batch.Draw(Resources.LobbyInfoOverlay.Texture, new Vector2((int)(640 - Resources.LobbyUnderlay.Width / 2) + 17, (int)(512 - Resources.LobbyUnderlay.Height / 2) + 21 + 400 - Resources.LobbyInfoOverlay.Height), Color.White);
               
                Resources.DrawString(DialogStrings.GetDescMsg(), new Vector2((int)(640 - Resources.LobbyUnderlay.Width / 2) + 17 + 9, (int)(512 - Resources.LobbyUnderlay.Height / 2) + 21 + 400 - Resources.LobbyInfoOverlay.Height + 22), Resources.SegoeUIx14pt, FontAlignment.Left, Color.White, true);

                batch.Draw(Resources.LobbyUnderlay.Texture, new Vector2(640 - Resources.LobbyUnderlay.Width / 2, 512 - Resources.LobbyUnderlay.Height / 2), Color.White);

                float YOffset = 330;

                for (int x = 0; x < SetupChars.Count; x++)
                {
                    batch.Draw(Resources.LobbyCharBar.Texture, new Vector2(689, YOffset + 30 * x), Color.White);
                    batch.Draw(Resources.LobbyColorPreview.Texture, new Vector2(689, YOffset + 30 * x), SetupChars[x].PlayerColor);
                    if (SetupChars[x].Type == PlayerType.Player)
                        Resources.DrawString(DialogStrings.Player + (x + 1) + ": " + ProfileManager.AllProfiles[SetupChars[x].CharProfile].Name, new Vector2(725, YOffset + 18 + 30 * x), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
                    else
                        Resources.DrawString(DialogStrings.Player + (x + 1) + ": BOT [ " + ProfileManager.AllProfiles[SetupChars[x].CharProfile].Name + " ]", new Vector2(725, YOffset + 18 + 30 * x), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Red, false);
                }

                batch.Draw(Resources.LobbyOverlay.Texture, new Vector2(640 - Resources.LobbyUnderlay.Width / 2, 512 - Resources.LobbyUnderlay.Height / 2), Color.White);
                batch.Draw(Resources.CPU.Texture, new Vector2(640 - Resources.LobbyUnderlay.Width / 2, 512 - Resources.LobbyUnderlay.Height / 2), Color.White);

                batch.Draw(Resources.LButton.Texture, new Vector2(700, 673), Color.White);
                batch.Draw(Resources.LButton.Texture, new Vector2(845, 673), Color.White);
                Resources.DrawString("Start", new Vector2(700 + 145 / 2, 673 + 18), Resources.SegoeUIx14pt, FontAlignment.Center, Color.White, true);
                Resources.DrawString("Settings", new Vector2(845 + 145 / 2, 673 + 18), Resources.SegoeUIx14pt, FontAlignment.Center, Color.White, true);
                batch.Draw(Resources.LButtonHT.Texture, new Vector2((ButtonOnLeft ? 700 : 845), 673), Color.White);
                //Resources.DrawString(Dialogs[0], new Vector2(700, 336), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black,false);
                //Resources.DrawString(Dialogs[1], new Vector2(700, 698), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black,false);
            }
        }
#else
        public void Draw(SpriteBatch batch)
        {
            if (ViewingSettings)
            {
                MainMenu.Draw(batch);
            }
            else
            {
                batch.Draw(Resources.LobbyUnderlay.Texture, Vector2.Zero, Color.White);
                float YOffset = 75;

                for (int x = 0; x < SetupChars.Count; x++)
                {
                    batch.Draw(Resources.LobbyCharBar.Texture, new Vector2(0, YOffset + 30 * x), Color.White);
                    batch.Draw(Resources.LobbyColorPreview.Texture, new Vector2(0, YOffset + 30 * x), SetupChars[x].PlayerColor);
                    if (SetupChars[x].Type == PlayerType.Player)
                        Resources.DrawString(DialogStrings.Player + (x + 1) + ": " + ProfileManager.AllProfiles[SetupChars[x].CharProfile].Name, new Vector2(36, YOffset + 18 + 30 * x), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
                    else
                        Resources.DrawString(DialogStrings.Player + (x + 1) + ": *" + ProfileManager.AllProfiles[SetupChars[x].CharProfile].Name + "*", new Vector2(36, YOffset + 18 + 30 * x), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Red, false);
                }
                batch.Draw(Resources.LobbyOverlay.Texture, Vector2.Zero, Color.White);
            }
        }
#endif
        #endregion

        #region Dispose

        public void Dispose()
        {
            CurrentBoardTexture = null;
            Resources.LobbyUnderlay.Dispose();
            Resources.LobbyCharBar.Dispose();
            Resources.LobbyColorPreview.Dispose();
            Resources.LobbyOverlay.Dispose();
        }

        #endregion

        #region Extra Methods

        /// <summary>
        /// Loads the new Board Texture and loads its name/creator.
        /// </summary>
        public void LoadBoard(string brdloc)
        {
            if (CurrentBoardTexture != null && CurrentBoardLocation == brdloc)
            {

            }
            else
            {
                try
                {
                    CurrentBoardTexture = Game1.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + brdloc);
                    CurrentBoardLocation = brdloc;
                }
                catch (FileNotFoundException)
                {

                }
                Dialogs[0] = DialogStrings.CurrentBoard + CurrentBoardLocation.Substring(CurrentBoardLocation.IndexOf('_') + 1).Replace(".png", "").Replace("boards\\", "");
                if (CurrentBoardLocation.IndexOf('_') >= 0)
                    Dialogs[1] = DialogStrings.CreatedBy + CurrentBoardLocation.Substring(0, CurrentBoardLocation.IndexOf('_')).Replace(".png", "").Replace("boards\\", "");
                else
                    Dialogs[1] = "";

                Resources.DefaultBoard = CurrentBoardTexture;
            }
        }

        /// <summary>
        /// Adds a new computer player.
        /// </summary>
        private void AddComputer()
        {
            SetupChars.Add(new CharacterShell(CharSelectScreen.ReturnRandSkin(), ProfileManager.GetBotProfile(), (ExtendedPlayerIndex)SetupChars.Count, PlayerType.Computer, Resources.PlayerColors[SetupChars.Count]));
        }

        #endregion
    }
}
