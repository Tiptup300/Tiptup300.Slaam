using Microsoft.Xna.Framework;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.x_;
using System.Collections.Generic;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;
using System.Linq;

namespace SlaamMono.Menus.Credits;

 public class CreditsRequestResolver : IResolver<CreditsRequest, IState>
 {
     private readonly IResources _resources;

     public CreditsRequestResolver(IResources resources)
     {
         _resources = resources;
     }

     public IState Resolve(CreditsRequest request)
     {
         CreditsState output;

         output = new CreditsState()
         {
             CreditsListings = new List<CreditsListing>(),
             MainCreditColor = Color.White,
             SubCreditColor = Color.White,
             TextCoords = new Vector2(5, GameGlobals.DRAWING_GAME_HEIGHT),
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
