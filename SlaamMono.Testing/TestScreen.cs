using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Screens;

namespace SlaamMono.Testing
{
    public class TestScreen : IScreen
    {
        public bool __Close__Ran { get; private set; }
        public void Close() => __Close__Ran = true;

        public bool __Draw__Ran { get; private set; }
        public SpriteBatch __Draw_InputParam_batch { get; private set; }
        public void Draw(SpriteBatch batch)
        {
            __Draw__Ran = true;
            __Draw_InputParam_batch = batch;
        }

        public bool __Open__Ran { get; private set; }
        public void Open() => __Open__Ran = true;

        public bool __Update__Ran { get; private set; }
        public void Update() => __Update__Ran = true;
    }
}
