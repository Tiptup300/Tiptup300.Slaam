using Microsoft.Xna.Framework.Graphics;
using SlaamMono.ResourceManagement;
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
                    return Resources.Instance.GetTexture("SpeedUp0").Texture;

                case PowerupType.SpeedDown:
                    return Resources.Instance.GetTexture("SpeedDown0").Texture;

                case PowerupType.Inversion:
                    return Resources.Instance.GetTexture("Inversion0").Texture;

                case PowerupType.Slaam:
                    return Resources.Instance.GetTexture("Slaam0").Texture;

                default:
                    throw new Exception();

            }
        }
    }
}
