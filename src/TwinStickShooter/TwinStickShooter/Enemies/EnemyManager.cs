using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;
using MonoGameLibrary.Sprite;

namespace TwinStickShooter.Enemies
{
    class EnemyManager : DrawableGameComponent
    {       
        bool onCooldown;
        float CooldownTime = 500;
        float currentCooldown;
        string enemyPoolTag = "Enemies";
        int enemyPoolSize = 50;
        public int WaveNumber;
        public int numOfEnemiesToSpawn = 5;
        public int numOfEnemiesKilled = 10;
        float timeBetweenRounds = 2000;

        Random r;
        PoolManager poolManager;      

        public EnemyManager(Game game) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();

            //technical debt this should be in the pool class
            Queue<DrawableSprite> enemies = new Queue<DrawableSprite>();
            for (int i = 0; i < enemyPoolSize; i++)
            {
                Enemy e = new Enemy(game);
                e.Initialize();
                e.Enabled = false;
                e.Visible = false;
                enemies.Enqueue(e);
            }

            Pool pool = new Pool(game, enemies);
            poolManager.poolDictionary.Add(enemyPoolTag, pool);


            r = new Random();

            onCooldown = false;
            currentCooldown = CooldownTime;
        }

        public override void Update(GameTime gameTime)
        {
            if (numOfEnemiesKilled >= numOfEnemiesToSpawn)
            {
                if (currentCooldown < timeBetweenRounds)
                {
                    currentCooldown += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                if (currentCooldown > timeBetweenRounds)
                {
                    WaveNumber++;
                    numOfEnemiesToSpawn += 3;
                    numOfEnemiesKilled = 0;
                    for (int i = 0; i < numOfEnemiesToSpawn; i++)
                    {
                        RandomSpawn();
                    }
                    currentCooldown = 0;
                }               
            }

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
                                    numOfEnemiesKilled++;
                                    poolManager.poolDictionary["PickUps"].SpawnFromPool(enemy.Location, new Vector2(0, 0));
                                }                     
                            }
                        }
                    }
                }
            }
        }

        private void RepeatedRandomSpawn(GameTime gameTime)
        {
            if (onCooldown == false)
            {
                float randomSpawnX = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
                poolManager.poolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(randomSpawnX, 0), new Vector2(0, 1));
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

        private void RandomSpawn()
        {
            float randomSpawnX = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
            float randomSpawnY = (float)r.Next(-300, 0);
            poolManager.poolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(randomSpawnX, randomSpawnY), new Vector2(0, 1));
        }
    }
}
