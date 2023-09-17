using Microsoft.Xna.Framework;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;

namespace Tiptup300.Slaam.States.ZibithLogo;

public class ZibithLogoState : IState
{
   public int StateIndex { get; set; } = -1;
   public Transition StateTransition { get; set; }
   public Color LogoColor { get; set; }

   public CachedTexture BackgroundTexture { get; set; }
   public CachedTexture LogoTexture { get; set; }
}
