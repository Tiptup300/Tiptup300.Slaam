using SlaamMono.Library.Screens;

namespace SlaamMono.Testing
{
    public class TestFirstScreenResolver : IFirstScreenResolver
    {
        public bool __Resolve__Ran { get; set; }
        public IScreen __Resolve__Output { get; set; }
        public IScreen Resolve()
        {
            __Resolve__Ran = true;
            return __Resolve__Output;
        }
    }
}
