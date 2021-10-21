namespace ZBlade
{
    public class Key
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