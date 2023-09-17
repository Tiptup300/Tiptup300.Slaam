using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.MatchCreation;
using System;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.States.BoardSelect;

 public class BoardSelectionScreenRequestResolver : IResolver<BoardSelectionScreenRequest, IState>
 {
     private readonly IResources _resources;

     public BoardSelectionScreenRequestResolver(IResources resources)
     {
         _resources = resources;
     }

     public IState Resolve(BoardSelectionScreenRequest request)
     {
         BoardSelectionScreenState output;

         output = new BoardSelectionScreenState();
         output.ParentLobbyScreen = request.ParentScreen;
         output.BoardNames = _resources.GetTextList("Boards");
         setBoardIndexs(output);

         return output;
     }
     private void setBoardIndexs(BoardSelectionScreenState state)
     {
         state.DrawingBoardIndex = new IntRange(0, 0, state.BoardNames.Count - 1);
         state.VerticalBoardOffset = new IntRange(0);
         state.HorizontalBoardOffset = new IntRange(-state.BoardNames.Count);
     }

 }
