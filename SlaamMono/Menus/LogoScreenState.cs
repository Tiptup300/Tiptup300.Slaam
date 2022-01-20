using Microsoft.Xna.Framework;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Subclasses;

namespace SlaamMono.Menus
{
    public class LogoScreenState
    {
        public int StateIndex { get; set; }
        public Transition StateTransition { get; set; }
        public Color LogoColor { get; set; }

        public CachedTexture BackgroundTexture { get; set; }
        public CachedTexture LogoTexture { get; set; }
    }
}
