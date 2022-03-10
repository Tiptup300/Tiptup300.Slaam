using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using System.Collections.Generic;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenState : IState
    {
        public LobbyScreenPerformer ParentLobbyScreen;
        public int DrawSizeWidth = 75;
        public int DrawSizeHeight = 75;
        public List<Texture2D> BoardTextures = new List<Texture2D>();
        public List<string> ValidBoards = new List<string>();
        public IntRange DrawingBoardIndex = new IntRange(0, 0, 0);
        public float MovementSpeed = 0.50f;
        public bool AlphaUp = false;
        public float Alpha = 255f;
        public Direction Vertical = Direction.None;
        public Direction Horizontal = Direction.None;
        public float VerticalOffset = 0f;
        public float HorizontalOffset = 0f;
        public bool IsStillLoadingBoards = true;
        public List<string> BoardNames;
        public int CurrentBoardLoading = 0;
        public int Save;
        public IntRange VerticalBoardOffset;
        public IntRange HorizontalBoardOffset;
        public bool WasChosen = false;
        public float Scale = 1.00f;
        public Rectangle CenteredRectangle;
        public bool HasFoundBoard = false;
        public string IsValidBoard;
    }
}
