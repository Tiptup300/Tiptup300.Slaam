using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using System;

namespace SlaamMono.Gameplay
{
    /// <summary>
    /// Scoreboards for the players in a game.
    /// </summary>
    public class MatchScoreboard
    {
        public bool Moving = false;

        private Vector2 Position;
        private CharacterActor Character;
        private const float MovementSpeed = 20f / 10f;
        private GameType CurrentGametype;
        private bool AlphaUp = false;
        private float Alpha = 255f;

        private readonly IFrameTimeService _frameTimeService;
        private readonly IResources _resources;

        public MatchScoreboard(IResources resources, IResolver<WhitePixelRequest, Texture2D> whitePixelResolver,
            IFrameTimeService frameTimeService)
        {
            _resources = resources;
            _frameTimeService = frameTimeService;
        }

        public void Initialize(MatchScoreboardRequest request)
        {
            Position = request.Position;
            Character = request.Character;
            CurrentGametype = request.GameType;
        }

        public void Update()
        {
            if (Moving)
            {
                Position.X += _frameTimeService.GetLatestFrame().MovementFactor * MovementSpeed;
                if (Position.X >= 0)
                {
                    Position.X = 0;
                    Moving = false;
                }
            }
            if (Character.CurrentPowerup != null && Character.CurrentPowerup.Active)
            {
                Alpha += (AlphaUp ? 1 : -1) * _frameTimeService.GetLatestFrame().MovementFactor;

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
            {
                Alpha = 255f;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_resources.GetTexture("GameScreenScoreBoard").Texture, Position, Color.White);
            RenderService.Instance.RenderText(Character.GetProfile().Name, new Vector2(8 + Position.X, 18 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopLeft, true);
            RenderService.Instance.RenderText(Character.Kills.ToString(), new Vector2(35 + Position.X, 68 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
            if (CurrentGametype == GameType.Classic || CurrentGametype == GameType.Survival)
            {
                RenderService.Instance.RenderText(Character.Lives.ToString(), new Vector2(73 + Position.X, 68 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
            }
            else if (CurrentGametype == GameType.Spree || CurrentGametype == GameType.TimedSpree)
            {
                RenderService.Instance.RenderText("inf.", new Vector2(73 + Position.X, 68 + Position.Y), _resources.GetFont("SegoeUIx14pt"), Color.White, Alignment.TopCenter, true);
            }
            Character.Draw(batch, new Vector2(184 + Position.X, 61 + Position.Y));
            RenderService.Instance.RenderRectangle(
                destinationRectangle: new Rectangle((int)Math.Round(12 + Position.X), (int)Math.Round(30 + Position.Y), 5, 33),
                color: Character.MarkingColor);
            if (Character.CurrentPowerup != null && !Character.CurrentPowerup.Used)
            {
                batch.Draw(Character.CurrentPowerup.SmallTex, new Vector2(125 + Position.X - Character.CurrentPowerup.SmallTex.Width / 2, 42 + Position.Y - Character.CurrentPowerup.SmallTex.Height / 2), new Color((byte)255, (byte)255, (byte)255, (byte)Alpha));
            }
        }
    }
}
