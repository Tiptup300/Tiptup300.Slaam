using System;
using System.Collections.Generic;
using System.Text;

namespace Slaam
{
    // Xbox 360 + PC Defaults 
    public static class GameGlobals
    {
#if !ZUNE
        public const int DRAWING_GAME_WIDTH = 1280;
        public const int DRAWING_GAME_HEIGHT = 720;
        public const int TILE_SIZE = 55;
        public const int BOARD_WIDTH = 20;
        public const int BOARD_HEIGHT = 10;
        public const string TEXTURE_FILE_PATH = "\\HD\\";
        public const string DEFAULT_PLAYER_NAME = "Guest";
#endif

#if ZUNE
        public const int DRAWING_GAME_WIDTH = 240;
        public const int DRAWING_GAME_HEIGHT = 320;
        public const int TILE_SIZE = 30;
        public const int BOARD_WIDTH = 07;
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