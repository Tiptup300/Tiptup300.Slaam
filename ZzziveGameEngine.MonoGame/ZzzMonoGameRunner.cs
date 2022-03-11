using System.Collections.Generic;

namespace ZzziveGameEngine.MonoGame
{
    public class ZzzMonoGameRunner : IZzzApp
    {
        private readonly ZzzMonoGame _game;
        private readonly IEnumerable<ZzzMonoGameComponent> _components;
        public ZzzMonoGameRunner(
           ZzzMonoGame game,
           IEnumerable<ZzzMonoGameComponent> components)
        {
            _game = game;
            _components = components;
        }

        public void Run()
        {
            addComponentsToGame();
            _game.Run();
        }

        private void addComponentsToGame()
        {
            foreach (var component in _components)
            {
                _game.Components.Add(component);
            }
        }
    }
}
