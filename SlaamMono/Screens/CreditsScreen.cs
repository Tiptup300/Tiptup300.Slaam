using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Input;
using SlaamMono.Resources;
using System.Collections.Generic;

namespace SlaamMono.Screens
{
    public class CreditsScreen : IScreen
    {
        private const float MovementSpeed = 3f / 120f;
        private readonly MainMenuScreen _menuScreen;
        private readonly IScreenDirector _screenDirector;
        private string[] credits;
        //private int CurrentCredit = 3;
        private List<CreditsListing> CreditsListings = new List<CreditsListing>();

        private Color MainCreditColor = Color.White;
        private Color SubCreditColor = Color.White;
        private Vector2 TextCoords = new Vector2(5, GameGlobals.DRAWING_GAME_HEIGHT);
        private bool Active = false;
        private float TextHeight = 0f;

        public CreditsScreen(MainMenuScreen menuScreen, IScreenDirector screenDirector)
        {
            _menuScreen = menuScreen;
            _screenDirector = screenDirector;
        }

        public void Open()
        {
            credits = ResourceManager.Credits.ToArray();
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Credits);
            for (int x = 0; x < credits.Length; x++)
            {
                string[] credinfo = credits[x].Replace("\r", "").Split("|".ToCharArray());
                string credname = credinfo[0];
                List<string> credcreds = new List<string>();
                for (int y = 1; y < credinfo.Length; y++)
                {
                    credcreds.Add(credinfo[y]);
                }
                CreditsListings.Add(new CreditsListing(credname, credcreds));
            }
            FeedManager.FeedsActive = false;
        }
        public void Update()
        {
            if (InputComponent.Players[0].PressedAction)
            {
                Active = !Active;
            }

            if (Active)
            {
                TextCoords.Y -= MovementSpeed * FrameRateDirector.MovementFactor;
            }

            if (TextCoords.Y < -TextHeight - 50)
            {
                TextCoords.Y = GameGlobals.DRAWING_GAME_HEIGHT;
            }

            if (InputComponent.Players[0].PressedAction2)
            {
                _screenDirector.ChangeTo(_menuScreen);
            }
        }

        public void Draw(SpriteBatch batch)
        {
#if !ZUNE
            batch.Draw(Resources.CreditsBG.Texture, new Vector2(1200 - Resources.CreditsBG.Width, 512 - Resources.CreditsBG.Height /2), Color.White);
#endif 
            float Offset = 0;

            for (int CurrentCredit = 0; CurrentCredit < CreditsListings.Count; CurrentCredit++)
            {
                if (TextCoords.Y + Offset > 0 && TextCoords.Y + Offset + 20 < GameGlobals.DRAWING_GAME_HEIGHT + ResourceManager.GetFont("SegoeUIx32pt").MeasureString(CreditsListings[CurrentCredit].Name).Y)
                {
                    TextManager.Instance.AddTextToRender(CreditsListings[CurrentCredit].Name, new Vector2(TextCoords.X, TextCoords.Y + Offset), ResourceManager.GetFont("SegoeUIx32pt"), MainCreditColor, TextAlignment.Default, false);
                }
                Offset += ResourceManager.GetFont("SegoeUIx32pt").MeasureString(CreditsListings[CurrentCredit].Name).Y / 1.5f;
                for (int x = 0; x < CreditsListings[CurrentCredit].Credits.Count; x++)
                {
                    if (TextCoords.Y + Offset > 0 && TextCoords.Y + Offset + 10 < GameGlobals.DRAWING_GAME_HEIGHT + ResourceManager.GetFont("SegoeUIx14pt").MeasureString(CreditsListings[CurrentCredit].Credits[x]).Y)
                    {
                        TextManager.Instance.AddTextToRender(CreditsListings[CurrentCredit].Credits[x], new Vector2(TextCoords.X + 10, TextCoords.Y + Offset), ResourceManager.GetFont("SegoeUIx14pt"), SubCreditColor, TextAlignment.Default, false);
                    }
                    Offset += (int)ResourceManager.GetFont("SegoeUIx14pt").MeasureString(CreditsListings[CurrentCredit].Credits[x]).Y;
                }
                Offset += 20;
            }

            TextHeight = Offset;
        }

        public void Close()
        {

        }

        private struct CreditsListing
        {
            public string Name;
            public List<string> Credits;
            public CreditsListing(string name, List<string> credits)
            {
                Name = name;
                Credits = credits;
            }
        }
    }
}
