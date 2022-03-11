using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.Input
{
    public static class Qwerty
    {
        private const float MovementSpeed = (20f / 10f);

        public static bool Active = false;

        public static InputDevice CurrentPlayer;
        private static QwertyStatus Status = QwertyStatus.GoingUp;
        private static Vector2 SelectedPosition = new Vector2(0, 0);
        private static Vector2 BoardPosition = new Vector2(500, 300);
        private static Vector2 TargetPosition = new Vector2(500, 300);
        private static QwertyKey[,] Keys = new QwertyKey[10, 4];

        public static bool CapslockToggled = false;
        public static bool ShiftToggled = false;

        public static string EditingString = "";

        private static IRenderGraph _renderGraphManager;
        private static IResources _resources;

        private static readonly Rectangle _boxRectangle = new Rectangle(0, 0, 1280, 1024);
        private static readonly Color _boxColor = new Color(0, 0, 0, 200);

        static Qwerty()
        {
            x_init(
                x_Di.Get<IRenderGraph>(),
                x_Di.Get<IResources>());

            InitKeys();
        }

        private static void x_init(IRenderGraph renderGraphManager, IResources resources)
        {
            _renderGraphManager = renderGraphManager;
            _resources = resources;
        }

        public static void InitKeys()
        {
            Keys[0, 0] = new QwertyKey("q", QwertyKeyType.Normal);
            Keys[1, 0] = new QwertyKey("w", QwertyKeyType.Normal);
            Keys[2, 0] = new QwertyKey("e", QwertyKeyType.Normal);
            Keys[3, 0] = new QwertyKey("r", QwertyKeyType.Normal);
            Keys[4, 0] = new QwertyKey("t", QwertyKeyType.Normal);
            Keys[5, 0] = new QwertyKey("y", QwertyKeyType.Normal);
            Keys[6, 0] = new QwertyKey("u", QwertyKeyType.Normal);
            Keys[7, 0] = new QwertyKey("i", QwertyKeyType.Normal);
            Keys[8, 0] = new QwertyKey("o", QwertyKeyType.Normal);
            Keys[9, 0] = new QwertyKey("p", QwertyKeyType.Normal);

            Keys[0, 1] = new QwertyKey("a", QwertyKeyType.Normal);
            Keys[1, 1] = new QwertyKey("s", QwertyKeyType.Normal);
            Keys[2, 1] = new QwertyKey("d", QwertyKeyType.Normal);
            Keys[3, 1] = new QwertyKey("f", QwertyKeyType.Normal);
            Keys[4, 1] = new QwertyKey("g", QwertyKeyType.Normal);
            Keys[5, 1] = new QwertyKey("h", QwertyKeyType.Normal);
            Keys[6, 1] = new QwertyKey("j", QwertyKeyType.Normal);
            Keys[7, 1] = new QwertyKey("k", QwertyKeyType.Normal);
            Keys[8, 1] = new QwertyKey("l", QwertyKeyType.Normal);
            Keys[9, 1] = new QwertyKey("Caps", QwertyKeyType.Caps);

            Keys[0, 2] = new QwertyKey("z", QwertyKeyType.Normal);
            Keys[1, 2] = new QwertyKey("x", QwertyKeyType.Normal);
            Keys[2, 2] = new QwertyKey("c", QwertyKeyType.Normal);
            Keys[3, 2] = new QwertyKey("v", QwertyKeyType.Normal);
            Keys[4, 2] = new QwertyKey("b", QwertyKeyType.Normal);
            Keys[5, 2] = new QwertyKey("n", QwertyKeyType.Normal);
            Keys[6, 2] = new QwertyKey("m", QwertyKeyType.Normal);
            Keys[7, 2] = new QwertyKey(".", QwertyKeyType.Normal);
            Keys[8, 2] = new QwertyKey("#", QwertyKeyType.Nums);
            Keys[9, 2] = new QwertyKey("Shift", QwertyKeyType.Shift);

            Keys[0, 3] = new QwertyKey("Space", QwertyKeyType.Space);
            Keys[1, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[2, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[3, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[4, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[5, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[6, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[7, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[8, 3] = new QwertyKey("", QwertyKeyType.Blank);
            Keys[9, 3] = new QwertyKey("Done", QwertyKeyType.Done);
        }

        public static void Update()
        {
            if (Status == QwertyStatus.GoingUp)
            {
                BoardPosition.Y -= FrameRateDirector.Instance.GetLatestFrame().MovementFactor * MovementSpeed;

                if (BoardPosition.Y <= TargetPosition.Y)
                    Status = QwertyStatus.Normal;
            }
            else if (Status == QwertyStatus.GoingDown)
            {
                BoardPosition.Y += FrameRateDirector.Instance.GetLatestFrame().MovementFactor * MovementSpeed;

                if (BoardPosition.Y >= 1024)
                    Active = false;
            }
            else
            {
                CheckKeys();

                if (Keys[(int)SelectedPosition.X, (int)SelectedPosition.Y].Type == QwertyKeyType.Blank)
                {
                    SelectedPosition.X = 0;
                }

                if (CurrentPlayer.PressedAction)
                {
                    if (IsKeyString("Space"))
                    {
                        EditingString += " ";
                    }
                    else if (IsKeyString("Shift"))
                    {
                        if (!ShiftToggled)
                        {
                            ShiftToggled = true;
                            CapslockToggled = false;
                            ToggleCaps(true);

                        }
                        else
                        {
                            ShiftToggled = false;
                            CapslockToggled = false;
                            ToggleCaps(false);
                        }
                    }
                    else if (IsKeyString("Caps"))
                    {
                        if (!CapslockToggled)
                        {
                            CapslockToggled = true;
                            ShiftToggled = false;
                            ToggleCaps(true);
                        }
                        else
                        {
                            CapslockToggled = false;
                            ShiftToggled = false;
                            ToggleCaps(false);
                        }
                    }
                    else if (IsKeyString("Done"))
                    {
                        Status = QwertyStatus.GoingDown;
                    }
                    else
                    {
                        EditingString += Keys[(int)SelectedPosition.X, (int)SelectedPosition.Y].Chars;
                        if (ShiftToggled && !CapslockToggled)
                        {
                            ToggleCaps(false);
                            ShiftToggled = false;
                        }
                    }
                }

                if (CurrentPlayer.PressedAction2 && EditingString.Length > 0)
                    EditingString = EditingString.Substring(0, EditingString.Length - 1);
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            _renderGraphManager.RenderBox(_boxRectangle, _boxColor);
            batch.Draw(_resources.GetTexture("KeyboardBG").Texture, new Vector2(BoardPosition.X - 10, BoardPosition.Y - 10), Color.White);
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (Keys[x, y].Type != QwertyKeyType.Space && Keys[x, y].Type != QwertyKeyType.Blank)
                        batch.Draw(_resources.GetTexture("Key").Texture, new Vector2(BoardPosition.X + x * 54, BoardPosition.Y + y * 54), Color.White);
                    else if (Keys[x, y].Type == QwertyKeyType.Space)
                        batch.Draw(_resources.GetTexture("Spacebar").Texture, new Vector2(BoardPosition.X + x * 54, BoardPosition.Y + y * 54), Color.White);
                }
            }

            batch.Draw(_resources.GetTexture("Textbox").Texture, new Vector2(BoardPosition.X + 91.5f, BoardPosition.Y - 60f), Color.White);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (Keys[x, y].Type == QwertyKeyType.Normal)
                        RenderGraph.Instance.RenderText(Keys[x, y].Chars, new Vector2(BoardPosition.X + 27 + (x * 54), BoardPosition.Y + 35 + (y * 54)), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopCenter, true);
                    else
                        RenderGraph.Instance.RenderText(Keys[x, y].Chars, new Vector2(BoardPosition.X + 27 + (x * 54), BoardPosition.Y + 40 + (y * 54)), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, false);
                }
            }
            RenderGraph.Instance.RenderText(EditingString, new Vector2(BoardPosition.X + 131.5f, BoardPosition.Y - 32f), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);

            if (Keys[(int)SelectedPosition.X, (int)SelectedPosition.Y].Type != QwertyKeyType.Space)
                batch.Draw(_resources.GetTexture("KeyHT").Texture, new Vector2(BoardPosition.X + SelectedPosition.X * 54, BoardPosition.Y + SelectedPosition.Y * 54), Color.White);
            else
                batch.Draw(_resources.GetTexture("SpaceHT").Texture, new Vector2(BoardPosition.X + SelectedPosition.X * 54, BoardPosition.Y + SelectedPosition.Y * 54), Color.White);
        }

        /// <summary>
        /// Displays the board onscreen in cool motion
        /// </summary>
        /// <param name="str">String to start out with.</param>
        /// <param name="position">Where to draw the board</param>
        public static void DisplayBoard(string str)
        {
            EditingString = str;
            BoardPosition = new Vector2(640 - _resources.GetTexture("KeyboardBG").Width / 2, 1024);
            TargetPosition = new Vector2(640 - _resources.GetTexture("KeyboardBG").Width / 2, 760);
            Active = true;
            Status = QwertyStatus.GoingUp;
        }

        /// <summary>
        /// Tells whether the selected key is equal to the string.
        /// </summary>
        /// <param name="str">Value to test</param>
        /// <returns></returns>
        public static bool IsKeyString(string str)
        {
            return Keys[(int)SelectedPosition.X, (int)SelectedPosition.Y].Chars.ToLower() == str.ToLower();
        }

        /// <summary>
        /// Toggles caps lock on all keys
        /// </summary>
        /// <param name="upper">Uppercase?</param>
        public static void ToggleCaps(bool upper)
        {
            string keystrL, keystrU;

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    keystrL = Keys[x, y].Chars.ToLower();
                    keystrU = keystrL.ToUpper();

                    if (keystrL != "space" && keystrL != "done" && keystrL != "shift" && keystrL != "caps")
                    {
                        if (upper)
                        {
                            Keys[x, y].Chars = keystrU;
                        }
                        else
                        {
                            Keys[x, y].Chars = keystrL;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Detects movements and moves to the appropriate keys
        /// </summary>
        public static void CheckKeys()
        {
            if (CurrentPlayer.PressedRight)
            {
                if (SelectedPosition.X == 0 && SelectedPosition.Y == 3)
                    SelectedPosition.X = 9;
                else
                    SelectedPosition.X++;
            }

            if (CurrentPlayer.PressedLeft)
                SelectedPosition.X--;

            if (CurrentPlayer.PressedUp)
                SelectedPosition.Y--;

            if (CurrentPlayer.PressedDown)
                SelectedPosition.Y++;

            if (SelectedPosition.X > 9)
                SelectedPosition.X = 0;
            if (SelectedPosition.X < 0)
                SelectedPosition.X = 9;

            if (SelectedPosition.Y > 3)
                SelectedPosition.Y = 0;
            if (SelectedPosition.Y < 0)
                SelectedPosition.Y = 3;
        }
    }
}
