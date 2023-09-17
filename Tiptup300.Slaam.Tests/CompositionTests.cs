
using Tiptup300.Slaam.Composition.x_;

namespace Tiptup300.Slaam.Tests;

public class CompositionTests
{
   [Fact]
   public void CompositionWorks()
   {
      ServiceLocator.Instance.GetService<SlamGameConfigurer>();
   }
}
