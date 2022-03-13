using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.x_;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus.Credits
{
    public partial class CreditsPerformer : IStatePerformer
    {
        private const float MovementSpeed = 3f / 120f;

        private CreditsState _state = new CreditsState();

        private readonly IResources _resources;
        private readonly IInputService _inputService;
        private readonly IFrameTimeService _frameTimeService;
        private readonly IResolver<MainMenuRequest, IState> _mainMenuResolver;
        private readonly IRenderService _renderService;

        public CreditsPerformer(
            IResources resources,
            IInputService inputService,
            IFrameTimeService frameTimeService,
            IResolver<MainMenuRequest, IState> mainMenuResolver,
            IRenderService renderService)
        {
            _resources = resources;
            _inputService = inputService;
            _frameTimeService = frameTimeService;
            _mainMenuResolver = mainMenuResolver;
            _renderService = renderService;
        }

        public void InitializeState()
        {
            // to remove
        }

        public IState Perform()
        {
            if (_inputService.GetPlayers()[0].PressedAction)
            {
                _state.Active = !_state.Active;
            }

            if (_state.Active)
            {
                _state.TextCoords = new Vector2(_state.TextCoords.X, _state.TextCoords.Y - MovementSpeed * _frameTimeService.GetLatestFrame().MovementFactor);
            }

            if (_state.TextCoords.Y < -_state.TextHeight - 50)
            {
                _state.TextCoords = new Vector2(_state.TextCoords.X, GameGlobals.DRAWING_GAME_HEIGHT);
            }

            if (_inputService.GetPlayers()[0].PressedAction2)
            {
                return _mainMenuResolver.Resolve(new MainMenuRequest());
            }
            return _state;
        }

        public void RenderState()
        {
            _renderService.Render(batch =>
            {

                float Offset = 0;

                for (int CurrentCredit = 0; CurrentCredit < _state.CreditsListings.Count; CurrentCredit++)
                {
                    if (_state.TextCoords.Y + Offset > 0 && _state.TextCoords.Y + Offset + 20 < GameGlobals.DRAWING_GAME_HEIGHT + _resources.GetFont("SegoeUIx32pt").MeasureString(_state.CreditsListings[CurrentCredit].Name).Y)
                    {
                        RenderService.Instance.RenderText(_state.CreditsListings[CurrentCredit].Name, new Vector2(_state.TextCoords.X, _state.TextCoords.Y + Offset), _resources.GetFont("SegoeUIx32pt"), _state.MainCreditColor, Alignment.TopLeft, false);
                    }
                    Offset += _resources.GetFont("SegoeUIx32pt").MeasureString(_state.CreditsListings[CurrentCredit].Name).Y / 1.5f;
                    for (int x = 0; x < _state.CreditsListings[CurrentCredit].Credits.Count; x++)
                    {
                        if (_state.TextCoords.Y + Offset > 0 && _state.TextCoords.Y + Offset + 10 < GameGlobals.DRAWING_GAME_HEIGHT + _resources.GetFont("SegoeUIx14pt").MeasureString(_state.CreditsListings[CurrentCredit].Credits[x]).Y)
                        {
                            RenderService.Instance.RenderText(_state.CreditsListings[CurrentCredit].Credits[x], new Vector2(_state.TextCoords.X + 10, _state.TextCoords.Y + Offset), _resources.GetFont("SegoeUIx14pt"), _state.SubCreditColor, Alignment.TopLeft, false);
                        }
                        Offset += (int)_resources.GetFont("SegoeUIx14pt").MeasureString(_state.CreditsListings[CurrentCredit].Credits[x]).Y;
                    }
                    Offset += 20;
                }

                _state.TextHeight = Offset;
            });
        }

        public void Close()
        {

        }

    }
}
