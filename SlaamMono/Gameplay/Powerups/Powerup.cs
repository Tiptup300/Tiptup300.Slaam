using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.Gameplay.Powerups
{
    public abstract class Powerup
    {
        public string Name;
        public bool Active = false;
        public bool Used = false;
        public Texture2D SmallTex, BigTex;
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
        public abstract void BeginAttack(Vector2 charposition, Direction chardirection, GameScreenState gameScreenState);

        /// <summary>
        /// Attack operations during attack
        /// </summary>
        public abstract void UpdateAttack(GameScreenState gameScreenState);

        /// <summary>
        /// Attack operations after attack
        /// </summary>
        public abstract void EndAttack(GameScreenState gameScreenState);

        /// <summary>
        /// The Drawing of the powerup.
        /// </summary>
        /// <param name="batch"></param>
        public abstract void Draw(SpriteBatch batch);
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
