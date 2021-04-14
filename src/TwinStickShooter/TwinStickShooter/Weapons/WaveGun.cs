using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.ObjectPool;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.Weapons
{
    class WaveGun : RangedWeapon
    {
        float ShotGunDamge = 2;
        int numberOfBullets = 10;
        float bulletOffset = .09f;
        float cooldownTime = 1000;
        string poolTag;

        Random random;

        public WaveGun(Game game, Pool shotPool, string poolTag)
        {
            this.WeaponName = "ShotGun";
            this.pool = shotPool;
            this.CooldownTime = cooldownTime;
            this.poolTag = poolTag;
            random = new Random();
        }

        public override void RotationFire(Vector2 locationOfGunHolder, float playerRotation)
        {
            //needs work
            WaveSpread(locationOfGunHolder, playerRotation);
        }

        void WaveSpread(Vector2 spawnLocation, float playerRotation)
        {
            for (int i = (int)-(.5*numberOfBullets); i < (.5*numberOfBullets); i++)
            {
                Vector2 target = new Vector2((float)Math.Cos(playerRotation+(bulletOffset*i)), (float)Math.Sin(playerRotation+(bulletOffset*i)));

                pool.SpawnFromPool(spawnLocation, target);
            }
        }
    }
}
