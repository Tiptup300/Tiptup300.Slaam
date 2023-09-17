using Tiptup300.Slaam.Library.Widgets;

namespace Tiptup300.Slaam.Tests;

public class TransistionTests
{
   [Fact]
   public void CanDoBasicTransition()
   {
      Transition transition = new Transition(TimeSpan.FromSeconds(3));

      transition.AddProgress(TimeSpan.FromSeconds(1));
      transition.AddProgress(TimeSpan.FromSeconds(1));
      transition.AddProgress(TimeSpan.FromSeconds(1));

      Assert.True(transition.IsFinished);
   }
}
