using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.x_;
using System;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreen : IStateUpdater
    {
        public bool x_HasFoundBoard { get => _state.HasFoundBoard; }
        public string x_IsValidBoard { get => _state.IsValidBoard; }

        private BoardSelectionScreenState _state = new BoardSelectionScreenState();

        private readonly IScreenManager _screenManager;
        private readonly IResources _resources;

        public BoardSelectionScreen(IResources resources, IScreenManager screenManager)
        {
            _resources = resources;
            _screenManager = screenManager;
        }

        public void Initialize(BoardSelectionScreenRequest request)
        {
            _state.ParentLobbyScreen = request.ParentScreen;
        }

        public void InitializeState()
        {
            _state.BoardNames = _resources.GetTextList("Boards");
            setBoardIndexs();
            BackgroundManager.ChangeBG(BackgroundType.Normal);
        }

        private void setBoardIndexs()
        {
            _state.DrawingBoardIndex = new IntRange(0, 0, _state.BoardNames.Count - 1);
            _state.VerticalBoardOffset = new IntRange(0);
            _state.HorizontalBoardOffset = new IntRange(-_state.BoardNames.Count);
        }

        public void UpdateState()
        {
            if (_state.IsStillLoadingBoards)
            {
                ContinueLoadingBoards();
                return;
            }

            _state.Alpha += (_state.AlphaUp ? 1 : -1) * FrameRateDirector.MovementFactor * _state.MovementSpeed;

            if (_state.AlphaUp && _state.Alpha >= 255f)
            {
                _state.AlphaUp = !_state.AlphaUp;
                _state.Alpha = 255f;
            }
            else if (!_state.AlphaUp && _state.Alpha <= 0f)
            {
                _state.AlphaUp = !_state.AlphaUp;
                _state.Alpha = 0f;
            }

            if (_state.WasChosen)
            {
                _state.Scale += FrameRateDirector.MovementFactor * .01f;

                if (_state.Scale >= 1.50f)
                {
                    _state.Scale = 1.50f;
                }

                if (InputComponent.Players[0].PressedAction)
                {
                    _state.ParentLobbyScreen.LoadBoard(_state.ValidBoards[_state.Save]);
                    _screenManager.ChangeTo(_state.ParentLobbyScreen);
                }

                if (InputComponent.Players[0].PressedAction2)
                {
                    _state.WasChosen = false;
                }
            }
            else
            {
                _state.Scale -= FrameRateDirector.MovementFactor * .01f;

                if (_state.Scale <= 1.00f)
                {
                    _state.Scale = 1.00f;
                }

                if (InputComponent.Players[0].PressingDown)
                {
                    _state.Vertical = Direction.Down;
                }
                if (InputComponent.Players[0].PressingUp)
                {
                    _state.Vertical = Direction.Up;
                }

                if (InputComponent.Players[0].PressingRight)
                {
                    _state.Horizontal = Direction.Right;
                }
                if (InputComponent.Players[0].PressingLeft)
                {
                    _state.Horizontal = Direction.Left;
                }

                if (_state.Vertical == Direction.Down)
                {
                    _state.VerticalOffset -= FrameRateDirector.MovementFactor * _state.MovementSpeed;

                    if (_state.VerticalOffset <= -_state.DrawSizeHeight)
                    {
                        _state.VerticalBoardOffset.Add(1);
                        _state.VerticalOffset = 0;
                        _state.Vertical = Direction.None;
                    }
                }
                else if (_state.Vertical == Direction.Up)
                {
                    _state.VerticalOffset += FrameRateDirector.MovementFactor * _state.MovementSpeed;

                    if (_state.VerticalOffset >= _state.DrawSizeHeight)
                    {
                        _state.VerticalBoardOffset.Sub(1);
                        _state.VerticalOffset = 0;
                        _state.Vertical = Direction.None;
                    }
                }

                if (_state.Horizontal == Direction.Left)
                {
                    _state.HorizontalOffset += FrameRateDirector.MovementFactor * _state.MovementSpeed;

                    if (_state.HorizontalOffset >= _state.DrawSizeWidth)
                    {
                        _state.HorizontalBoardOffset.Add(1);
                        _state.HorizontalOffset = 0;
                        _state.Horizontal = Direction.None;
                    }
                }
                else if (_state.Horizontal == Direction.Right)
                {
                    _state.HorizontalOffset -= FrameRateDirector.MovementFactor * _state.MovementSpeed;

                    if (_state.HorizontalOffset <= -_state.DrawSizeWidth)
                    {
                        _state.HorizontalBoardOffset.Sub(1);
                        _state.HorizontalOffset = 0;
                        _state.Horizontal = Direction.None;
                    }
                }

                if (_state.HorizontalOffset == 0 && _state.VerticalOffset == 0 && InputComponent.Players[0].PressedAction)
                {
                    _state.WasChosen = true;
                }

            }

        }


        public void RenderState(SpriteBatch batch)
        {
            _state.DrawingBoardIndex.Value = _state.VerticalBoardOffset.Value;
            for (int x = _state.HorizontalBoardOffset.Value; x < 8; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    Vector2 Pos = new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _state.DrawSizeWidth / 2 + x * _state.DrawSizeWidth + _state.HorizontalOffset - _state.DrawSizeWidth * 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _state.DrawSizeHeight / 2 + y * _state.DrawSizeHeight + _state.VerticalOffset - _state.DrawSizeHeight * 2);
                    if (!(Pos.X < -_state.DrawSizeWidth || Pos.X > GameGlobals.DRAWING_GAME_WIDTH || Pos.Y < -_state.DrawSizeHeight || Pos.Y > GameGlobals.DRAWING_GAME_HEIGHT))
                    {
                        if (_state.DrawingBoardIndex.Value < _state.BoardTextures.Count && _state.BoardTextures[_state.DrawingBoardIndex.Value] != null)
                        {
                            batch.Draw(_state.BoardTextures[_state.DrawingBoardIndex.Value], new Rectangle((int)Pos.X, (int)Pos.Y, _state.DrawSizeWidth, _state.DrawSizeHeight), Color.White);
                        }
                        else
                        {
                            batch.Draw(_resources.GetTexture("NowLoading").Texture, Pos, Color.White);
                        }
                    }
                    if (Pos == new Vector2(_state.CenteredRectangle.X, _state.CenteredRectangle.Y))
                    {
                        _state.Save = _state.DrawingBoardIndex.Value;
                    }
                    _state.DrawingBoardIndex.Add(1);
                }
            }
            batch.Draw(_resources.GetTexture("MenuTop").Texture, Vector2.Zero, Color.White);
            if (!_state.IsStillLoadingBoards)
            {
                _state.CenteredRectangle = CenterRectangle(new Rectangle(0, 0, (int)(_state.Scale * _state.DrawSizeWidth), (int)(_state.Scale * _state.DrawSizeHeight)), new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2));
                if (_state.WasChosen)
                {
                    batch.Draw(_state.BoardTextures[_state.Save], _state.CenteredRectangle, Color.White);
                }
                batch.Draw(_resources.GetTexture("BoardSelect").Texture, _state.CenteredRectangle, new Color((byte)255, (byte)255, (byte)255, (byte)_state.Alpha));
                RenderGraph.Instance.RenderText(DialogStrings.CleanMapName(_state.ValidBoards[_state.Save]), new Vector2(27, 225), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopLeft, true);
            }
        }

        public void Close()
        {
            _state.BoardTextures = null;
            _state.ValidBoards = null;
            GC.Collect();
        }


        private void ContinueLoadingBoards()
        {
            try
            {
                Texture2D temp = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + _state.BoardNames[_state.CurrentBoardLoading]);

                _state.HasFoundBoard = true;
                _state.IsValidBoard = _state.BoardNames[_state.CurrentBoardLoading];
                _state.BoardTextures.Add(temp);
                _state.ValidBoards.Add(_state.BoardNames[_state.CurrentBoardLoading]);

            }
            catch (InvalidOperationException)
            {
                // Found a .png that is either corrupt or not really a .png, lets just skip it!
            }


            _state.CurrentBoardLoading++;
            if (_state.CurrentBoardLoading == _state.BoardNames.Count)
            {
                FinishLoadingBoards();
            }

        }

        private void FinishLoadingBoards()
        {
            _state.IsStillLoadingBoards = false;
            _state.DrawingBoardIndex = new IntRange(0, 0, _state.BoardTextures.Count - 1);
            _state.VerticalBoardOffset = new IntRange(0, 0, _state.BoardTextures.Count - 1);
            _state.HorizontalBoardOffset = new IntRange(-_state.BoardTextures.Count, -_state.BoardTextures.Count, -1);
        }

        private Rectangle CenterRectangle(Rectangle rect, Vector2 pos)
        {
            return new Rectangle((int)pos.X - rect.Width / 2, (int)pos.Y - rect.Height / 2, rect.Width, rect.Height);
        }
    }
}
