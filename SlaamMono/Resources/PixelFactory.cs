using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources
{
    public class PixelFactory : IPixelFactory
    {
        public Texture2D BuildPixel()
        {
            Texture2D output;

            output = new Texture2D(SlaamGame.Graphics.GraphicsDevice, 1, 1);
            output.SetData(new Color[] { Color.White });

            return output;
        }

    }
}
