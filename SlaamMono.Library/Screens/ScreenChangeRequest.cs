using System;

namespace SlaamMono.Library.Screens
{
    public class ScreenChangeRequest
    {
        public Type ScreenType { get; private set; }
        public object Data { get; private set; }

        public ScreenChangeRequest(Type screenType, object data)
        {
            ScreenType = screenType;
            Data = data;
        }
    }
}
