using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class BoardSelectionScreenResolver : IResolver<BoardSelectionScreenRequest, BoardSelectionScreen>
    {
        private readonly IResources _resources;
        private readonly IScreenManager _screenManager;

        public BoardSelectionScreenResolver(IResources resources, IScreenManager screenManager)
        {
            _resources = resources;
            _screenManager = screenManager;
        }

        public BoardSelectionScreen Resolve(BoardSelectionScreenRequest request)
        {
            BoardSelectionScreen output;

            output = new BoardSelectionScreen(_resources, _screenManager);
            output.Initialize(request);

            return output;
        }
    }
}
