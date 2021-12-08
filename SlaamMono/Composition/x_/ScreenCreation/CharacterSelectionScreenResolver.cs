using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;

namespace SlaamMono.Composition.x_.ScreenCreation
{
    public class CharacterSelectionScreenResolver : IResolver<CharacterSelectionScreenRequest, CharacterSelectionScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;

        public CharacterSelectionScreenResolver(ILogger logger, IScreenManager screenDirector)
        {
            _logger = logger;
            _screenDirector = screenDirector;
        }

        public CharacterSelectionScreen Resolve(CharacterSelectionScreenRequest request)
        {
            CharacterSelectionScreen output;

            output = new CharacterSelectionScreen(_logger, _screenDirector);
            output.Initialize(request);

            return output;
        }
    }
}
