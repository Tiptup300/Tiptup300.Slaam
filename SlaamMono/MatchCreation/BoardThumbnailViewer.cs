using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.Rendering.Text;
using SlaamMono.Library.Screens;
using SlaamMono.ResourceManagement;
using SlaamMono.x_;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlaamMono.MatchCreation
{
    public class BoardThumbnailViewer : IScreen
    {
        public LobbyScreen ParentScreen;

        private List<Texture2D> Boards = new List<Texture2D>();
        private List<string> ValidBoards = new List<string>();

        private IntRange DrawingBoardIndex = new IntRange(0, 0, 0);

        private float MovementSpeed = 0.50f;

        private bool AlphaUp = false;
        private float Alpha = 255f;

        private Direction Vertical = Direction.None;
        private Direction Horizontal = Direction.None;

        private float VOffset = 0f;
        private float HOffset = 0f;

        private bool StillLoadingBoards = true;
        string[] boards;
        private int CurrentBoardLoading = 0;

        private int save;

        private IntRange VBoardOffset;
        private IntRange HBoardOffset;

        private bool Chosen = false;
        private float SizeIncrease = 1.00f;

        public bool FoundBoard = false;
        public string ValidBoard;

#if ZUNE
        public int DrawSizeWidth = 75;
        public int DrawSizeHeight = 75;
#else
        public int DrawSizeWidth = ((GameGlobals.TILE_SIZE * GameGlobals.BOARD_WIDTH) / 4);
        public int DrawSizeHeight = ((GameGlobals.TILE_SIZE * GameGlobals.BOARD_HEIGHT) / 4);
#endif
        private readonly IScreenManager _screenDirector;

        public BoardThumbnailViewer(LobbyScreen parentscreen)
        {
            ParentScreen = parentscreen;
            _screenDirector = Di.Get<IScreenManager>();
        }

        public void Open()
        {
            LoadAllBoards();
            FeedManager.InitializeFeeds(DialogStrings.BoardSelectScreenFeed);
            BackgroundManager.ChangeBG(BackgroundType.Normal);
        }

        public void Update()
        {
            if (StillLoadingBoards)
            {
                ContinueLoadingBoards();
            }
            else
            {
                Alpha += (AlphaUp ? 1 : -1) * FrameRateDirector.MovementFactor * MovementSpeed;

                if (AlphaUp && Alpha >= 255f)
                {
                    AlphaUp = !AlphaUp;
                    Alpha = 255f;
                }
                else if (!AlphaUp && Alpha <= 0f)
                {
                    AlphaUp = !AlphaUp;
                    Alpha = 0f;
                }

                if (Chosen)
                {
                    SizeIncrease += FrameRateDirector.MovementFactor * .01f;

                    if (SizeIncrease >= 1.50f)
                    {
                        SizeIncrease = 1.50f;
                    }

                    if (InputComponent.Players[0].PressedAction)
                    {
                        ParentScreen.LoadBoard(ValidBoards[save]);
                        _screenDirector.ChangeTo(ParentScreen);
                    }

                    if (InputComponent.Players[0].PressedAction2)
                    {
                        //SizeIncrease = 1.00f;
                        Chosen = false;
                    }
                }
                else
                {
                    SizeIncrease -= FrameRateDirector.MovementFactor * .01f;

                    if (SizeIncrease <= 1.00f)
                    {
                        SizeIncrease = 1.00f;
                    }

                    if (InputComponent.Players[0].PressingDown)
                    {
                        Vertical = Direction.Down;
                    }
                    if (InputComponent.Players[0].PressingUp)
                    {
                        Vertical = Direction.Up;
                    }

                    if (InputComponent.Players[0].PressingRight)
                    {
                        Horizontal = Direction.Right;
                    }
                    if (InputComponent.Players[0].PressingLeft)
                    {
                        Horizontal = Direction.Left;
                    }

                    if (Vertical == Direction.Down)
                    {
                        VOffset -= FrameRateDirector.MovementFactor * MovementSpeed;

                        if (VOffset <= -DrawSizeHeight)
                        {
                            VBoardOffset.Add(1);
                            VOffset = 0;
                            Vertical = Direction.None;
                        }
                    }
                    else if (Vertical == Direction.Up)
                    {
                        VOffset += FrameRateDirector.MovementFactor * MovementSpeed;

                        if (VOffset >= DrawSizeHeight)
                        {
                            VBoardOffset.Sub(1);
                            VOffset = 0;
                            Vertical = Direction.None;
                        }
                    }

                    if (Horizontal == Direction.Left)
                    {
                        HOffset += FrameRateDirector.MovementFactor * MovementSpeed;

                        if (HOffset >= DrawSizeWidth)
                        {
                            HBoardOffset.Add(1);
                            HOffset = 0;
                            Horizontal = Direction.None;
                        }
                    }
                    else if (Horizontal == Direction.Right)
                    {
                        HOffset -= FrameRateDirector.MovementFactor * MovementSpeed;

                        if (HOffset <= -DrawSizeWidth)
                        {
                            HBoardOffset.Sub(1);
                            HOffset = 0;
                            Horizontal = Direction.None;
                        }
                    }

                    if (HOffset == 0 && VOffset == 0 && InputComponent.Players[0].PressedAction)
                    {
                        Chosen = true;
                    }

                }
            }
        }

        public Rectangle CenteredRectangle;

        public void Draw(SpriteBatch batch)
        {
            DrawingBoardIndex.Value = VBoardOffset.Value;
            for (int x = HBoardOffset.Value; x < 8; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    Vector2 Pos = new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - DrawSizeWidth / 2 + x * DrawSizeWidth + HOffset - DrawSizeWidth * 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - DrawSizeHeight / 2 + y * DrawSizeHeight + VOffset - DrawSizeHeight * 2);
                    if (!(Pos.X < -DrawSizeWidth || Pos.X > GameGlobals.DRAWING_GAME_WIDTH || Pos.Y < -DrawSizeHeight || Pos.Y > GameGlobals.DRAWING_GAME_HEIGHT))
                    {
                        if (DrawingBoardIndex.Value < Boards.Count && Boards[DrawingBoardIndex.Value] != null)
                        {
                            batch.Draw(Boards[DrawingBoardIndex.Value], new Rectangle((int)Pos.X, (int)Pos.Y, DrawSizeWidth, DrawSizeHeight), Color.White);
                        }
                        else
                        {
                            batch.Draw(Resources.Instance.GetTexture("NowLoading").Texture, Pos, Color.White);
                        }
                    }
                    if (Pos == new Vector2(CenteredRectangle.X, CenteredRectangle.Y))
                    {
                        save = DrawingBoardIndex.Value;
                    }
                    DrawingBoardIndex.Add(1);
                }
            }
            batch.Draw(Resources.Instance.GetTexture("MenuTop").Texture, Vector2.Zero, Color.White);
            if (!StillLoadingBoards)
            {
                CenteredRectangle = CenterRectangle(new Rectangle(0, 0, (int)(SizeIncrease * DrawSizeWidth), (int)(SizeIncrease * DrawSizeHeight)), new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2));
                if (Chosen)
                {
                    batch.Draw(Boards[save], CenteredRectangle, Color.White);
                }
                batch.Draw(Resources.Instance.GetTexture("BoardSelect").Texture, CenteredRectangle, new Color((byte)255, (byte)255, (byte)255, (byte)Alpha));
#if !ZUNE
                batch.Draw(Resources.BoardSelectTextUnderlay.Texture, new Vector2(0, 175), new Color(255, 255, 255, 100));
#endif
                RenderGraphManager.Instance.RenderText(DialogStrings.CleanMapName(ValidBoards[save]), new Vector2(27, 225), Resources.Instance.GetFont("SegoeUIx32pt"), Color.White, TextAlignment.Default, true);
            }
        }

        public void Close()
        {
            /*for (int x = 0; x < Boards.Count; x++)
            {
                if(x != save)
                    Boards[x].Dispose();
            }*/
            Boards = null;
            ValidBoards = null;
            GC.Collect();
        }

        public void LoadAllBoards()
        {
            //boards = Directory.GetFiles("boards");
            boards = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Content\\BoardList.txt");// Game1.Content.Load<List<String>>(Directory.GetCurrentDirectory() + "\\content\\BoardList").ToArray();
            List<string> strs = new List<string>();
            for (int x = 0; x < boards.Length; x++)
            {
                //if (boards[x].EndsWith(".png"))
                strs.Add(boards[x]);
            }
            boards = new string[strs.Count];
            for (int x = 0; x < strs.Count; x++)
            {
                boards[x] = strs[x];
            }
            DrawingBoardIndex = new IntRange(0, 0, boards.Length - 1);
            VBoardOffset = new IntRange(0);
            HBoardOffset = new IntRange(-boards.Length);
        }

        private void ContinueLoadingBoards()
        {
            //if (boards[CurrentBoardLoading].EndsWith(".png"))
            {
                try
                {
                    Texture2D temp = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + boards[CurrentBoardLoading]);//Texture2D.FromFile(Game1.Graphics.GraphicsDevice, boards[CurrentBoardLoading]);
                    //if ((temp.Width == 800 && temp.Height == 800))
                    {
                        FoundBoard = true;
                        ValidBoard = boards[CurrentBoardLoading];
                        Boards.Add(temp);
                        ValidBoards.Add(boards[CurrentBoardLoading]);
                    }
                }
                catch (InvalidOperationException)
                {
                    // Found a .png that is either corrupt or not really a .png, lets just skip it!
                }
            }

            CurrentBoardLoading++;

            if (CurrentBoardLoading == boards.Length)
            {
                FinishLoadingBoards();
            }

        }

        private void FinishLoadingBoards()
        {
            StillLoadingBoards = false;
            DrawingBoardIndex = new IntRange(0, 0, Boards.Count - 1);
            VBoardOffset = new IntRange(0, 0, Boards.Count - 1);
            HBoardOffset = new IntRange(-Boards.Count, -Boards.Count, -1);
        }

        private Rectangle CenterRectangle(Rectangle rect, Vector2 pos)
        {
            return new Rectangle((int)pos.X - rect.Width / 2, (int)pos.Y - rect.Height / 2, rect.Width, rect.Height);
        }
    }
}
