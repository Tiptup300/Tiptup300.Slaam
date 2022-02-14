using Microsoft.Xna.Framework;
using SlaamMono.x_;
using System.Collections.Generic;
using ZzziveGameEngine;

namespace SlaamMono.Menus
{
    public class CreditsState : IState
    {
        public string[] credits { get; set; }
        public List<CreditsListing> CreditsListings { get; set; } = new List<CreditsListing>();

        public Color MainCreditColor { get; set; } = Color.White;
        public Color SubCreditColor { get; set; } = Color.White;
        public Vector2 TextCoords { get; set; } = new Vector2(5, GameGlobals.DRAWING_GAME_HEIGHT);
        public bool Active { get; set; } = false;
        public float TextHeight { get; set; } = 0f;
    }
}
