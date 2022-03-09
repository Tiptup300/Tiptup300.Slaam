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
        public bool Survival = false;
        public PlayerCharacterSelectBoxStatus CurrentState = PlayerCharacterSelectBoxStatus.Computer;

        private const float _scrollSpeed = 4f / 35f;
        private List<string> _parentSkinStrings = new List<string>();
        private int _playerIDX;
        private float _offset;
        private IntRange _chosenSkin = new IntRange(0);
        private IntRange _chosenProfile;
        private PlayerCharacterSelectBoxMovementStatus _status = PlayerCharacterSelectBoxMovementStatus.Stationary;
        private Texture2D[] _displayResources = new Texture2D[3];
        private Texture2D[] _parentCharSkins;
        private Vector2[] _positions = new Vector2[10];
        private string[] _messageLines = new string[6];

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
            _playerIDX = InputComponent.GetIndex(playeridx);
            _parentCharSkins = parentcharskins;
            _parentSkinStrings = parentskinstrings;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _refreshSkins();

            _positions[0] = Position;
            _positions[1] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + _offset - 70);
            _positions[2] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + _offset);
            _positions[3] = new Vector2(Position.X + 75, Position.Y + 125 - 30 + _offset + 70);
            _positions[4] = new Vector2(Position.X + 175, Position.Y + 108);
            _positions[5] = new Vector2(Position.X + 188, Position.Y + 77);

            _positions[6] = new Vector2(Position.X + 209, Position.Y + 146);
            _positions[7] = new Vector2(Position.X + 412, Position.Y + 146);
            _positions[8] = new Vector2(Position.X + 209, Position.Y + 188);
            _positions[9] = new Vector2(Position.X + 412, Position.Y + 188);

            _messageLines[0] = DialogStrings.Player + (ExtendedPlayerIndex)_playerIDX;
            _messageLines[1] = DialogStrings.PressStartToJoin;
            _messageLines[2] = "";
            _messageLines[3] = "";
            _messageLines[4] = "";
            _messageLines[5] = "";
        }

        public void Reset()
        {
            if (_chosenProfile == null || ProfileManager.PlayableProfiles.Count - 1 != _chosenProfile.Max)
                _chosenProfile = new IntRange(0, 0, ProfileManager.PlayableProfiles.Count - 1);
        }

        public void Update()
        {
            switch (CurrentState)
            {
                case PlayerCharacterSelectBoxStatus.Computer:
                    {
                        if (InputComponent.Players[_playerIDX].PressedStart)
                        {
                            CurrentState = PlayerCharacterSelectBoxStatus.ProfileSelect;
                            _messageLines[1] = DialogStrings.SelectAProfile;
                            _messageLines[0] = ProfileManager.PlayableProfiles[_chosenProfile.Value].Name;
                            _resetStats();
                        }
                    }
                    break;

                case PlayerCharacterSelectBoxStatus.ProfileSelect:
                    {
                        if (InputComponent.Players[_playerIDX].PressedAction2)
                        {
                            _messageLines[0] = DialogStrings.Player + (ExtendedPlayerIndex)_playerIDX;
                            _messageLines[1] = DialogStrings.PressStartToJoin;
                            _messageLines[2] = "";
                            _messageLines[3] = "";
                            _messageLines[4] = "";
                            _messageLines[5] = "";
                            CurrentState = PlayerCharacterSelectBoxStatus.Computer;
                        }

                        if (InputComponent.Players[_playerIDX].PressedUp)
                        {
                            _chosenProfile.Add(1);
                            _messageLines[0] = ProfileManager.PlayableProfiles[_chosenProfile.Value].Name;
                            _resetStats();
                        }

                        if (InputComponent.Players[_playerIDX].PressedDown)
                        {
                            _chosenProfile.Sub(1);
                            _messageLines[0] = ProfileManager.PlayableProfiles[_chosenProfile.Value].Name;
                            _resetStats();
                        }

                        if (InputComponent.Players[_playerIDX].PressedAction)
                        {
                            CurrentState = PlayerCharacterSelectBoxStatus.CharSelect;
                            _findSkin(ProfileManager.PlayableProfiles[_chosenProfile.Value].Skin);
                            _messageLines[1] = DialogStrings.PlayingAs + _parentSkinStrings[_chosenSkin.Value].Substring(_parentSkinStrings[_chosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                        }
                    }
                    break;

                case PlayerCharacterSelectBoxStatus.CharSelect:
                    {
                        if (InputComponent.Players[_playerIDX].PressedAction2)
                        {
                            _messageLines[1] = DialogStrings.SelectAProfile;
                            CurrentState = PlayerCharacterSelectBoxStatus.ProfileSelect;
                        }

                        if (InputComponent.Players[_playerIDX].PressingUp && _status == PlayerCharacterSelectBoxMovementStatus.Stationary)
                        {
                            _status = PlayerCharacterSelectBoxMovementStatus.Lowering;
                        }
                        else if (InputComponent.Players[_playerIDX].PressingDown && _status == PlayerCharacterSelectBoxMovementStatus.Stationary)
                        {
                            _status = PlayerCharacterSelectBoxMovementStatus.Raising;
                        }

                        if (InputComponent.Players[_playerIDX].PressedAction && CurrentState == PlayerCharacterSelectBoxStatus.CharSelect)
                        {
                            ProfileManager.PlayableProfiles[_chosenProfile.Value].Skin = _parentSkinStrings[_chosenSkin.Value];
                            ProfileManager.SaveProfiles();
                            CurrentState = PlayerCharacterSelectBoxStatus.Done;
                        }

                        if (_status != PlayerCharacterSelectBoxMovementStatus.Stationary)
                        {
                            if (_status == PlayerCharacterSelectBoxMovementStatus.Lowering)
                                _offset -= FrameRateDirector.MovementFactor * _scrollSpeed;
                            else
                                _offset += FrameRateDirector.MovementFactor * _scrollSpeed;

                            _positions[1] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset - 70);
                            _positions[2] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset);
                            _positions[3] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset + 70);

                        }

                        if (_offset >= 70)
                        {
                            _chosenSkin.Add(1, 0, _parentCharSkins.Length - 1);
                            _refreshSkins();
                            _offset = 0;
                            _status = PlayerCharacterSelectBoxMovementStatus.Stationary;
                            _messageLines[1] = DialogStrings.PlayingAs + _parentSkinStrings[_chosenSkin.Value].Substring(_parentSkinStrings[_chosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                            _positions[1] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset - 70);
                            _positions[2] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset);
                            _positions[3] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset + 70);
                        }
                        else if (_offset <= -70)
                        {

                            _chosenSkin.Sub(1, 0, _parentCharSkins.Length - 1);
                            _refreshSkins();
                            _offset = 0;
                            _status = PlayerCharacterSelectBoxMovementStatus.Stationary;
                            _messageLines[1] = DialogStrings.PlayingAs + _parentSkinStrings[_chosenSkin.Value].Substring(_parentSkinStrings[_chosenSkin.Value].IndexOf('_') + 1).Replace(".png", "").Replace("skins\\", "");
                            _positions[1] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset - 70);
                            _positions[2] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset);
                            _positions[3] = new Vector2(_positions[0].X + 75, _positions[0].Y + 125 - 30 + _offset + 70);
                        }
                    }
                    break;

            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("ProfileShell").Texture, new Vector2(13, 96), Color.White);

            var temp = _messageLines[1];

            if (CurrentState == PlayerCharacterSelectBoxStatus.CharSelect)
            {
                batch.Draw(_displayResources[1], new Vector2(152, 239), new Rectangle(0, 0, 50, 60), Color.White);
                temp = temp.Substring(DialogStrings.PlayingAs.Length);
            }

            RenderGraph.Instance.RenderText(temp, new Vector2(31, 141), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
            RenderGraph.Instance.RenderText(_messageLines[0], new Vector2(20, 70), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
        }

        private void _refreshSkins()
        {
            _displayResources[0] = _parentCharSkins[_chosenSkin.NextValue(1, 0, _parentCharSkins.Length - 1)];
            _displayResources[1] = _parentCharSkins[_chosenSkin.Value];
            _displayResources[2] = _parentCharSkins[_chosenSkin.PreviousValue(1, 0, _parentCharSkins.Length - 1)];
        }

        private void _resetStats()
        {
            if (Survival)
            {
                _messageLines[2] = DialogStrings.BestTime + NormalStatsBoard.TimeSpanToString(ProfileManager.PlayableProfiles[_chosenProfile.Value].BestGame);
                _messageLines[3] = "";
                _messageLines[4] = "";
                _messageLines[5] = "";
            }
            else
            {
                _messageLines[2] = "" + ProfileManager.PlayableProfiles[_chosenProfile.Value].TotalGames;
                _messageLines[3] = "" + ProfileManager.PlayableProfiles[_chosenProfile.Value].TotalDeaths;
                _messageLines[4] = "" + ProfileManager.PlayableProfiles[_chosenProfile.Value].TotalPowerups;
                _messageLines[5] = "" + ProfileManager.PlayableProfiles[_chosenProfile.Value].TotalKills;
            }
        }

        private void _findSkin(string str)
        {
            for (int x = 0; x < _parentSkinStrings.Count; x++)
            {
                if (_parentSkinStrings[x] == str)
                {
                    _chosenSkin.Value = x;
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
                skinLocation: _parentSkinStrings[_chosenSkin.Value],
                characterProfileIndex: ProfileManager.PlayableProfiles.GetRealIndex(_chosenProfile.Value),
                playerIndex: (ExtendedPlayerIndex)_playerIDX,
                playerType: type,
                playerColor: _playerColorResolver.GetColorByIndex(_playerIDX));
        }


    }
}
