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
        int enemyPoolSize = 50;
        bool onCooldown;
        float CooldownTime = 500;
        float currentCooldown;
        string enemyPoolTag = "Enemies";

        Random r;
        PoolManager poolManager;
        

        public EnemyManager(Game game) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            //TODO Initilize pool

            r = new Random();

            onCooldown = false;
            currentCooldown = CooldownTime;
        }

        public override void Update(GameTime gameTime)
        {
            RandomSpawn(gameTime);

            CheckCollision();
            
            base.Update(gameTime);
        }

        // not sure if this is needed
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void CheckCollision()
        {
            foreach (Enemy enemy in poolManager.poolDictionary[enemyPoolTag].objectPool)
            {
                if (enemy.Enabled)
                {
                    //enemy and bullet collision
                    foreach (var item in poolManager.poolDictionary["Shots"].objectPool)
                    {
                        if (item.Enabled)
                        {
                            if (enemy.Intersects(item))
                            {
                                if (enemy.PerPixelCollision(item))
                                {
                                    enemy.Enabled = false;
                                    item.Enabled = false;
                                }                     
                            }
                        }
                    }
                }
            }
        }

        private void RandomSpawn(GameTime gameTime)
        {
            if (onCooldown == false)
            {
                float randomSpawn = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
                poolManager.poolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(randomSpawn, 0), new Vector2(0, 1));
                onCooldown = true;
            }

            if (onCooldown)
            {
                currentCooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (currentCooldown <= 0f)
                {
                    onCooldown = false;
                    currentCooldown = CooldownTime;
                }
            }
        }
    }
}
