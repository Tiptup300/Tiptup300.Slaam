using Microsoft.Xna.Framework;
using System.Tiptup300.Primitives;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.x_;

namespace Tiptup300.Slaam.States.Credits;

public class CreditsRequestResolver : IResolver<CreditsRequest, IState>
{
   private readonly IResources _resources;
   private readonly GameConfiguration _gameConfiguration;

   public CreditsRequestResolver(IResources resources, GameConfiguration gameConfiguration)
   {
      _resources = resources;
      _gameConfiguration = gameConfiguration;
   }

   public IState Resolve(CreditsRequest request)
   {
      CreditsState output;

      output = new CreditsState()
      {
         CreditsListings = new List<CreditsListing>(),
         MainCreditColor = Color.White,
         SubCreditColor = Color.White,
         TextCoords = new Vector2(5, _gameConfiguration.DRAWING_GAME_HEIGHT),
         Active = false,
         TextHeight = 0f
      };


      output.Credits = _resources.GetTextList("Credits").ToArray();
      output.CreditsListings = generateCreditListings(output.Credits);

      return output;
   }

   private static List<CreditsListing> generateCreditListings(string[] credits)
   {
      List<CreditsListing> output;

      output = credits
          .Select(line =>
          {
             var elements = line
                     .Replace("\r", "")
                     .Split('|');

             return new CreditsListing(
                     name: elements[0],
                     credits: elements.Skip(1).ToList()
               );
          })
          .ToList();

      return output;
   }
}
