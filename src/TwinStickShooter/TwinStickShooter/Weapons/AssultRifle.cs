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
        float cooldownTime = 100;
        Random r;

        public AssultRifle(Game game, PoolManager shotPool)
        {
            this.pool = shotPool;
            this.WeaponName = "Assult Rifle";
            this.CooldownTime = cooldownTime;
            r = new Random();
        }

        public override void RotationFire(Vector2 spawnLocation, float playerRotation)
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

            Vector2 target = new Vector2((float)Math.Cos(playerRotation+(offset* spreadModifier)), (float)Math.Sin(playerRotation+(offset* spreadModifier)));

            pool.SpawnFromPool("Shots", spawnLocation, target);
        }
    }
}
