using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;
using MonoGameLibrary.Sprite;

namespace TwinStickShooter.Pickups
{
    class PickUpManager : DrawableGameComponent
    {
        int pickUpPoolSize = 10;
        string pickUpPoolTag = "PickUps";

        PoolManager poolManager;

        public PickUpManager(Game game) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            
            //technical debt this should be in the pool class
            Queue<DrawableSprite> pickUps = new Queue<DrawableSprite>();
            for (int i = 0; i < pickUpPoolSize; i++)
            {
                PickUp p = new PickUp(game);
                p.Initialize();
                p.Enabled = false;
                pickUps.Enqueue(p);
            }

            Pool pool = new Pool(game, pickUps);
            poolManager.PoolDictionary.Add(pickUpPoolTag, pool);
        }
    }
}
