namespace SlaamMono.Input
{
    public struct QwertyKey
    {
        public bool Selected;
        public string Chars;
        public QwertyKeyType Type;

        public QwertyKey(string chars, QwertyKeyType type)
        {
            Selected = false;
            Chars = chars;
            Type = type;
        }
    }
}
