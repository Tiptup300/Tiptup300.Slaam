using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Input;
using SlaamMono.Resources;

namespace SlaamMono
{
    public static class Qwerty
    {
        #region Variables

        private const float MovementSpeed = (20f / 10f);

        public static bool Active = false;

        public static InputDevice CurrentPlayer;
        private static QwertyStatus Status = QwertyStatus.GoingUp;
        private static Vector2 SelectedPosition = new Vector2(0, 0);
        private static Vector2 BoardPosition = new Vector2(500, 300);
        private static Vector2 TargetPosition = new Vector2(500, 300);
        private static Key[,] Keys = new Key[10, 4];

        public static bool CapslockToggled = false;
        public static bool ShiftToggled = false;

        public static string EditingString = "";

        #endregion

        #region Constructor

        static Qwerty()
        {
            InitKeys();
        }


        public static void InitKeys()
        {
            Keys[0, 0] = new Key("q", KeyType.Normal);
            Keys[1, 0] = new Key("w", KeyType.Normal);
            Keys[2, 0] = new Key("e", KeyType.Normal);
            Keys[3, 0] = new Key("r", KeyType.Normal);
            Keys[4, 0] = new Key("t", KeyType.Normal);
            Keys[5, 0] = new Key("y", KeyType.Normal);
            Keys[6, 0] = new Key("u", KeyType.Normal);
            Keys[7, 0] = new Key("i", KeyType.Normal);
            Keys[8, 0] = new Key("o", KeyType.Normal);
            Keys[9, 0] = new Key("p", KeyType.Normal);

            Keys[0, 1] = new Key("a", KeyType.Normal);
            Keys[1, 1] = new Key("s", KeyType.Normal);
            Keys[2, 1] = new Key("d", KeyType.Normal);
            Keys[3, 1] = new Key("f", KeyType.Normal);
            Keys[4, 1] = new Key("g", KeyType.Normal);
            Keys[5, 1] = new Key("h", KeyType.Normal);
            Keys[6, 1] = new Key("j", KeyType.Normal);
            Keys[7, 1] = new Key("k", KeyType.Normal);
            Keys[8, 1] = new Key("l", KeyType.Normal);
            Keys[9, 1] = new Key("Caps", KeyType.Caps);

            Keys[0, 2] = new Key("z", KeyType.Normal);
            Keys[1, 2] = new Key("x", KeyType.Normal);
            Keys[2, 2] = new Key("c", KeyType.Normal);
            Keys[3, 2] = new Key("v", KeyType.Normal);
            Keys[4, 2] = new Key("b", KeyType.Normal);
            Keys[5, 2] = new Key("n", KeyType.Normal);
            Keys[6, 2] = new Key("m", KeyType.Normal);
            Keys[7, 2] = new Key(".", KeyType.Normal);
            Keys[8, 2] = new Key("#", KeyType.Nums);
            Keys[9, 2] = new Key("Shift", KeyType.Shift);

            Keys[0, 3] = new Key("Space", KeyType.Space);
            Keys[1, 3] = new Key("", KeyType.Blank);
            Keys[2, 3] = new Key("", KeyType.Blank);
            Keys[3, 3] = new Key("", KeyType.Blank);
            Keys[4, 3] = new Key("", KeyType.Blank);
            Keys[5, 3] = new Key("", KeyType.Blank);
            Keys[6, 3] = new Key("", KeyType.Blank);
            Keys[7, 3] = new Key("", KeyType.Blank);
            Keys[8, 3] = new Key("", KeyType.Blank);
            Keys[9, 3] = new Key("Done", KeyType.Done);
        }
        #endregion

        #region Update

