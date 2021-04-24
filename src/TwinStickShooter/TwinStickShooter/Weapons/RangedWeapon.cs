using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.ObjectPool;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.Weapons
{

    class RangedWeapon : Weapon
    {
        protected PoolManager poolManager;
        protected Pool pool;

        protected void LoadPool(Game game, string poolTag)
        {
            poolManager = (PoolManager)game.Services.GetService<IPoolManager>();
            pool = poolManager.PoolDictionary[poolTag];
        }
    }
}
