using SlaamMono.MatchCreation;
using SlaamMono.Menus.Credits;
using SlaamMono.PlayerProfiles;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;
using ZzziveGameEngine.StateManagement.States;

namespace SlaamMono.Menus
{
   public class MainMenuScreenPerformer : IPerformer<MainMenuScreenState>, IRenderer<MainMenuScreenState>
   {
      private MainMenuScreenState _state;
      private readonly IResolver<IRequest, IState> _stateResolver;

      public MainMenuScreenPerformer(IResolver<IRequest, IState> stateResolver)
      {
         _stateResolver = stateResolver;
      }

      public void InitializeState()
      {

         _state = new MainMenuScreenState();
      }



      private void selectedCredits(object sender, EventArgs e) => _state.NextState = _stateResolver.Resolve(new CreditsRequest());
      private void selectedHighscores(object sender, EventArgs e) => _state.NextState = new HighScoreScreenRequestState();
      private void selectedManageProfiles(object sender, EventArgs e) => _state.NextState = _stateResolver.Resolve(new ProfileEditScreenRequest());
      private void selectedSurvival(object sender, EventArgs e) => _state.NextState = new CharacterSelectionScreenState() { isForSurvival = true };
      private void selectedClassicMode(object sender, EventArgs e) => _state.NextState = _stateResolver.Resolve(new CharacterSelectionScreenRequest());
      private void exitGame(object sender, EventArgs e) => _state.NextState = new GameExitState();


      public IState Perform(MainMenuScreenState state)
      {
         if (state.NextState != null)
         {
            return state.NextState;
         }
         else
         {
            return state;
         }
      }
      public void Render(MainMenuScreenState state) { }

   }

}
