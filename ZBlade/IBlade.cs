using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    public abstract class IBlade
    {
        private int height = 30;
        public int Height
        {
            get { return height; }
            set
            {
                if (value > 100 || value < 15)
                    throw new Exception("Invalid Blade Height");

                height = value;
            }
        }

        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }


        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch batch, Vector2 Position);
    }

    public class InfoBlade : IBlade
    {
        
        /// <summary>
        /// Gets or sets the display data for when the blade is out.
        /// </summary>
        public static BladeSetup BladeOutSetup { get; set; }

        /// <summary>
        /// Gets or sets the display data for when the blade is in.
        /// </summary>
        public static BladeSetup BladeInSetup { get; set; }

        /// <summary>
        /// Gets or sets the display data for when the blade is hidden.
        /// </summary>
        public static BladeSetup BladeHiddenSetup { get; set; }

        /// <summary>
        /// Gets or sets the display data for when the keyboard is displayed.
        /// </summary>
        public static BladeSetup BladeKeyOutSetup { get; set; }

        public InfoBlade()
        {
            Height = 30;
        }

        private BladeSetup CurrentBladeSetup
        {
            get
            {
                if (ZuneBlade.instance.Status == BladeStatus.In)
                    return BladeInSetup;
                else if (ZuneBlade.instance.Status == BladeStatus.Out)
                    return BladeOutSetup;
                else if (ZuneBlade.instance.Status == BladeStatus.Hidden)
                    return BladeHiddenSetup;
                else if (ZuneBlade.instance.Status == BladeStatus.KeyOut)
                    return BladeKeyOutSetup;

                return null;
            }
        }

        public override void Draw(SpriteBatch batch, Vector2 Position)
        {
            Helpers.DrawString(
                 batch,
                 ZuneBlade.Font12,
                 CurrentBladeSetup.BackButtonText,
                 Position + new Vector2(10, (int)(Height / 2)),
                 new Vector2(
                     0,
                     ZuneBlade.Font12.MeasureString(CurrentBladeSetup.BackButtonText).Y / 2f));

            Helpers.DrawString(
                batch,
                ZuneBlade.Font12,
                CurrentBladeSetup.PlayButtonText,
                Position + new Vector2(Width-10, (int)(Height / 2)),
                new Vector2(
                    ZuneBlade.Font12.MeasureString(CurrentBladeSetup.PlayButtonText).X,
                    ZuneBlade.Font12.MeasureString(CurrentBladeSetup.PlayButtonText).Y / 2f));

            Helpers.DrawString(
                batch,
                ZuneBlade.Font14,
                CurrentBladeSetup.MiddleButtonText,
                Position + new Vector2(Width / 2, (int)(Height / 2 - 2)),
                ZuneBlade.Font12.MeasureString(CurrentBladeSetup.MiddleButtonText) / 2f);
        }

        public override void Update(GameTime gameTime)
        {

        }

    }
}
