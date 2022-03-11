using NUnit.Framework;
using SlaamMono.Composition.x_;
using ZzziveGameEngine;

namespace SlaamMono.Testing
{
    public class CompositionTests
    {
        [Test]
        public void CompositionWorks()
        {
            x_Di.Get<SlaamGameApp>();
        }
    }
}
