using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.PlayerProfiles;
using SlaamMono.StatsBoards;
using System.Collections.Generic;

namespace SlaamMono.MatchCreation
{
    public class PlayerCharacterSelectBox
    {
        public bool Survival { get => _state.Survival; set { _state.Survival = value; } }
        public PlayerCharacterSelectBoxStatus CurrentState { get => _state.Status; set { _state.Status = value; } }

        private PlayerCharacterSelectBoxState _state = new PlayerCharacterSelectBoxState();

        private readonly PlayerColorResolver _playerColorResolver;
        private readonly IResources _resources;

        public PlayerCharacterSelectBox(
            Vector2 Position,
            Texture2D[] parentcharskins,
            ExtendedPlayerIndex playeridx,
            List<string> parentskinstrings,
            PlayerColorResolver playerColorResolver,
            IResources resources)
        {
            _state.PlayerIndex = InputComponent.GetIndex(playeridx);
            _state.ParentCharSkins = parentcharskins;
            _state.ParentSkinStrings = parentskinstrings;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _refreshSkins();

            _state.Positions[0] = Position;
            _state.Positions[1] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + _state.Offset - 70);
            _state.Positions[2] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + _state.Offset);
            _state.Positions[3] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + _state.Offset + 70);
            _state.Positions[4] = new Vector2(Position.X + 175, Position.Y + 108);
            _state.Positions[5] = new Vector2(Position.X + 188, Position.Y + 77);

            _state.Positions[6] = new Vector2(Position.X + 209, Position.Y + 146);
            _state.Positions[7] = new Vector2(Position.X + 412, Position.Y + 146);
            _state.Positions[8] = new Vector2(Position.X + 209, Position.Y + 188);
            _state.Positions[9] = new Vector2(Position.X + 412, Position.Y + 188);

            _state.MessageLines[0] = DialogStrings.Player + (ExtendedPlayerIndex)_state.PlayerIndex;
            _state.MessageLines[1] = DialogStrings.PressStartToJoin;
            _state.MessageLines[2] = "";
            _state.MessageLines[3] = "";
            _state.MessageLines[4] = "";
            _state.MessageLines[5] = "";
        }

        public void Reset()
        {
            if (_state.ChosenProfile == null || ProfileManager.PlayableProfiles.Count - 1 != _state.ChosenProfile.Max)
                _state.ChosenProfile = new IntRange(0, 0, ProfileManager.PlayableProfiles.Count - 1);
        }

        public void Update()
        {
            switch (CurrentState)
            {
                case PlayerCharacterSelectBoxStatus.Computer:
                    {
                        if (InputComponent.Players[_state.PlayerIndex].PressedStart)
                        {
                            CurrentState = PlayerCharacterSelectBoxStatus.ProfileSelect;
                            _state.MessageLines[1] = DialogStrings.SelectAProfile;
                            _state.MessageLines[0] = ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].Name;
                            _resetStats();
                        }
                    }
                    break;

                case PlayerCharacterSelectBoxStatus.ProfileSelect:
                    {
                        if (InputComponent.Players[_state.PlayerIndex].PressedAction2)
                        {
                            _state.MessageLines[0] = DialogStrings.Player + (ExtendedPlayerIndex)_state.PlayerIndex;
                            _state.MessageLines[1] = DialogStrings.PressStartToJoin;
                            _state.MessageLines[2] = "";
                            _state.MessageLines[3] = "";
                            _state.MessageLines[4] = "";
                            _state.MessageLines[5] = "";
                            CurrentState = PlayerCharacterSelectBoxStatus.Computer;
                        }

                        if (InputComponent.Players[_state.PlayerIndex].PressedUp)
                        {
                            _state.ChosenProfile.Add(1);
                            _state.MessageLines[0] = ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].Name;
                            _resetStats();
                        }

                        if (InputComponent.Players[_state.PlayerIndex].PressedDown)
                        {
                            _state.ChosenProfile.Sub(1);
                            _state.MessageLines[0] = ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].Name;
                            _resetStats();
                        }

                        if (InputComponent.Players[_state.PlayerIndex].PressedAction)
                        {
                            CurrentState = PlayerCharacterSelectBoxStatus.CharSelect;
                            _findSkin(ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].Skin);
                            _state.MessageLines[1] = DialogStrings.PlayingAs + _state.ParentSkinStrings[_state.ChosenSkin.Value].Substring(_state.ParentSkinStrings[_state.ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                        }
                    }
                    break;

                case PlayerCharacterSelectBoxStatus.CharSelect:
                    {
                        if (InputComponent.Players[_state.PlayerIndex].PressedAction2)
                        {
                            _state.MessageLines[1] = DialogStrings.SelectAProfile;
                            CurrentState = PlayerCharacterSelectBoxStatus.ProfileSelect;
                        }

                        if (InputComponent.Players[_state.PlayerIndex].PressingUp && _state.MovementStatus == PlayerCharacterSelectBoxMovementStatus.Stationary)
                        {
                            _state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Lowering;
                        }
                        else if (InputComponent.Players[_state.PlayerIndex].PressingDown && _state.MovementStatus == PlayerCharacterSelectBoxMovementStatus.Stationary)
                        {
                            _state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Raising;
                        }

                        if (InputComponent.Players[_state.PlayerIndex].PressedAction && CurrentState == PlayerCharacterSelectBoxStatus.CharSelect)
                        {
                            ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].Skin = _state.ParentSkinStrings[_state.ChosenSkin.Value];
                            ProfileManager.SaveProfiles();
                            CurrentState = PlayerCharacterSelectBoxStatus.Done;
                        }

                        if (_state.MovementStatus != PlayerCharacterSelectBoxMovementStatus.Stationary)
                        {
                            if (_state.MovementStatus == PlayerCharacterSelectBoxMovementStatus.Lowering)
                                _state.Offset -= FrameRateDirector.MovementFactor * _state.ScrollSpeed;
                            else
                                _state.Offset += FrameRateDirector.MovementFactor * _state.ScrollSpeed;

                            _state.Positions[1] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset - 70);
                            _state.Positions[2] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset);
                            _state.Positions[3] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset + 70);

                        }

                        if (_state.Offset >= 70)
                        {
                            _state.ChosenSkin.Add(1, 0, _state.ParentCharSkins.Length - 1);
                            _refreshSkins();
                            _state.Offset = 0;
                            _state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Stationary;
                            _state.MessageLines[1] = DialogStrings.PlayingAs + _state.ParentSkinStrings[_state.ChosenSkin.Value].Substring(_state.ParentSkinStrings[_state.ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                            _state.Positions[1] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset - 70);
                            _state.Positions[2] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset);
                            _state.Positions[3] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset + 70);
                        }
                        else if (_state.Offset <= -70)
                        {

                            _state.ChosenSkin.Sub(1, 0, _state.ParentCharSkins.Length - 1);
                            _refreshSkins();
                            _state.Offset = 0;
                            _state.MovementStatus = PlayerCharacterSelectBoxMovementStatus.Stationary;
                            _state.MessageLines[1] = DialogStrings.PlayingAs + _state.ParentSkinStrings[_state.ChosenSkin.Value].Substring(_state.ParentSkinStrings[_state.ChosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                            _state.Positions[1] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset - 70);
                            _state.Positions[2] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset);
                            _state.Positions[3] = new Vector2(_state.Positions[0].X + 75, _state.Positions[0].Y + 125 - 30 + _state.Offset + 70);
                        }
                    }
                    break;

            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("ProfileShell").Texture, new Vector2(13, 96), Color.White);

            var temp = _state.MessageLines[1];

            if (CurrentState == PlayerCharacterSelectBoxStatus.CharSelect)
            {
                batch.Draw(_state.DisplayResources[1], new Vector2(152, 239), new Rectangle(0, 0, 50, 60), Color.White);
                temp = temp.Substring(DialogStrings.PlayingAs.Length);
            }

            RenderGraph.Instance.RenderText(temp, new Vector2(31, 141), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
            RenderGraph.Instance.RenderText(_state.MessageLines[0], new Vector2(20, 70), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
        }

        private void _refreshSkins()
        {
            _state.DisplayResources[0] = _state.ParentCharSkins[_state.ChosenSkin.NextValue(1, 0, _state.ParentCharSkins.Length - 1)];
            _state.DisplayResources[1] = _state.ParentCharSkins[_state.ChosenSkin.Value];
            _state.DisplayResources[2] = _state.ParentCharSkins[_state.ChosenSkin.PreviousValue(1, 0, _state.ParentCharSkins.Length - 1)];
        }

        private void _resetStats()
        {
            if (Survival)
            {
                _state.MessageLines[2] = DialogStrings.BestTime + NormalStatsBoard.TimeSpanToString(ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].BestGame);
                _state.MessageLines[3] = "";
                _state.MessageLines[4] = "";
                _state.MessageLines[5] = "";
            }
            else
            {
                _state.MessageLines[2] = "" + ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].TotalGames;
                _state.MessageLines[3] = "" + ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].TotalDeaths;
                _state.MessageLines[4] = "" + ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].TotalPowerups;
                _state.MessageLines[5] = "" + ProfileManager.PlayableProfiles[_state.ChosenProfile.Value].TotalKills;
            }
        }

        private void _findSkin(string str)
        {
            for (int x = 0; x < _state.ParentSkinStrings.Count; x++)
            {
                if (_state.ParentSkinStrings[x] == str)
                {
                    _state.ChosenSkin.Value = x;
                    break;
                }
            }
            _refreshSkins();
        }

        public CharacterShell GetShell()
        {
            PlayerType type = PlayerType.Computer;
            if (CurrentState != PlayerCharacterSelectBoxStatus.Computer)
            {
                type = PlayerType.Player;
            }
            return new CharacterShell(
                skinLocation: _state.ParentSkinStrings[_state.ChosenSkin.Value],
                characterProfileIndex: ProfileManager.PlayableProfiles.GetRealIndex(_state.ChosenProfile.Value),
                playerIndex: (ExtendedPlayerIndex)_state.PlayerIndex,
                playerType: type,
                playerColor: _playerColorResolver.GetColorByIndex(_state.PlayerIndex));
        }


    }
}
