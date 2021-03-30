using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.ObjectPool;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.Weapons
{
    class ShotGun : Weapon, IRangedWeapon
    {
        float ShotGunDamge = 2;
        int numberOfBullets = 10;
        float bulletOffset = .075f;
        ShotPool ammoPool;
        Random random;

        public ShotPool AmmoPool
        {
            get { return ammoPool; }
            set { ammoPool = value; }
        }

        public ShotGun(Game game, ShotPool shotPool)
        {
            this.AmmoPool = shotPool;
            this.WeaponName = "ShotGun";
            random = new Random();
        }

        public override void Fire(Vector2 locationOfGunHolder, Vector2 target)
        {
            //needs work
            WaveSpread(locationOfGunHolder, target);

            //needs work
            //ClusterSpread(locationOfGunHolder, target);
        }

        void WaveSpread(Vector2 locationOfGunHolder, Vector2 target)
        {
            //double rotation = -25.2;

            //float rotationCorrectedTargetX = (target.X * (float)Math.Cos(rotation)) - (target.Y * (float)Math.Sin(rotation));
            //float rotationCorrectedTargetY = (target.Y * (float)Math.Cos(rotation)) + (target.X * (float)Math.Sin(rotation));

            //ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(rotationCorrectedTargetX, rotationCorrectedTargetY - (2*bulletOffset)));
            //ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(rotationCorrectedTargetX, rotationCorrectedTargetY - bulletOffset));
            //ammoPool.SpawnFromPool(locationOfGunHolder, target);
            //ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(rotationCorrectedTargetX, rotationCorrectedTargetY + bulletOffset));
            //ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(rotationCorrectedTargetX, rotationCorrectedTargetY + (2*bulletOffset)));

            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y - (2 * bulletOffset)));
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y - bulletOffset));
            ammoPool.SpawnFromPool(locationOfGunHolder, target);
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y + bulletOffset));
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y + (2 * bulletOffset)));
        }

        void ClusterSpread(Vector2 locationOfGunHolder, Vector2 target)
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                int randomOffsetX = random.Next(-1,1);
                int randomOffsetY = random.Next(-1,1);
                ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X + randomOffsetX, target.Y + randomOffsetY));
            }
        }
    }
}
