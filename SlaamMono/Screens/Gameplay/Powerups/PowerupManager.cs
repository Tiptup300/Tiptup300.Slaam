using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Resources;
using System;

namespace SlaamMono.Gameplay.Powerups
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
                    return ResourceManager.Instance.GetTexture("SpeedUp0").Texture;

                case PowerupType.SpeedDown:
                    return ResourceManager.Instance.GetTexture("SpeedDown0").Texture;

                case PowerupType.Inversion:
                    return ResourceManager.Instance.GetTexture("Inversion0").Texture;

                case PowerupType.Slaam:
                    return ResourceManager.Instance.GetTexture("Slaam0").Texture;

                default:
                    throw new Exception();

            }
        }
    }
}
