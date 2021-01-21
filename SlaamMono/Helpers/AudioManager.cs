using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;


namespace SlaamMono
{
    /// <summary>
    /// Helper class that manages all things dealing with audio.
    /// </summary>
    public class AudioManager : GameComponent
    {
        public AudioManager(SlaamGame game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Play
        /// </summary>
        /// <param name="soundName">Sound name</param>
        public static void Play(string soundName)
        {
            //soundbank.PlayCue(soundName);
        }

        /// <summary>
        /// Play
        /// </summary>
        /// <param name="sound">Sound</param>
        public static void Play(SFX sound)
        {
            Play(sound.ToString());
        }

        #region Enums

        /// <summary>
        /// Sounds and music that are used.
        /// </summary>
        /// <returns></returns>
        public enum SFX
        {
            // Menu SFX
            MenuMoveSwish,
            MenuClicked,
            // Game SFX
            Attacked,
            Died,
            // Music
            MenuMusic,
            CreditsMusic,
            GameScreenMusic,

        }
        #endregion
    }
}
