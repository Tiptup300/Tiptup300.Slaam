namespace SlaamMono.Screens
{
    public class FirstScreenResolver : IFirstScreenResolver
    {
        private readonly IScreenFactory _screenFactory;

        public FirstScreenResolver(
            IScreenFactory screenFactory)
        {
            _screenFactory = screenFactory;
        }

        public IScreen Resolve()
        {
            return _screenFactory.Get(nameof(LogoScreen));
        }
    }
}
