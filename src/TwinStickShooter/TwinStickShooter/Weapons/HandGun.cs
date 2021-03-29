using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.ObjectPool;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.Weapons
{
    class HandGun : Weapon, IRangedWeapon
    {
        float HandGunDamge = 2;
        static int ammoPoolSize = 10;
        ShotPool ammoPool;

        public ShotPool AmmoPool
        {
            get { return ammoPool; }
            set { ammoPool = value; }
        }

        public HandGun(Game game) 
        {
            this.AmmoPool = new ShotPool(game, ammoPoolSize);
            game.Components.Add(AmmoPool);
        }

        public override void Fire(Vector2 locationOfGunHolder, Vector2 target)
        {
            ammoPool.SpawnFromPool(locationOfGunHolder, target);
        }
    }
}
