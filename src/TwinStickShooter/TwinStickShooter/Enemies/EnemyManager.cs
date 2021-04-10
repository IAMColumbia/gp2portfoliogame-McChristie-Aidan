using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;

namespace TwinStickShooter.Enemies
{
    class EnemyManager : DrawableGameComponent
    {
        PoolManager poolManager;
        int enemyPoolSize = 50;
        bool onCooldown;
        float CooldownTime = 1000;
        string enemyPoolTag = "enemies";
        Random r;

        public EnemyManager(Game game) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            r = new Random();
            onCooldown = false;
            poolManager.InitializeEnemyPool(enemyPoolTag, enemyPoolSize);
        }

        public override void Update(GameTime gameTime)
        {
            if (onCooldown == false)
            {
                float randomSpawn = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
                poolManager.SpawnFromPool(enemyPoolTag, new Vector2(randomSpawn,0) , new Vector2(0,1));
                onCooldown = true;
            }

            if (onCooldown)
            {
                CooldownTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (CooldownTime <= 0f)
                {
                    onCooldown = false;
                    CooldownTime = 1000;
                }
            }

            Console.Write(CooldownTime);
            base.Update(gameTime);
        }

        // not sure if this is needed
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        //TODO
        private void CheckCollision()
        {

        }
    }
}
