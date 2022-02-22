using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.x_;
using System.Collections.Generic;

namespace SlaamMono.Menus
{
    public partial class CreditsScreen : IStateUpdater
    {
        private const float MovementSpeed = 3f / 120f;

        private CreditsState _state = new CreditsState();

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;

        public CreditsScreen(IScreenManager screenDirector, IResources resources)
        {
            _screenDirector = screenDirector;
            _resources = resources;
        }

        public void InitializeState()
        {
            _state.credits = _resources.GetTextList("Credits").ToArray();
            BackgroundManager.ChangeBG(BackgroundType.Credits);
            for (int x = 0; x < _state.credits.Length; x++)
            {
                string[] credinfo = _state.credits[x].Replace("\r", "").Split("|".ToCharArray());
                string credname = credinfo[0];
                List<string> credcreds = new List<string>();
                for (int y = 1; y < credinfo.Length; y++)
                {
                    credcreds.Add(credinfo[y]);
                }
                _state.CreditsListings.Add(new CreditsListing(credname, credcreds));
            }
        }
        public void UpdateState()
        {
            if (InputComponent.Players[0].PressedAction)
            {
                _state.Active = !_state.Active;
            }

            if (_state.Active)
            {
                _state.TextCoords = new Vector2(_state.TextCoords.X, _state.TextCoords.Y - MovementSpeed * FrameRateDirector.MovementFactor);
            }

            if (_state.TextCoords.Y < -_state.TextHeight - 50)
            {
                _state.TextCoords = new Vector2(_state.TextCoords.X, GameGlobals.DRAWING_GAME_HEIGHT);
            }

            if (InputComponent.Players[0].PressedAction2)
            {
                _screenDirector.ChangeTo<IMainMenuScreen>();
            }
        }

        public void RenderState(SpriteBatch batch)
        {
            float Offset = 0;

            for (int CurrentCredit = 0; CurrentCredit < _state.CreditsListings.Count; CurrentCredit++)
            {
                if (_state.TextCoords.Y + Offset > 0 && _state.TextCoords.Y + Offset + 20 < GameGlobals.DRAWING_GAME_HEIGHT + _resources.GetFont("SegoeUIx32pt").MeasureString(_state.CreditsListings[CurrentCredit].Name).Y)
                {
                    RenderGraph.Instance.RenderText(_state.CreditsListings[CurrentCredit].Name, new Vector2(_state.TextCoords.X, _state.TextCoords.Y + Offset), _resources.GetFont("SegoeUIx32pt"), _state.MainCreditColor, Alignment.TopLeft, false);
                }
                Offset += _resources.GetFont("SegoeUIx32pt").MeasureString(_state.CreditsListings[CurrentCredit].Name).Y / 1.5f;
                for (int x = 0; x < _state.CreditsListings[CurrentCredit].Credits.Count; x++)
                {
                    if (_state.TextCoords.Y + Offset > 0 && _state.TextCoords.Y + Offset + 10 < GameGlobals.DRAWING_GAME_HEIGHT + _resources.GetFont("SegoeUIx14pt").MeasureString(_state.CreditsListings[CurrentCredit].Credits[x]).Y)
                    {
                        RenderGraph.Instance.RenderText(_state.CreditsListings[CurrentCredit].Credits[x], new Vector2(_state.TextCoords.X + 10, _state.TextCoords.Y + Offset), _resources.GetFont("SegoeUIx14pt"), _state.SubCreditColor, Alignment.TopLeft, false);
                    }
                    Offset += (int)_resources.GetFont("SegoeUIx14pt").MeasureString(_state.CreditsListings[CurrentCredit].Credits[x]).Y;
                }
                Offset += 20;
            }

            _state.TextHeight = Offset;
        }

        public void Close()
        {

        }
    }
}
