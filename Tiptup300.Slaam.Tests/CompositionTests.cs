using SlaamMono.Composition.x_;

namespace SlaamMono.Testing;

public class CompositionTests
{
   [Fact]
   public void CompositionWorks()
   {
      ServiceLocator.Instance.GetService<SlamGameConfigurer>();
   }
}
