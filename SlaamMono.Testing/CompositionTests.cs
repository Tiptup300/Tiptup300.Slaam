﻿using NUnit.Framework;
using SlaamMono.Composition;
using SlaamMono.Library;

namespace SlaamMono.Testing
{
    public class CompositionTests
    {
        [Test]
        public void CompositionWorks()
        {
            x_Di.Get<IApp>();
        }
    }
}