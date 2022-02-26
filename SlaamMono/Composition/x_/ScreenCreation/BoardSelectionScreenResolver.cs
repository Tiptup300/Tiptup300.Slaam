using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class BoardSelectionScreenResolver : IResolver<BoardSelectionScreenRequestState, BoardSelectionScreen>
    {
        private readonly BoardSelectionScreen _boardSelectionScreen;

        public BoardSelectionScreenResolver(BoardSelectionScreen boardSelectionScreen)
        {
            _boardSelectionScreen = boardSelectionScreen;
        }

        public BoardSelectionScreen Resolve(BoardSelectionScreenRequestState request)
        {
            BoardSelectionScreen output;

            output = _boardSelectionScreen;
            output.Initialize(request);

            return output;
        }
    }
}
