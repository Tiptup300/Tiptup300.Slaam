using SlaamMono.Library;
using SlaamMono.Library.Graphing;
using ZzziveGameEngine;

namespace SlaamMono.PlayerProfiles
{
    public class ProfileEditScreenState : IState
    {
        public Graph MainMenu;
        public Graph SubMenu;
        public IntRange CurrentMenu = new IntRange(0, 0, 1);
        public IntRange CurrentMenuChoice = new IntRange(0, 0, 0);
        public int EditingProfile;
        public bool WaitingForQwerty = false;
        public bool SetupNewProfile = false;
    }
}
