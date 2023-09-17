using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.x_;
using Tiptup300.Slaam.Library.Rendering;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation;

public class BoardSelectionScreenPerformer : IPerformer<BoardSelectionScreenState>, IRenderer<BoardSelectionScreenState>
{
   private readonly IResources _resources;
   private readonly IInputService _inputService;
   private readonly IFrameTimeService _frameTimeService;
   private readonly IRenderService _renderService;

   public BoardSelectionScreenPerformer(
       IResources resources,
       IInputService inputService,
       IFrameTimeService frameTimeService,
       IRenderService renderService)
   {
      _resources = resources;
      _inputService = inputService;
      _frameTimeService = frameTimeService;
      _renderService = renderService;
   }

   public void InitializeState()
   {
   }

   public IState Perform(BoardSelectionScreenState state)
   {
      computeAlpha();

      if (state.IsStillLoadingBoards)
      {
         continueLoadingBoards(state);
         return state;
      }
      if (state.WasChosen)
      {
         runWasChosen(state);
      }
      else
      {
         runWasNotChosen(state);

      }
      return state;
   }
   private void computeAlpha() { }
   private void runWasChosen(BoardSelectionScreenState state)
   {
      state.Scale += _frameTimeService.GetLatestFrame().MovementFactor * .01f;

      if (state.Scale >= 1.50f)
      {
         state.Scale = 1.50f;
      }

      if (_inputService.GetPlayers()[0].PressedAction)
      {
         LobbyScreenFunctions.LoadBoard(state.ValidBoards[state.Save], state.ParentLobbyScreen);
         // todo
         //_screenManager.ChangeTo(_state.ParentLobbyScreen);
      }

      if (_inputService.GetPlayers()[0].PressedAction2)
      {
         state.WasChosen = false;
      }
   }
   private void runWasNotChosen(BoardSelectionScreenState state)
   {
      state.Scale -= MathHelper.Max(
             _frameTimeService.GetLatestFrame().MovementFactor * .01f, 1.0f
         );

      checkDpad(state);

      if (state.Vertical == Direction.Down)
      {
         state.VerticalOffset -= _frameTimeService.GetLatestFrame().MovementFactor * state.MovementSpeed;

         if (state.VerticalOffset <= -state.DrawSizeHeight)
         {
            state.VerticalBoardOffset.Add(1);
            state.VerticalOffset = 0;
            state.Vertical = Direction.None;
         }
      }
      else if (state.Vertical == Direction.Up)
      {
         state.VerticalOffset += _frameTimeService.GetLatestFrame().MovementFactor * state.MovementSpeed;

         if (state.VerticalOffset >= state.DrawSizeHeight)
         {
            state.VerticalBoardOffset.Sub(1);
            state.VerticalOffset = 0;
            state.Vertical = Direction.None;
         }
      }

      if (state.Horizontal == Direction.Left)
      {
         state.HorizontalOffset += _frameTimeService.GetLatestFrame().MovementFactor * state.MovementSpeed;

         if (state.HorizontalOffset >= state.DrawSizeWidth)
         {
            state.HorizontalBoardOffset.Add(1);
            state.HorizontalOffset = 0;
            state.Horizontal = Direction.None;
         }
      }
      else if (state.Horizontal == Direction.Right)
      {
         state.HorizontalOffset -= _frameTimeService.GetLatestFrame().MovementFactor * state.MovementSpeed;

         if (state.HorizontalOffset <= -state.DrawSizeWidth)
         {
            state.HorizontalBoardOffset.Sub(1);
            state.HorizontalOffset = 0;
            state.Horizontal = Direction.None;
         }
      }

      if (state.HorizontalOffset == 0 && state.VerticalOffset == 0 && _inputService.GetPlayers()[0].PressedAction)
      {
         state.WasChosen = true;
      }
   }
   private void checkDpad(BoardSelectionScreenState state)
   {
      if (_inputService.GetPlayers()[0].PressingDown)
      {
         state.Vertical = Direction.Down;
      }
      if (_inputService.GetPlayers()[0].PressingUp)
      {
         state.Vertical = Direction.Up;
      }

      if (_inputService.GetPlayers()[0].PressingRight)
      {
         state.Horizontal = Direction.Right;
      }
      if (_inputService.GetPlayers()[0].PressingLeft)
      {
         state.Horizontal = Direction.Left;
      }
   }
   private void continueLoadingBoards(BoardSelectionScreenState _state)
   {
      try
      {
         Texture2D temp = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + _state.BoardNames[_state.CurrentBoardLoading]);

         _state.HasFoundBoard = true;
         _state.IsValidBoard = _state.BoardNames[_state.CurrentBoardLoading];
         _state.BoardTextures.Add(temp);
         _state.ValidBoards.Add(_state.BoardNames[_state.CurrentBoardLoading]);

      }
      catch (InvalidOperationException)
      {
         // Found a .png that is either corrupt or not really a .png, lets just skip it!
      }
      _state.CurrentBoardLoading++;
      if (_state.CurrentBoardLoading == _state.BoardNames.Count)
      {
         finishLoadingBoards(_state);
      }
   }
   private void finishLoadingBoards(BoardSelectionScreenState _state)
   {
      _state.IsStillLoadingBoards = false;
      _state.DrawingBoardIndex = new IntRange(0, 0, _state.BoardTextures.Count - 1);
      _state.VerticalBoardOffset = new IntRange(0, 0, _state.BoardTextures.Count - 1);
      _state.HorizontalBoardOffset = new IntRange(-_state.BoardTextures.Count, -_state.BoardTextures.Count, -1);
   }

   public void Render(BoardSelectionScreenState state, int width, int height)
   {
      _renderService.Render(batch =>
      {

         state.DrawingBoardIndex.Value = state.VerticalBoardOffset.Value;
         for (int x = state.HorizontalBoardOffset.Value; x < 8; x++)
         {
            for (int y = 0; y < 11; y++)
            {
               Vector2 Pos = new Vector2(width / 2 - state.DrawSizeWidth / 2 + x * state.DrawSizeWidth + state.HorizontalOffset - state.DrawSizeWidth * 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - state.DrawSizeHeight / 2 + y * state.DrawSizeHeight + state.VerticalOffset - state.DrawSizeHeight * 2);
               if (!(Pos.X < -state.DrawSizeWidth || Pos.X > GameGlobals.DRAWING_GAME_WIDTH || Pos.Y < -state.DrawSizeHeight || Pos.Y > GameGlobals.DRAWING_GAME_HEIGHT))
               {
                  if (state.DrawingBoardIndex.Value < state.BoardTextures.Count && state.BoardTextures[state.DrawingBoardIndex.Value] != null)
                  {
                     batch.Draw(state.BoardTextures[state.DrawingBoardIndex.Value], new Rectangle((int)Pos.X, (int)Pos.Y, state.DrawSizeWidth, state.DrawSizeHeight), Color.White);
                  }
                  else
                  {
                     batch.Draw(_resources.GetTexture("NowLoading").Texture, Pos, Color.White);
                  }
               }
               if (Pos == new Vector2(state.CenteredRectangle.X, state.CenteredRectangle.Y))
               {
                  state.Save = state.DrawingBoardIndex.Value;
               }
               state.DrawingBoardIndex.Add(1);
            }
         }
         batch.Draw(_resources.GetTexture("MenuTop").Texture, Vector2.Zero, Color.White);
         if (!state.IsStillLoadingBoards)
         {
            state.CenteredRectangle = centerRectangle(new Rectangle(0, 0, (int)(state.Scale * state.DrawSizeWidth), (int)(state.Scale * state.DrawSizeHeight)), new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2));
            if (state.WasChosen)
            {
               batch.Draw(state.BoardTextures[state.Save], state.CenteredRectangle, Color.White);
            }
            batch.Draw(_resources.GetTexture("BoardSelect").Texture, state.CenteredRectangle, new Color((byte)255, (byte)255, (byte)255, (byte)state.Alpha));
            _renderService.RenderText(DialogStrings.CleanMapName(state.ValidBoards[state.Save]), new Vector2(27, 225), _resources.GetFont("SegoeUIx32pt"), Color.White, Alignment.TopLeft, true);
         }
      });
   }
   private Rectangle centerRectangle(Rectangle rect, Vector2 pos)
   {
      return new Rectangle((int)pos.X - rect.Width / 2, (int)pos.Y - rect.Height / 2, rect.Width, rect.Height);
   }

   public void Render(BoardSelectionScreenState state)
   {
      throw new NotImplementedException();
   }
}
