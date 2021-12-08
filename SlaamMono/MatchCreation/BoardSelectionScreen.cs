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
using System.Collections.Generic;
using System.IO;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreen : IScreen
    {
        public LobbyScreen ParentScreen;
        public bool FoundBoard = false;
        public string ValidBoard;
        private int _drawSizeWidth = 75;
        private int _drawSizeHeight = 75;

        private List<Texture2D> _boardTextures = new List<Texture2D>();
        private List<string> _validBoards = new List<string>();
        private IntRange _drawingBoardIndex = new IntRange(0, 0, 0);
        private float _movementSpeed = 0.50f;
        private bool _alphaUp = false;
        private float _alpha = 255f;
        private Direction _vertical = Direction.None;
        private Direction _horizontal = Direction.None;
        private float _verticalOffset = 0f;
        private float _horizontalOffset = 0f;
        private bool _isStillLoadingBoards = true;
        private string[] _boardNames;
        private int _currentBoardLoading = 0;
        private int _save;
        private IntRange _verticalBoardOffset;
        private IntRange _horizontalBoardOffset;
        private bool _wasChosen = false;
        private float _scale = 1.00f;

        private readonly IScreenManager _screenManager;
        private readonly IResources _resources;

        public BoardSelectionScreen(IResources resources, IScreenManager screenManager)
        {
            _resources = resources;
            _screenManager = screenManager;
        }

        public void Initialize(BoardSelectionScreenRequest request)
        {
            ParentScreen = request.ParentScreen;
        }

        public void Open()
        {
            LoadAllBoards();
            FeedManager.InitializeFeeds(DialogStrings.BoardSelectScreenFeed);
            BackgroundManager.ChangeBG(BackgroundType.Normal);
        }

        public void Update()
        {
            if (_isStillLoadingBoards)
            {
                ContinueLoadingBoards();
            }
            else
            {
                _alpha += (_alphaUp ? 1 : -1) * FrameRateDirector.MovementFactor * _movementSpeed;

                if (_alphaUp && _alpha >= 255f)
                {
                    _alphaUp = !_alphaUp;
                    _alpha = 255f;
                }
                else if (!_alphaUp && _alpha <= 0f)
                {
                    _alphaUp = !_alphaUp;
                    _alpha = 0f;
                }

                if (_wasChosen)
                {
                    _scale += FrameRateDirector.MovementFactor * .01f;

                    if (_scale >= 1.50f)
                    {
                        _scale = 1.50f;
                    }

                    if (InputComponent.Players[0].PressedAction)
                    {
                        ParentScreen.LoadBoard(_validBoards[_save]);
                        _screenManager.ChangeTo(ParentScreen);
                    }

                    if (InputComponent.Players[0].PressedAction2)
                    {
                        //SizeIncrease = 1.00f;
                        _wasChosen = false;
                    }
                }
                else
                {
                    _scale -= FrameRateDirector.MovementFactor * .01f;

                    if (_scale <= 1.00f)
                    {
                        _scale = 1.00f;
                    }

                    if (InputComponent.Players[0].PressingDown)
                    {
                        _vertical = Direction.Down;
                    }
                    if (InputComponent.Players[0].PressingUp)
                    {
                        _vertical = Direction.Up;
                    }

                    if (InputComponent.Players[0].PressingRight)
                    {
                        _horizontal = Direction.Right;
                    }
                    if (InputComponent.Players[0].PressingLeft)
                    {
                        _horizontal = Direction.Left;
                    }

                    if (_vertical == Direction.Down)
                    {
                        _verticalOffset -= FrameRateDirector.MovementFactor * _movementSpeed;

                        if (_verticalOffset <= -_drawSizeHeight)
                        {
                            _verticalBoardOffset.Add(1);
                            _verticalOffset = 0;
                            _vertical = Direction.None;
                        }
                    }
                    else if (_vertical == Direction.Up)
                    {
                        _verticalOffset += FrameRateDirector.MovementFactor * _movementSpeed;

                        if (_verticalOffset >= _drawSizeHeight)
                        {
                            _verticalBoardOffset.Sub(1);
                            _verticalOffset = 0;
                            _vertical = Direction.None;
                        }
                    }

                    if (_horizontal == Direction.Left)
                    {
                        _horizontalOffset += FrameRateDirector.MovementFactor * _movementSpeed;

                        if (_horizontalOffset >= _drawSizeWidth)
                        {
                            _horizontalBoardOffset.Add(1);
                            _horizontalOffset = 0;
                            _horizontal = Direction.None;
                        }
                    }
                    else if (_horizontal == Direction.Right)
                    {
                        _horizontalOffset -= FrameRateDirector.MovementFactor * _movementSpeed;

                        if (_horizontalOffset <= -_drawSizeWidth)
                        {
                            _horizontalBoardOffset.Sub(1);
                            _horizontalOffset = 0;
                            _horizontal = Direction.None;
                        }
                    }

                    if (_horizontalOffset == 0 && _verticalOffset == 0 && InputComponent.Players[0].PressedAction)
                    {
                        _wasChosen = true;
                    }

                }
            }
        }

        public Rectangle CenteredRectangle;

        public void Draw(SpriteBatch batch)
        {
            _drawingBoardIndex.Value = _verticalBoardOffset.Value;
            for (int x = _horizontalBoardOffset.Value; x < 8; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    Vector2 Pos = new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _drawSizeWidth / 2 + x * _drawSizeWidth + _horizontalOffset - _drawSizeWidth * 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _drawSizeHeight / 2 + y * _drawSizeHeight + _verticalOffset - _drawSizeHeight * 2);
                    if (!(Pos.X < -_drawSizeWidth || Pos.X > GameGlobals.DRAWING_GAME_WIDTH || Pos.Y < -_drawSizeHeight || Pos.Y > GameGlobals.DRAWING_GAME_HEIGHT))
                    {
                        if (_drawingBoardIndex.Value < _boardTextures.Count && _boardTextures[_drawingBoardIndex.Value] != null)
                        {
                            batch.Draw(_boardTextures[_drawingBoardIndex.Value], new Rectangle((int)Pos.X, (int)Pos.Y, _drawSizeWidth, _drawSizeHeight), Color.White);
                        }
                        else
                        {
                            batch.Draw(_resources.GetTexture("NowLoading").Texture, Pos, Color.White);
                        }
                    }
                    if (Pos == new Vector2(CenteredRectangle.X, CenteredRectangle.Y))
                    {
                        _save = _drawingBoardIndex.Value;
                    }
                    _drawingBoardIndex.Add(1);
                }
            }
            batch.Draw(_resources.GetTexture("MenuTop").Texture, Vector2.Zero, Color.White);
            if (!_isStillLoadingBoards)
            {
                CenteredRectangle = CenterRectangle(new Rectangle(0, 0, (int)(_scale * _drawSizeWidth), (int)(_scale * _drawSizeHeight)), new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2));
                if (_wasChosen)
                {
                    batch.Draw(_boardTextures[_save], CenteredRectangle, Color.White);
                }
                batch.Draw(_resources.GetTexture("BoardSelect").Texture, CenteredRectangle, new Color((byte)255, (byte)255, (byte)255, (byte)_alpha));
