using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using System;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay
{
    /// <summary>
    /// Scoreboards for the players in a game.
    /// </summary>
    public class Scoreboard
    {
        public bool Moving = false;

        private Vector2 Position;
        private CharacterActor Character;
        private const float MovementSpeed = 20f / 10f;
        private GameType CurrentGametype;
        private bool AlphaUp = false;
        private float Alpha = 255f;

        private readonly IResolver<WhitePixelRequest, Texture2D> _whitePixelResolver;
        private readonly IResources _resources;

        public Scoreboard(IResources resources, IResolver<WhitePixelRequest, Texture2D> whitePixelResolver)
        {
            _resources = resources;
            _whitePixelResolver = whitePixelResolver;
        }

        public void Initialize(ScoreboardRequest request)
        {
            Position = request.Position;
            Character = request.Character;
            CurrentGametype = request.GameType;
        }

        public void Update()
        {
            if (Moving)
            {
                Position.X += FrameRateDirector.MovementFactor * MovementSpeed;
                if (Position.X >= 0)
                {
                    Position.X = 0;
                    Moving = false;
                }

            }
            if (Character.CurrentPowerup != null && Character.CurrentPowerup.Active)
            {
                Alpha += (AlphaUp ? 1 : -1) * FrameRateDirector.MovementFactor;

                if (AlphaUp && Alpha >= 255f)
                {
                    AlphaUp = !AlphaUp;
                    Alpha = 255f;
                }
                else if (!AlphaUp && Alpha <= 0f)
                {
                    AlphaUp = !AlphaUp;
                    Alpha = 0f;
                }
            }
            else
                Alpha = 255f;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("GameScreenScoreBoard").Texture, Position, Color.White);
            RenderGraph.Instance.RenderText(Character.GetProfile().Name, new Vector2(8 + Position.X, 18 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopLeft, true);
            RenderGraph.Instance.RenderText(Character.Kills.ToString(), new Vector2(35 + Position.X, 68 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
            if (CurrentGametype == GameType.Classic || CurrentGametype == GameType.Survival)
            {
                RenderGraph.Instance.RenderText(Character.Lives.ToString(), new Vector2(73 + Position.X, 68 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
            }
            else if (CurrentGametype == GameType.Spree || CurrentGametype == GameType.TimedSpree)
            {
                RenderGraph.Instance.RenderText("inf.", new Vector2(73 + Position.X, 68 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
            }
            Character.Draw(batch, new Vector2(184 + Position.X, 61 + Position.Y));
            batch.Draw(
                _whitePixelResolver.Resolve(new WhitePixelRequest()),
                new Rectangle((int)Math.Round(12 + Position.X), (int)Math.Round(30 + Position.Y), 5, 33),
                Character.MarkingColor);
            if (Character.CurrentPowerup != null && !Character.CurrentPowerup.Used)
            {
                batch.Draw(Character.CurrentPowerup.SmallTex, new Vector2(125 + Position.X - Character.CurrentPowerup.SmallTex.Width / 2, 42 + Position.Y - Character.CurrentPowerup.SmallTex.Height / 2), new Color((byte)255, (byte)255, (byte)255, (byte)Alpha));
            }
        }
    }
}
