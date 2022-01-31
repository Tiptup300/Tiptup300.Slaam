namespace SlaamMono.Library.Screens
{
    public class ScreenRequest : IRequest
    {
        public string Name { get; private set; }

        public ScreenRequest(string name)
        {
            Name = name;
        }
    }
}
