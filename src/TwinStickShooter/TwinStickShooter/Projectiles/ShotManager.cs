using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;

namespace TwinStickShooter.Projectiles
{
    class ShotManager : DrawableGameComponent
    {
        PoolManager poolManager;

        public ShotManager(Game game) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
        }

        public override void Update(GameTime gameTime)
        {
            //CheckCollision();

            base.Update(gameTime);
        }

        private void CheckCollision()
        {
            foreach (Shot shot in poolManager.PoolDictionary["Shots"].objectPool)
            {
                if (shot.Enabled)
                {
                    foreach (var item in poolManager.PoolDictionary["Enemies"].objectPool)
                    {
                        
                    }
                }
            }
        }
    }
}
