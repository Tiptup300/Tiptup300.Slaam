using SlaamMono.Library.ResourceManagement;
using SlaamMono.Subclasses;
using SlaamMono.SubClasses;

namespace SlaamMono.Menus
{
    public class LogoScreenState
    {
        public Timer DisplayTime { get; set; }
        public Transition LogoColorTransition { get; set; }
        public bool HasShown { get; set; }
        public CachedTexture BackgroundTexture { get; set; }
        public CachedTexture LogoTexture { get; set; }
    }
}
