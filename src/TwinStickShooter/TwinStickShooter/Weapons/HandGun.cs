using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.ObjectPool;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.Weapons
{
    class HandGun : RangedWeapon
    {
        float HandGunDamge = 2;
        float cooldownTime = 250;
        string poolTag;

        public HandGun(Game game, PoolManager shotPool, string poolTag) 
        {
            this.pool = shotPool;
            this.WeaponName = "HandGun";
            this.CooldownTime = cooldownTime;
            this.poolTag = poolTag;
        }

        public override void RotationFire(Vector2 spawnLocation, float playerRotation)
        {
            Vector2 target = new Vector2((float)Math.Cos(playerRotation), (float)Math.Sin(playerRotation));
            pool.SpawnFromPool(poolTag, spawnLocation, target);
        }
    }
}
