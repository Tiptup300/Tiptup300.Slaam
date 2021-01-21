using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
{
    public class MuhLazza : Powerup
    {
        private Vector2 CharPosition;
        private Direction CharDirection;
        private readonly Texture2D LazzaTex;
        private Rectangle DrawingRect;
        private float Width = 5;
        private const float MovementSpeed = (20f / 10f);

        public MuhLazza(GameScreen parentgamescreen)
            : base("Muh Lazza", Color.Blue, parentgamescreen)
        {
            LazzaTex = Resources.MuhLazza1.Texture;
        }

        public override void BeginAttack(Vector2 charposition, Direction chardirection)
        {
            Active = true;
            CharPosition = charposition;
            CharDirection = chardirection;
            DrawingRect = new Rectangle((int)CharPosition.X, (int)CharPosition.Y-30, 5, 30);
        }

        public override void UpdateAttack()
        {
            Width += FPSManager.MovementFactor * MovementSpeed;
            DrawingRect.Width = (int)Math.Round(Width);
        }

        public override void EndAttack()
        {
            Active = false;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(LazzaTex, DrawingRect, Color.White);
        }
    }
}
