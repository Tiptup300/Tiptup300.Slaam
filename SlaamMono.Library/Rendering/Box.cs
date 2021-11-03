using Microsoft.Xna.Framework;

namespace SlaamMono.Library.Rendering
{
    public class Box
    {
        public Rectangle Destination { get; private set; }
        public Color Color { get; private set; }

        public Box(Rectangle destination, Color color)
        {
            Destination = destination;
            Color = color;
        }
    }
}
