using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Resources;
using System;

namespace SlaamMono.Powerups
{
    public static class PowerupManager
    {
        private static Random rand = new Random();

        public static PowerupType GetRandomPowerup()
        {
            return (PowerupType)rand.Next(1, 5);
        }

        public static Texture2D GetPowerupTexture(PowerupType type)
        {
            switch (type)
            {
                case PowerupType.SpeedUp:
                    return ResourceManager.PU_SpeedUp[1].Texture;

                case PowerupType.SpeedDown:
                    return ResourceManager.PU_SpeedDown[1].Texture;

                case PowerupType.Inversion:
                    return ResourceManager.PU_Inversion[1].Texture;

                case PowerupType.Slaam:
                    return ResourceManager.PU_Slaam[1].Texture;

                default:
                    throw new Exception();

            }
        }
    }
}