        public static void Update()
        {
            if (Status == QwertyStatus.GoingUp)
            {
                BoardPosition.Y -= FrameRateDirector.MovementFactor * MovementSpeed;

                if (BoardPosition.Y <= TargetPosition.Y)
                    Status = QwertyStatus.Normal;
            }
            else if (Status == QwertyStatus.GoingDown)
            {
                BoardPosition.Y += FrameRateDirector.MovementFactor * MovementSpeed;

                if (BoardPosition.Y >= 1024)
                    Active = false;
            }
            else
            {
                CheckKeys();

                if (Keys[(int)SelectedPosition.X, (int)SelectedPosition.Y].Type == KeyType.Blank)
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

        #endregion

        #region Draw

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.Dot, new Rectangle(0, 0, 1280, 1024), new Color(0, 0, 0, 200));
            batch.Draw(ResourceManager.KeyboardBG.Texture, new Vector2(BoardPosition.X - 10, BoardPosition.Y - 10), Color.White);
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (Keys[x, y].Type != KeyType.Space && Keys[x, y].Type != KeyType.Blank)
                        batch.Draw(ResourceManager.Key.Texture, new Vector2(BoardPosition.X + x * 54, BoardPosition.Y + y * 54), Color.White);
                    else if (Keys[x, y].Type == KeyType.Space)
                        batch.Draw(ResourceManager.Spacebar.Texture, new Vector2(BoardPosition.X + x * 54, BoardPosition.Y + y * 54), Color.White);
                }
            }

            batch.Draw(ResourceManager.Textbox.Texture, new Vector2(BoardPosition.X + 91.5f, BoardPosition.Y - 60f), Color.White);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (Keys[x, y].Type == KeyType.Normal)
                        ResourceManager.DrawText(Keys[x, y].Chars, new Vector2(BoardPosition.X + 27 + (x * 54), BoardPosition.Y + 35 + (y * 54)), ResourceManager.SegoeUIx32pt, Color.White, TextAlignment.Centered, true);
                    else
                        ResourceManager.DrawText(Keys[x, y].Chars, new Vector2(BoardPosition.X + 27 + (x * 54), BoardPosition.Y + 40 + (y * 54)), ResourceManager.SegoeUIx14pt, Color.White, TextAlignment.Centered, false);
                }
            }
            ResourceManager.DrawText(EditingString, new Vector2(BoardPosition.X + 131.5f, BoardPosition.Y - 32f), ResourceManager.SegoeUIx14pt, Color.Black, TextAlignment.Default, false);

            if (Keys[(int)SelectedPosition.X, (int)SelectedPosition.Y].Type != KeyType.Space)
                batch.Draw(ResourceManager.KeyHT.Texture, new Vector2(BoardPosition.X + SelectedPosition.X * 54, BoardPosition.Y + SelectedPosition.Y * 54), Color.White);
            else
                batch.Draw(ResourceManager.SpaceHT.Texture, new Vector2(BoardPosition.X + SelectedPosition.X * 54, BoardPosition.Y + SelectedPosition.Y * 54), Color.White);
        }

        #endregion

        #region Extra Methods

        /// <summary>
        /// Displays the board onscreen in cool motion
        /// </summary>
        /// <param name="str">String to start out with.</param>
        /// <param name="position">Where to draw the board</param>
        public static void DisplayBoard(string str)
        {
            EditingString = str;
            BoardPosition = new Vector2(640 - ResourceManager.KeyboardBG.Width / 2, 1024);
            TargetPosition = new Vector2(640 - ResourceManager.KeyboardBG.Width / 2, 760);
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

        #endregion

        #region Structs & Enums

        private struct Key
        {
            public bool Selected;
            public string Chars;
            public KeyType Type;

            public Key(string chars, KeyType type)
            {
                Selected = false;
                Chars = chars;
                Type = type;
            }
        }

        private enum KeyType
        {
            Normal,
            Nums,
            Caps,
            Shift,
            Done,
            Space,
            Blank,
        }

        private enum QwertyStatus
        {
            GoingUp,
            Normal,
            GoingDown,
        }

        #endregion
    }
}
