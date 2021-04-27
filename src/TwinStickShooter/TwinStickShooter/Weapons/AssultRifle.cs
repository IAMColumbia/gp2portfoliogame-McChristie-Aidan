using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;

namespace TwinStickShooter.Weapons
{
    class AssultRifle : RangedWeapon
    {
        float RifleDamge = 2;
        float spreadModifier = .15f;
        float cooldownTime = 150;
        //string poolTag;

        Random r;

        public AssultRifle(Game game, string poolTag)
        {
            base.LoadPool(game, poolTag);
            this.WeaponName = "Assult Rifle";
            this.CooldownTime = cooldownTime;
            //this.poolTag = poolTag;
            r = new Random();
        }

        public override void RotationFire(Vector2 spawnLocation, float playerRotation, float speed)
        {
            float offset;

            if (r.Next(0,2) == 1)
            {
                offset = (float)r.NextDouble();
            }
            else
            {
                offset = -(float)r.NextDouble();
            }

            Vector2 target = new Vector2((float)Math.Cos(playerRotation+(offset * spreadModifier)), (float)Math.Sin(playerRotation+(offset * spreadModifier)));

            Projectiles.Shot s = (Projectiles.Shot)pool.SpawnFromPool(spawnLocation, target);
            s.Speed = speed;
        }
    }
}
