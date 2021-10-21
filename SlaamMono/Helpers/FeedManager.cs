using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Helpers
{
    /// <summary>
    /// Manages the feed you see at the bottom of menus within the game.
    /// </summary>
    static class FeedManager
    {
        

        public const float BarMovement = 15f / 10f;
        public const float TextMovement = 3f / 20f;

        public static bool FeedsActive = false;

        private static Rectangle FeedRect = new Rectangle(0, 975, 0, 54);

        private static float TextX = 1350f;

        private static string FeedText;

        

        

        public static void InitializeFeeds(string str)
        {
            FeedsActive = true;
            FeedText = str;
            TextX = 1350;
            FeedRect.Width = 0;
        }

        

        

        public static void Update()
        {
            if (FeedsActive)
            {
                if (FeedRect.Width >= 1280)
                {
                    TextX -= FrameRateDirector.MovementFactor * TextMovement;
                    if (TextX <= FeedText.Length * -8)
                        TextX = 1350;
                }
                else
                {
                    FeedRect.Width += (int)(FrameRateDirector.MovementFactor * BarMovement);
                }
            }
        }

        

        

        public static void Draw(SpriteBatch batch)
        {
#if !ZUNE
            if (FeedsActive)
            {
                batch.Draw(Resources.Feedbar.Texture, FeedRect, Color.White);
                Resources.DrawString(FeedText, new Vector2(TextX, FeedRect.Y + 32), Resources.SegoeUIx14pt, TextAlignment.Default, Color.White,true);
            }
#endif
        }

        
    }
}
