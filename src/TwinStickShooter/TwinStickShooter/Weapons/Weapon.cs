using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;

namespace TwinStickShooter.Weapons
{
    public interface IWeapon
    {
        string WeaponName { get; }
        float Damage { get; }
        float FireRate { get; }

        void Fire(Vector2 spawnLocation, Vector2 target);
    }

    public interface IRangedWeapon
    {
        ShotPool AmmoPool { get; }
    }

    class Weapon : IWeapon
    {
        public string WeaponName { get; protected set; }
        public float Damage { get; private set; }
        public float FireRate { get; private set; }

        public virtual void Fire(Vector2 spawnLocation, Vector2 target)
        {
            //TODO add Shot logic
        }

    }
}
