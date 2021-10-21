using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    public abstract class IBlade
    {
        private int height = 30;
        public int Height
        {
            get { return height; }
            set
            {
                if (value > 100 || value < 15)
                    throw new Exception("Invalid Blade Height");

                height = value;
            }
        }

        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch batch, Vector2 Position);
    }
}
