using Microsoft.Xna.Framework;

namespace ZzziveGameEngine.MonoGame
{
    public class ZzzMonoGame : Game
    {
        private readonly IZzzMonoGame _game;

        public ZzzMonoGame(
           IZzzMonoGame game)
        {
            _game = game;
        }

        protected override void Initialize() => _game.Initialize();
        protected override void LoadContent() => _game.LoadContent();
        protected override void Update(GameTime gameTime) => _game.Update();
        protected override void Draw(GameTime gameTime) => _game.Draw();
    }
}
