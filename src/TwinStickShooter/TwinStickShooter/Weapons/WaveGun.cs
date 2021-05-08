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
        float WaveGunDamage = 1;
        int numberOfBullets = 10;
        float bulletOffset = .09f;
        float cooldownTime = 1000;
        //string poolTag;

        Random random;

        public WaveGun(Game game, string poolTag)
        {
            base.LoadPool(game, poolTag);
            this.WeaponName = "WaveGun";
            this.CooldownTime = cooldownTime;
            //this.poolTag = poolTag;
            random = new Random();
        }

        public override void RotationFire(Vector2 locationOfGunHolder, float playerRotation, float speed)
        {
            WaveSpread(locationOfGunHolder, playerRotation, speed);
        }

        void WaveSpread(Vector2 spawnLocation, float playerRotation, float speed)
        {
            for (int i = (int)-(.5*numberOfBullets); i < (.5*numberOfBullets); i++)
            {
                Vector2 target = new Vector2((float)Math.Cos(playerRotation+(bulletOffset*i)), (float)Math.Sin(playerRotation+(bulletOffset*i)));

                Projectiles.Shot s = (Projectiles.Shot)pool.SpawnFromPool(spawnLocation, target);
                s.Speed = speed;
                s.damage = WaveGunDamage;
            }
        }
    }
}
