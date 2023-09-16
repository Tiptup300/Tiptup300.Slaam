using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus.Credits
{
    public struct CreditsState : IState
    {
        public string[] Credits { get; set; }
        public List<CreditsListing> CreditsListings { get; set; }

        public Color MainCreditColor { get; set; }
        public Color SubCreditColor { get; set; }
        public Vector2 TextCoords { get; set; }
        public bool Active { get; set; }
        public float TextHeight { get; set; }
    }
}
