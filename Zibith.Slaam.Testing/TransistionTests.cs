using NUnit.Framework;
using SlaamMono.Subclasses;
using System;

namespace SlaamMono.Testing
{
    public class TransistionTests
    {
        [Test]
        public void CanDoBasicTransition()
        {
            Transition transition = new Transition(TimeSpan.FromSeconds(3));

            transition.AddProgress(TimeSpan.FromSeconds(1));
            transition.AddProgress(TimeSpan.FromSeconds(1));
            transition.AddProgress(TimeSpan.FromSeconds(1));

            Assert.True(transition.IsFinished);
        }
    }
}
