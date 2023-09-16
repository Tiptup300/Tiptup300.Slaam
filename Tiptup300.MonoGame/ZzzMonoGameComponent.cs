using Microsoft.Xna.Framework;

namespace ZzziveGameEngine.MonoGame
{
    public class ZzzMonoGameComponent : DrawableGameComponent
    {
        private readonly IZzzMonoGameComponent _component;

        public ZzzMonoGameComponent(
           Game game,
           IZzzMonoGameComponent component)
           : base(game)
        {
            _component = component;
        }

        public override void Initialize() => _component.Initialize();
        protected override void LoadContent() => _component.LoadContent();
        public override void Update(GameTime gameTime) => _component.Update(gameTime);
        public override void Draw(GameTime gameTime) => _component.Draw(gameTime);
        protected override void Dispose(bool disposing) => _component.Dispose(disposing);
    }
}
