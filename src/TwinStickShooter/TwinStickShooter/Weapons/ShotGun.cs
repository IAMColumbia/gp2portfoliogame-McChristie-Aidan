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
        static int ammoPoolSize = 50;
        float bulletOffset = .05f;
        ShotPool ammoPool;

        public ShotPool AmmoPool
        {
            get { return ammoPool; }
            set { ammoPool = value; }
        }

        public ShotGun(Game game)
        {
            this.AmmoPool = new ShotPool(game, ammoPoolSize);
            game.Components.Add(AmmoPool);
        }

        public override void Fire(Vector2 locationOfGunHolder, Vector2 target)
        {
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y - (2*bulletOffset)));
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y - bulletOffset));
            ammoPool.SpawnFromPool(locationOfGunHolder, target);
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y + bulletOffset));
            ammoPool.SpawnFromPool(locationOfGunHolder, new Vector2(target.X, target.Y + (2*bulletOffset)));
        }
    }
}
