using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono
{
    // Xbox 360 + PC Defaults 
    public static class GameGlobals
    {
#if !ZUNE
        public const int DRAWING_GAME_WIDTH = 800;
        public const int DRAWING_GAME_HEIGHT = 480;
        public const int TILE_SIZE = 30;
        public const int BOARD_WIDTH = 20;
        public const int BOARD_HEIGHT = 10;
        public const string TEXTURE_FILE_PATH = "\\MOBILE\\";
        public const string DEFAULT_PLAYER_NAME = "Player";
#endif

#if ZUNE
        public const int DRAWING_GAME_WIDTH = 800;
        public const int DRAWING_GAME_HEIGHT = 480;
        public const int TILE_SIZE = 30;
        public const int BOARD_WIDTH = 20;
        public const int BOARD_HEIGHT = 10;
        public const string TEXTURE_FILE_PATH = "\\MOBILE\\";
        public const string DEFAULT_PLAYER_NAME = "Player";
#endif

        public static void SetupGame()
        {
            // Do Nothing
        }
    }

}