using Microsoft.Xna.Framework;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.x_;
using System.Collections.Generic;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus.Credits
{
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
            processCreditListings(output);

            return output;
        }

        private static void processCreditListings(CreditsState output)
        {
            for (int x = 0; x < output.Credits.Length; x++)
            {
                string[] credinfo = output.Credits[x].Replace("\r", "").Split("|".ToCharArray());
                string credname = credinfo[0];
                List<string> credcreds = new List<string>();
                for (int y = 1; y < credinfo.Length; y++)
                {
                    credcreds.Add(credinfo[y]);
                }
                output.CreditsListings.Add(new CreditsListing(credname, credcreds));
            }
        }
    }
}
