namespace Tiptup300.Slaam.x_;

public record GameConfiguration
(
   int DRAWING_GAME_WIDTH = 800,
   int DRAWING_GAME_HEIGHT = 480,
   int TILE_SIZE = 30,
   int BOARD_WIDTH = 20,
   int BOARD_HEIGHT = 10,
   string TEXTURE_FILE_PATH = "\\MOBILE\\",
   string DEFAULT_PLAYER_NAME = "Player",
   bool ShowFPS = true
);