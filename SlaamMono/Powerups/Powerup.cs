using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Resources;
using SlaamMono.Screens;
using SlaamMono.SubClasses;
using System;

namespace SlaamMono.Powerups
{
    public abstract class Powerup
    {
        public string Name;
        public bool Active = false;
        public bool Used = false;
        public Texture2D SmallTex, BigTex;
        public GameScreen ParentGameScreen;
        public bool AttackingType = false;
        public PowerupUse ThisPowerupsUse;
        public int AttackingRange;
        public bool AttackingInLine;

        public Powerup(string name, CachedTexture[] Textures, PowerupUse powuse)
        {
            Name = name;
            SmallTex = Textures[1].Texture;
            BigTex = Textures[0].Texture;
            ThisPowerupsUse = powuse;
        }

        /// <summary>
        /// Attack Operations before attack
        /// </summary>
        public abstract void BeginAttack(Vector2 charposition, Direction chardirection);

        /// <summary>
        /// Attack operations during attack
        /// </summary>
        public abstract void UpdateAttack();

        /// <summary>
        /// Attack operations after attack
        /// </summary>
        public abstract void EndAttack();

        /// <summary>
        /// The Drawing of the powerup.
        /// </summary>
        /// <param name="batch"></param>
        public abstract void Draw(SpriteBatch batch);
    }

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

    public enum PowerupType
    {
        None = 0,
        SpeedUp = 1,
        SpeedDown = 2,
        Inversion = 3,
        Slaam = 4,
    }

    public enum PowerupUse
    {
        Strategy,
        Attacking,
        Defensive,
        Evasion,
    }
}
