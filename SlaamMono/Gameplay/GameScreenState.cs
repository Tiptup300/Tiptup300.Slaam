using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Gameplay.Boards;
using SlaamMono.PlayerProfiles;
using SlaamMono.SubClasses;
using System;
using System.Collections.Generic;
using ZBlade;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay
{
    public class GameScreenState : IState
    {
        public bool Timing { get; set; } = false;
        public bool IsPaused { get; set; } = false;
        public int KillsToWin { get; set; } = 0;
        public Timer PowerupTime { get; set; }
        public float SpreeStepSize { get; set; }
        public float SpreeCurrentStep { get; set; }
        public int SpreeHighestKillCount { get; set; }
        public int StepsRemaining { get; set; }
        public Vector2 Boardpos { get; set; }
        public int BoardSize { get; set; } = 0;

        public Texture2D Tileset { get; set; }
        public GameScreenTimer Timer { get; set; }
        public List<CharacterShell> SetupCharacters { get; set; }
        public int NullChars { get; set; } = 0;
        public List<Scoreboard> Scoreboards { get; set; } = new List<Scoreboard>();
        public Random Rand { get; set; } = new Random();
        public GameStatus CurrentGameStatus { get; set; }
        public int ReadySetGoPart { get; set; } = 0;
        public Timer ReadySetGoThrottle { get; set; }
        public MatchScoreCollection ScoreKeeper { get; set; }

        public Tile[,] Tiles { get; set; }
        public MenuItemTree main { get; set; } = new MenuItemTree();
        public GameType ThisGameType { get; set; }
        public List<CharacterActor> Characters { get; set; } = new List<CharacterActor>();
    }
}
