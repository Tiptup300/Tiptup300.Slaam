using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
{
    public class CharSelectBox
    {
        #region Variables

        private const float ScrollSpeed = (4f / 35f);

        private List<String> ParentSkinStrings = new List<string>();

        private int PlayerIDX;
        private int SelectedIndex = -1;

        private float Offset;

        private IntRange ChosenSkin = new IntRange(0);
        private IntRange ChosenProfile;

        private Status CurrentStatus = Status.Stationary;
        public CharSelectBoxState CurrentState = CharSelectBoxState.Computer;

        private Texture2D[] DispResources = new Texture2D[3];
        private Texture2D[] ParentCharSkins;

        private Vector2[] Positions = new Vector2[10];
        private String[] MsgStrings = new string[6];

        public bool Survival = false;

        #endregion

        #region Constructor

        public CharSelectBox(Vector2 Position, Texture2D[] parentcharskins, ExtendedPlayerIndex playeridx, List<String> parentskinstrings)
        {
            PlayerIDX = Input.GetIndex(playeridx);
            ParentCharSkins = parentcharskins;
            ParentSkinStrings = parentskinstrings;
            RefreshSkins();

            Positions[0] = Position;
            Positions[1] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + Offset - 70);
            Positions[2] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + Offset);
            Positions[3] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + Offset + 70);
            Positions[4] = new Vector2(Position.X + 175, Position.Y + 108);
            Positions[5] = new Vector2(Position.X + 188, Position.Y + 77);

            Positions[6] = new Vector2(Position.X + 209, Position.Y + 146);
            Positions[7] = new Vector2(Position.X + 412, Position.Y + 146);
            Positions[8] = new Vector2(Position.X + 209, Position.Y + 188);
            Positions[9] = new Vector2(Position.X + 412, Position.Y + 188);

            MsgStrings[0] = DialogStrings.Player+(ExtendedPlayerIndex)PlayerIDX;
            MsgStrings[1] = DialogStrings.PressStartToJoin;
            MsgStrings[2] = "";
            MsgStrings[3] = "";
            MsgStrings[4] = "";
            MsgStrings[5] = "";
        }

        public void Reset()
        {
            if(ChosenProfile == null || ProfileManager.PlayableProfiles.Count-1 != ChosenProfile.Max)
                ChosenProfile = new IntRange(0, 0, ProfileManager.PlayableProfiles.Count-1);
        }

        #endregion

        #region Update

        public void Update()
        {
            switch (CurrentState)
            {
                case CharSelectBoxState.Computer:
                    {
                        if (Input.Players[(int)PlayerIDX].PressedStart)
                        {
                            CurrentState = CharSelectBoxState.ProfileSelect;
                            MsgStrings[1] = DialogStrings.SelectAProfile;
                            MsgStrings[0] = ProfileManager.PlayableProfiles[ChosenProfile.Value].Name;
                            ResetStats();
                        }
                    }
                    break;

                case CharSelectBoxState.ProfileSelect:
                    {
                        if (Input.Players[(int)PlayerIDX].PressedAction2)
                        {
                            MsgStrings[0] = DialogStrings.Player + (ExtendedPlayerIndex)PlayerIDX;
                            MsgStrings[1] = DialogStrings.PressStartToJoin;
                            MsgStrings[2] = "";
                            MsgStrings[3] = "";
                            MsgStrings[4] = "";
                            MsgStrings[5] = "";
                            CurrentState = CharSelectBoxState.Computer;
                        }

                        if (Input.Players[(int)PlayerIDX].PressedUp)
                        {
                            ChosenProfile.Add(1);
                            MsgStrings[0] = ProfileManager.PlayableProfiles[ChosenProfile.Value].Name;
                            ResetStats();
                        }

                        if (Input.Players[(int)PlayerIDX].PressedDown)
                        {
                            ChosenProfile.Sub(1);
                            MsgStrings[0] = ProfileManager.PlayableProfiles[ChosenProfile.Value].Name;
                            ResetStats();
                        }
                        
                        if (Input.Players[(int)PlayerIDX].PressedAction)
                        {
                            CurrentState = CharSelectBoxState.CharSelect;
                            FindSkin(ProfileManager.PlayableProfiles[ChosenProfile.Value].Skin);
                            MsgStrings[1] = DialogStrings.PlayingAs + ParentSkinStrings[ChosenSkin.Value].Substring(ParentSkinStrings[ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                        }
                    }
                    break;

                case CharSelectBoxState.CharSelect:
                    {
                        if (Input.Players[(int)PlayerIDX].PressedAction2)
                        {
                            MsgStrings[1] = DialogStrings.SelectAProfile;
                            CurrentState = CharSelectBoxState.ProfileSelect;
                        }

                        if (Input.Players[(int)PlayerIDX].PressingUp && CurrentStatus == Status.Stationary)
                        {
                            CurrentStatus = Status.Lowering;
                        }
                        else if (Input.Players[(int)PlayerIDX].PressingDown && CurrentStatus == Status.Stationary)
                        {
                            CurrentStatus = Status.Raising;
                        }

                        if (Input.Players[(int)PlayerIDX].PressedAction && CurrentState == CharSelectBoxState.CharSelect)
                        {
                            ProfileManager.PlayableProfiles[ChosenProfile.Value].Skin = ParentSkinStrings[ChosenSkin.Value];
                            ProfileManager.SaveProfiles();
                            SelectedIndex = ChosenSkin.Value;
                            CurrentState = CharSelectBoxState.Done;
                        }

                        if (CurrentStatus != Status.Stationary)
                        {
                                if (CurrentStatus == Status.Lowering)
                                    Offset -= FPSManager.MovementFactor * ScrollSpeed;
                                else
                                    Offset += FPSManager.MovementFactor * ScrollSpeed;

                                Positions[1] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset - 70);
                                Positions[2] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset);
                                Positions[3] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset + 70);
                            
                        }

                        if (Offset >= 70)
                        {
                            ChosenSkin.Add(1, 0, ParentCharSkins.Length - 1);
                            RefreshSkins();
                            Offset = 0;
                            CurrentStatus = Status.Stationary;
                            MsgStrings[1] = DialogStrings.PlayingAs + ParentSkinStrings[ChosenSkin.Value].Substring(ParentSkinStrings[ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                            Positions[1] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset - 70);
                            Positions[2] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset);
                            Positions[3] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset + 70);
                        }
                        else if (Offset <= -70)
                        {

                            ChosenSkin.Sub(1, 0, ParentCharSkins.Length - 1);
                            RefreshSkins();
                            Offset = 0;
                            CurrentStatus = Status.Stationary;
                            MsgStrings[1] = DialogStrings.PlayingAs + ParentSkinStrings[ChosenSkin.Value].Substring(ParentSkinStrings[ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                            Positions[1] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset - 70);
                            Positions[2] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset);
                            Positions[3] = new Vector2(Positions[0].X + 75, Positions[0].Y + 125 - 30 + Offset + 70);
                        }
                    }
                    break;

            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch) // 387
        {
#if !ZUNE
            batch.Draw(Resources.ProfileShell.Texture, Positions[0], Color.White);

            if (CurrentState == CharSelectBoxState.CharSelect)
            {
                if (Offset > -8)
                    batch.Draw(DispResources[0], Positions[1], new Rectangle(0, 0, 50, 60), Color.White);
                batch.Draw(DispResources[1], Positions[2], new Rectangle(0, 0, 50, 60), Color.White);
                if (Offset < 8)
                    batch.Draw(DispResources[2], Positions[3], new Rectangle(0, 0, 50, 60), Color.White);
            }
            batch.Draw(Resources.ProfileShello.Texture, Positions[0], Color.White);

            Resources.DrawString(MsgStrings[1], Positions[4], Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
            Resources.DrawString(MsgStrings[0], Positions[5], Resources.SegoeUIx32pt, FontAlignment.Left, Color.Black, false);
            
            // Stats
            Resources.DrawString(MsgStrings[2], Positions[6], Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
            Resources.DrawString(MsgStrings[3], Positions[7], Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
            Resources.DrawString(MsgStrings[4], Positions[8], Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
            Resources.DrawString(MsgStrings[5], Positions[9], Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
#else
            batch.Draw(Resources.ProfileShell.Texture, new Vector2(13, 96), Color.White);
            
            var temp = MsgStrings[1];

            if (CurrentState == CharSelectBoxState.CharSelect)
            {
                batch.Draw(DispResources[1], new Vector2(152,239), new Rectangle(0, 0, 50, 60), Color.White);
                temp = temp.Substring(DialogStrings.PlayingAs.Length);
            }


            Resources.DrawString(temp, new Vector2(31,141), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
            Resources.DrawString(MsgStrings[0], new Vector2(20, 70), Resources.SegoeUIx14pt, FontAlignment.Left, Color.Black, false);
#endif
        }

        #endregion

        #region Info Reset Methods

        /// <summary>
        /// Sets the new skins for display
        /// </summary>
        public void RefreshSkins()
        {
            DispResources[0] = ParentCharSkins[ChosenSkin.NextValue(1, 0, ParentCharSkins.Length - 1)];
            DispResources[1] = ParentCharSkins[ChosenSkin.Value];
            DispResources[2] = ParentCharSkins[ChosenSkin.PreviousValue(1, 0, ParentCharSkins.Length - 1)];
        }

        /// <summary>
        /// Resets the current stats when the profile changes
        /// </summary>
        public void ResetStats()
        {
            if (Survival)
            {
                MsgStrings[2] = DialogStrings.BestTime + NormalStatsBoard.TimeSpanToString(ProfileManager.PlayableProfiles[ChosenProfile.Value].BestGame);
                MsgStrings[3] = "";
                MsgStrings[4] = "";
                MsgStrings[5] = "";
            }
            else
            {
#if !ZUNE
                MsgStrings[2] = DialogStrings.Played + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalGames + DialogStrings.Games;
                MsgStrings[3] = DialogStrings.Died + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalDeaths + DialogStrings.Times;
                MsgStrings[4] = DialogStrings.Used + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalPowerups + DialogStrings.Powerups;
                MsgStrings[5] = ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalKills + DialogStrings.Kills;
#else
                MsgStrings[2] = "" + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalGames;
                MsgStrings[3] = "" + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalDeaths;
                MsgStrings[4] = "" + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalPowerups;
                MsgStrings[5] = "" + ProfileManager.PlayableProfiles[ChosenProfile.Value].TotalKills;
#endif
                }
        }

        #endregion

        #region Find Skin Method

        /// <summary>
        /// Finds the inserted skin, if its not found it gets a random one.
        /// </summary>
        /// <param name="str">The Skin its looking for.</param>
        private void FindSkin(String str)
        {
            for (int x = 0; x < ParentSkinStrings.Count; x++)
            {
                if (ParentSkinStrings[x] == str)
                {
                    ChosenSkin.Value = x;
                    break;
                }
            }
            RefreshSkins();
        }

        #endregion

        #region Get Shell Method

        /// <summary>
        /// Gets a characters base information into a simple class for the gamescreen.
        /// </summary>
        /// <returns></returns>
        public CharacterShell GetShell()
        {
            PlayerType type = PlayerType.Computer;
            if (CurrentState != CharSelectBoxState.Computer)
                type = PlayerType.Player;
            return new CharacterShell(ParentSkinStrings[ChosenSkin.Value], ProfileManager.PlayableProfiles.GetRealIndex(ChosenProfile.Value), (ExtendedPlayerIndex)PlayerIDX, type, Resources.PlayerColors[PlayerIDX]);
        }

        #endregion

        #region Enums

        public enum Status
        {
            Lowering,
            Stationary,
            Raising,
        }

        #endregion
    }

    #region Enums

    public enum CharSelectBoxState
    {
        Computer,
        ProfileSelect,
        CharSelect,
        Done,
    }

    #endregion
}
