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
        float CooldownTime { get; }

        void Fire(Vector2 spawnLocation, Vector2 target);
        void RotationFire(Vector2 spawnLocation, float playerRotation , float shotSpeed);
    }

    class Weapon : IWeapon
    {
        public string WeaponName { get; protected set; }
        public float Damage { get; protected set; }
        public float CooldownTime { get; protected set; }

        public virtual void Fire(Vector2 spawnLocation, Vector2 target)
        {
            //TODO add Shot logic
        }

        public virtual void RotationFire(Vector2 spawnLocation, float playerRotation, float shotSpeed)
        {

        }
    }
}
