﻿using System;
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
        ShotPool ammoPool;

        public ShotPool AmmoPool
        {
            get { return ammoPool; }
            private set {  ammoPool = value; }
        }

        public HandGun(Game game, ShotPool shotPool) 
        {
            this.AmmoPool = shotPool;
            this.WeaponName = "HandGun";
        }

        public override void Fire(Vector2 locationOfGunHolder, Vector2 target)
        {
            ammoPool.SpawnFromPool(locationOfGunHolder, target);
        }
    }
}
