using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;

namespace TwinStickShooter.Weapons
{
    class AssultRifle : Weapon, IRangedWeapon
    {
        float RifleDamge = 2;
        ShotPool ammoPool;

        public ShotPool AmmoPool
        {
            get { return ammoPool; }
            private set { ammoPool = value; }
        }

        public AssultRifle(Game game, ShotPool shotPool)
        {
            this.AmmoPool = shotPool;
            this.WeaponName = "Assult Rifle";
        }

        public override void Fire(Vector2 locationOfGunHolder, Vector2 target)
        {
            ammoPool.SpawnFromPool(locationOfGunHolder, target);
        }

    }
}
