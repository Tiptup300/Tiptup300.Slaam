using System;
using System.Linq;

namespace SlaamMono.Library.Screens
{
    public class ScreenChangeRequest
    {
        public Type ScreenType { get; private set; }
        public object Data { get; private set; }

        public ScreenChangeRequest(Type screenType, object data)
        {
            if (screenType.GetInterfaces().Contains(typeof(IStateUpdater)) == false)
            {
                throw new Exception("Cannot change to non IScreen Type");
            }
            ScreenType = screenType;
            Data = data;
        }
    }
}
