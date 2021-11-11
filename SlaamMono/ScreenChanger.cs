using SlaamMono.Library;
using SlaamMono.Library.Screens;

namespace SlaamMono
{
    public class ScreenChanger : ICommand<ScreenChangeRequest>
    {
        private readonly IResolver<ScreenRequest, IScreen> _screenResolver;

        public ScreenChanger(
            IResolver<ScreenRequest, IScreen> screenResolver
            )
        {
            _screenResolver = screenResolver;
        }

        public void Execute(ScreenChangeRequest request)
        {

        }
    }
}
