using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Input;
using System.Collections.Generic;
using ZzziveGameEngine;

namespace SlaamMono.MatchCreation
{
    public class PlayerCharacterSelectBoxRequest : IRequest
    {
        public Vector2 Position { get; set; }
        public Texture2D[] parentcharskins { get; set; }
        public ExtendedPlayerIndex playeridx { get; set; }
        public List<string> parentskinstrings { get; set; }
        public bool IsSurvival { get; set; }
    }
}