#if !ZUNE
                batch.Draw(Resources.BoardSelectTextUnderlay.Texture, new Vector2(0, 175), new Color(255, 255, 255, 100));
#endif
                RenderGraph.Instance.RenderText(DialogStrings.CleanMapName(_validBoards[_save]), new Vector2(27, 225), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopLeft, true);
            }
        }

        public void Close()
        {
            /*for (int x = 0; x < Boards.Count; x++)
            {
                if(x != save)
                    Boards[x].Dispose();
            }*/
            _boardTextures = null;
            _validBoards = null;
            GC.Collect();
        }

        public void LoadAllBoards()
        {
            //boards = Directory.GetFiles("boards");
            _boardNames = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Content\\BoardList.txt");// Game1.Content.Load<List<String>>(Directory.GetCurrentDirectory() + "\\content\\BoardList").ToArray();
            List<string> strs = new List<string>();
            for (int x = 0; x < _boardNames.Length; x++)
            {
                //if (boards[x].EndsWith(".png"))
                strs.Add(_boardNames[x]);
            }
            _boardNames = new string[strs.Count];
            for (int x = 0; x < strs.Count; x++)
            {
                _boardNames[x] = strs[x];
            }
            _drawingBoardIndex = new IntRange(0, 0, _boardNames.Length - 1);
            _verticalBoardOffset = new IntRange(0);
            _horizontalBoardOffset = new IntRange(-_boardNames.Length);
        }

        private void ContinueLoadingBoards()
        {
            //if (boards[CurrentBoardLoading].EndsWith(".png"))
            {
                try
                {
                    Texture2D temp = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + _boardNames[_currentBoardLoading]);//Texture2D.FromFile(Game1.Graphics.GraphicsDevice, boards[CurrentBoardLoading]);
                    //if ((temp.Width == 800 && temp.Height == 800))
                    {
                        FoundBoard = true;
                        ValidBoard = _boardNames[_currentBoardLoading];
                        _boardTextures.Add(temp);
                        _validBoards.Add(_boardNames[_currentBoardLoading]);
                    }
                }
                catch (InvalidOperationException)
                {
                    // Found a .png that is either corrupt or not really a .png, lets just skip it!
                }
            }

            _currentBoardLoading++;

            if (_currentBoardLoading == _boardNames.Length)
            {
                FinishLoadingBoards();
            }

        }

        private void FinishLoadingBoards()
        {
            _isStillLoadingBoards = false;
            _drawingBoardIndex = new IntRange(0, 0, _boardTextures.Count - 1);
            _verticalBoardOffset = new IntRange(0, 0, _boardTextures.Count - 1);
            _horizontalBoardOffset = new IntRange(-_boardTextures.Count, -_boardTextures.Count, -1);
        }

        private Rectangle CenterRectangle(Rectangle rect, Vector2 pos)
        {
            return new Rectangle((int)pos.X - rect.Width / 2, (int)pos.Y - rect.Height / 2, rect.Width, rect.Height);
        }
    }
}
