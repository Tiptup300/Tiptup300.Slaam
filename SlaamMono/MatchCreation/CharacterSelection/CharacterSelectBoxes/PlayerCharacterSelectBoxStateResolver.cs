using Microsoft.Xna.Framework;
using SlaamMono.Library.Input;
using ZzziveGameEngine;

namespace SlaamMono.MatchCreation.CharacterSelection.CharacterSelectBoxes
{
    public class PlayerCharacterSelectBoxStateResolver : IResolver<PlayerCharacterSelectBoxRequest, PlayerCharacterSelectBoxState>
    {
        private readonly IInputService _inputService;

        public PlayerCharacterSelectBoxStateResolver(IInputService inputService)
        {
            _inputService = inputService;
        }

        public PlayerCharacterSelectBoxState Resolve(PlayerCharacterSelectBoxRequest request)
        {
            PlayerCharacterSelectBoxState output;

            output = new PlayerCharacterSelectBoxState();
            output.PlayerIndex = _inputService.GetIndex(request.playeridx);
            output.ParentCharSkins = request.parentcharskins;
            output.ParentSkinStrings = request.parentskinstrings;
            PlayerCharacterSelectBoxPerformer._refreshSkins(output);

            output.Positions[0] = request.Position;
            output.Positions[1] = new Vector2(request.Position.X + 75, request.Position.Y + 125 - 30 + output.Offset - 70);
            output.Positions[2] = new Vector2(request.Position.X + 75, request.Position.Y + 125 - 30 + output.Offset);
            output.Positions[3] = new Vector2(request.Position.X + 75, request.Position.Y + 125 - 30 + output.Offset + 70);
            output.Positions[4] = new Vector2(request.Position.X + 175, request.Position.Y + 108);
            output.Positions[5] = new Vector2(request.Position.X + 188, request.Position.Y + 77);

            output.Positions[6] = new Vector2(request.Position.X + 209, request.Position.Y + 146);
            output.Positions[7] = new Vector2(request.Position.X + 412, request.Position.Y + 146);
            output.Positions[8] = new Vector2(request.Position.X + 209, request.Position.Y + 188);
            output.Positions[9] = new Vector2(request.Position.X + 412, request.Position.Y + 188);

            output.MessageLines[0] = DialogStrings.Player + (ExtendedPlayerIndex)output.PlayerIndex;
            output.MessageLines[1] = DialogStrings.PressStartToJoin;
            output.MessageLines[2] = "";
            output.MessageLines[3] = "";
            output.MessageLines[4] = "";
            output.MessageLines[5] = "";

            output.IsSurvival = request.IsSurvival;

            return output;
        }
    }
}
