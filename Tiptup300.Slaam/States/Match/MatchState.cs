using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.Match.Actors;
using Tiptup300.Slaam.States.Match.Boards;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.Match.Scoreboard;
using Tiptup300.Slaam.States.Match.Timer;

namespace Tiptup300.Slaam.States.Match;

public class MatchState : IState
{
   public bool IsTiming { get; set; } = false;
   public bool IsPaused { get; set; } = false;
   public int KillsToWin { get; set; } = 0;
   public Library.Widgets.TimerWidget PowerupTime { get; set; }
   public float SpreeStepSize { get; set; }
   public float SpreeCurrentStep { get; set; }
   public int SpreeHighestKillCount { get; set; }
   public int StepsRemaining { get; set; }
   public Vector2 Boardpos { get; set; }
   public int BoardSize { get; set; } = 0;

   public Texture2D Tileset { get; set; }
   public MatchTimer Timer { get; set; }
   public List<CharacterShell> SetupCharacters { get; set; }
   public int NullChars { get; set; } = 0;
   public List<MatchScoreboard> Scoreboards { get; set; } = new List<MatchScoreboard>();
   public Random Rand { get; set; } = new Random();
   public GameStatus CurrentGameStatus { get; set; }
   public int ReadySetGoPart { get; set; } = 0;
   public Library.Widgets.TimerWidget ReadySetGoThrottle { get; set; }
   public MatchScoreCollection ScoreKeeper { get; set; }

   public Tile[,] Tiles { get; set; }
   public GameType GameType { get; set; }
   public List<CharacterActor> Characters { get; set; } = new List<CharacterActor>();

   public MatchSettings CurrentMatchSettings { get; set; }

   public SurvivalGameScreenState SurvivalState { get; set; } = new SurvivalGameScreenState();

   public bool EndGameSelected { get; set; }
}
