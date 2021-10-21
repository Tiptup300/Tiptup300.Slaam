using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    public partial class ZuneBlade
    {
        internal Texture2D Key1;
        internal Texture2D Key2;
        internal Texture2D Key6;
        internal Texture2D TextBox;

        internal Key[] Keys;
        internal const int KeyboardRowSize = 10;
        internal const int KeyboardColSize = 5;

        internal BladeStatus tempStatus;
        internal string CurrentKeyboardString;
        internal int CurrentKeyboardIndex;

        internal bool CapsMode;
        internal bool ShiftMode;

        public static int MinimumSize = 0;
        public static int MaximumSize = 24;
        public static bool PhoneNo = false;

        internal int KeyboardXPosition
        {
            get { return keyboardXPosition; }
            set
            {
                keyboardXPosition = value;

                if (keyboardXPosition < 0)
                    keyboardXPosition = KeyboardRowSize - 1;

                if (keyboardXPosition >= KeyboardRowSize)
                    keyboardXPosition -= KeyboardRowSize;
            }
        }
        internal int keyboardXPosition;
        internal int KeyboardYPosition;

        internal Key CurrentKey
        {
            get
            {
                int x = 0;
                int y = 0;
                for (int i = 0; i < Keys.Length; i++)
                {
                    if (KeyboardXPosition >= x && keyboardXPosition < x + Keys[i].Size &&
                        y == KeyboardYPosition)
                        return Keys[i];

                    x += Keys[i].Size;

                    if (x >= KeyboardRowSize)
                    {
                        y++;
                        x -= KeyboardRowSize;
                    }
                }

                return null;
            }
        }

        internal Vector2 CurrentKeyPos
        {
            get
            {
                int x = 0;
                int y = 0;
                for (int i = 0; i < Keys.Length; i++)
                {
                    if (KeyboardXPosition >= x && keyboardXPosition < x + Keys[i].Size &&
                        y == KeyboardYPosition)
                        return new Vector2(x, y);

                    x += Keys[i].Size;

                    if (x >= KeyboardRowSize)
                    {
                        y++;
                        x -= KeyboardRowSize;
                    }
                }

                return Vector2.Zero;
            }
        }

        internal void SetupKeys()
        {
            Keys = new Key[43];

            Keys[0] = new Key("1");
            Keys[1] = new Key("2");
            Keys[2] = new Key("3");
            Keys[3] = new Key("4");
            Keys[4] = new Key("5");
            Keys[5] = new Key("6");
            Keys[6] = new Key("7");
            Keys[7] = new Key("8");
            Keys[8] = new Key("9");
            Keys[9] = new Key("0");

            Keys[10] = new Key("q");
            Keys[11] = new Key("w");
            Keys[12] = new Key("e");
            Keys[13] = new Key("r");
            Keys[14] = new Key("t");
            Keys[15] = new Key("y");
            Keys[16] = new Key("u");
            Keys[17] = new Key("i");
            Keys[18] = new Key("o");
            Keys[19] = new Key("p");

            Keys[20] = new Key("a");
            Keys[21] = new Key("s");
            Keys[22] = new Key("d");
            Keys[23] = new Key("f");
            Keys[24] = new Key("g");
            Keys[25] = new Key("h");
            Keys[26] = new Key("j");
            Keys[27] = new Key("k");
            Keys[28] = new Key("l");
            Keys[29] = new Key(";");

            Keys[30] = new Key("z");
            Keys[31] = new Key("x");
            Keys[32] = new Key("c");
            Keys[33] = new Key("v");
            Keys[34] = new Key("b");
            Keys[35] = new Key("n");
            Keys[36] = new Key("m");
            Keys[37] = new Key("!");
            Keys[38] = new Key("");
            Keys[39] = new Key(Key.KeyType.Left);

            Keys[40] = new Key(Key.KeyType.Shift);
            Keys[41] = new Key(Key.KeyType.Space);
            Keys[42] = new Key(Key.KeyType.Submit);

        }

        internal EventHandler OnFinished;

        public static void BeginShowKeyboardInput(string title, string defaultText, EventHandler onFinished)
        {
            if (instance.Keys == null)
                instance.SetupKeys();
            instance.CurrentKeyboardString = defaultText;
            instance.tempStatus = instance.status;
            instance.Status = BladeStatus.KeyOut;
            instance.CurrentKeyboardIndex = instance.CurrentKeyboardString.Length;
            InfoBlade.BladeKeyOutSetup.MiddleButtonText = title;
            instance.OnFinished = onFinished;

        }

        public static string EndShowKeyboardInput()
        {
            string temp = instance.CurrentKeyboardString;
            instance.CurrentKeyboardString = "";
            return temp;
        }

        bool FirstUpdate = true;

        internal void UpdateKeyboard(GameTime gameTime)
        {
            if (FirstUpdate)
            {
                FirstUpdate = false;
                return;
            }
            if (input.IsPressed(ZuneButtons.PlayPause))
            {
                // Keys = null;
                // status = tempStatus;
                // OnFinished(CurrentKeyboardString, null);
            }
            else if (input.IsPressed(ZuneButtons.Back))
            {
                Keys = null;
                status = tempStatus;
                OnFinished(null, null);
            }
            else if (input.IsPressed(ZuneButtons.PadCenter) && CurrentKey != null)
            {
                if (CurrentKey.Type == Key.KeyType.Normal && CurrentKeyboardString.Length != MaximumSize)
                {

                    if (PhoneNo && char.IsDigit(KeyToString(CurrentKey)[0]) && CurrentKeyboardString.Length < 10
                        || !PhoneNo)
                        CurrentKeyboardString = CurrentKeyboardString.Insert(CurrentKeyboardIndex++, KeyToString(CurrentKey));

                    if (ShiftMode)
                    {
                        CapsMode = false;
                        ShiftMode = false;
                    }
                }
                else if (CurrentKey.Type == Key.KeyType.Left)
                {
                    if (CurrentKeyboardString.Length > 0)
                        CurrentKeyboardString = CurrentKeyboardString.Remove(CurrentKeyboardIndex - 1, 1);
                }
                else if (CurrentKey.Type == Key.KeyType.Right)
                    CurrentKeyboardIndex++;
                else if (CurrentKey.Type == Key.KeyType.Caps)
                {
                    CapsMode = !CapsMode;
                }
                else if (CurrentKey.Type == Key.KeyType.Space)
                {
                    CurrentKeyboardString = CurrentKeyboardString.Insert(CurrentKeyboardIndex++, " ");
                }
                else if (CurrentKey.Type == Key.KeyType.Shift)
                {
                    ShiftMode = !ShiftMode;
                    CapsMode = ShiftMode;
                }
                else if (CurrentKey.Type == Key.KeyType.Submit && CurrentKeyboardString.Length >= MinimumSize)
                {

                    Keys = null;
                    status = tempStatus;
                    OnFinished(CurrentKeyboardString, null);
                    FirstUpdate = true;
                }

                CurrentKeyboardIndex = (int)MathHelper.Clamp(CurrentKeyboardIndex, 0, CurrentKeyboardString.Length);
            }

            if (input.IsPressed(ZuneButtons.DPadLeft))
                KeyboardXPosition = (int)CurrentKeyPos.X - 1;
            else if (input.IsPressed(ZuneButtons.DPadRight))
                KeyboardXPosition = (int)CurrentKeyPos.X + CurrentKey.Size;
            else if (input.IsPressed(ZuneButtons.DPadUp))
                KeyboardYPosition--;
            else if (input.IsPressed(ZuneButtons.DPadDown))
                KeyboardYPosition++;

            if (KeyboardYPosition < 0)
                KeyboardYPosition += KeyboardColSize;

            if (KeyboardYPosition >= KeyboardColSize)
                KeyboardYPosition -= KeyboardColSize;
        }

        internal string KeyToString(Key key)
        {
            string temp;

            temp = key.Value1;//(NumMode ? key.Value2 : key.Value1);

            if (CapsMode && key.Type == Key.KeyType.Normal)
                temp = temp.ToUpper();

            return temp;
        }

        internal void DrawKeyboard()
        {
            Vector2 position = Vector2.Zero;
            for (int i = 0; i < Keys.Length; i++)
            {
                DrawKey(position, Keys[i], Keys[i] == CurrentKey);

                position.X += Keys[i].Size;

                if (position.X >= KeyboardRowSize)
                {
                    position.Y++;
                    position.X -= KeyboardRowSize;
                }
            }

            batch.Draw(TextBox, new Vector2(0, 150), Color.White);

            if (PhoneNo)
            {

                string temp = CurrentKeyboardString;

                if (CurrentKeyboardString.Length > 10)
                    temp = CurrentKeyboardString.Substring(0, 10);
                else if (CurrentKeyboardString.Length < 10)
                    temp = CurrentKeyboardString.PadRight(10, '_');

                string str = "(";
                str += temp.Substring(0, 3);
                str += ") ";
                str += temp.Substring(3, 3);
                str += "-";
                str += temp.Substring(6, 4);
                batch.DrawString(font_14, str, new Vector2(11, 153), Color.Black);
            }
            else
                batch.DrawString(font_14, CurrentKeyboardString, new Vector2(11, 153), Color.Black);
            //Helpers.DrawString(batch, font_14, CurrentKeyboardString, new Vector2(50,215), Vector2.Zero, Color.White);
        }

        Vector2 blockSize = new Vector2(24, 24);

        internal void DrawKey(Vector2 pos, Key key, bool highlighted)
        {
            var ButtonPos = new Vector2(0, 8) + new Vector2(pos.X * blockSize.X, pos.Y * blockSize.Y);

            var TextPos = ButtonPos + new Vector2(blockSize.X * key.Size, blockSize.Y) / 2;

            int amt = 1;

            if (highlighted)
            {
                amt = 3;
            }

            for (int x = 0; x < amt; x++)
            {
                if (key.Size == 1)
                    batch.Draw(Key1, ButtonPos, Color.White);
                else if (key.Size == 2)
                    batch.Draw(Key2, ButtonPos, Color.White);
                else if (key.Size == 6)
                    batch.Draw(Key6, ButtonPos, Color.White);
            }

            if (highlighted)
                Helpers.DrawString(batch, font_12, KeyToString(key), TextPos + new Vector2(1f, 0), font_12.MeasureString(KeyToString(key)) / 2, Color.White);
            else
                Helpers.DrawString(batch, font_10, KeyToString(key), TextPos + new Vector2(.5f, .5f), font_10.MeasureString(KeyToString(key)) / 2f, Color.White);

        }
    }

    internal class Key
    {
        public KeyType Type;

        public string Value1;
        public string Value2;

        public int Size = 1;

        public Key(string val1, string val2)
        {
            Value1 = val1;
            Value2 = val2;

            Type = KeyType.Normal;
        }

        public Key(string val1)
        {
            Value1 = val1;
            Value2 = "";

            Type = KeyType.Normal;
        }

        public Key(KeyType type)
        {
            Type = type;

            switch (type)
            {
                case KeyType.Space:
                    Value1 = "";
                    Value2 = "";
                    Size = 6;
                    break;

                case KeyType.Caps:
                    Value1 = "Caps";
                    Value2 = "Caps";
                    Size = 2;
                    break;

                case KeyType.Shift:
                    Value1 = "Shift";
                    Value2 = "Shift";
                    Size = 2;
                    break;

                case KeyType.Left:
                    Value1 = "<";
                    break;

                case KeyType.Right:
                    Value1 = ">";
                    break;

                case KeyType.Submit:
                    Value1 = "Submit";
                    Value2 = "Submit";
                    Size = 2;
                    break;
            }

        }

        public enum KeyType
        {
            Normal,
            Space,
            Shift,
            Caps,
            Left,
            Right,
            Submit,
        }
    }

}