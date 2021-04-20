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
        int enemyPoolSize = 100;
        public int WaveNumber;
        public int numOfEnemiesToSpawn = 5;
        public int numOfEnemiesKilled = 10;
        float timeBetweenRounds = 2000;

        Random r;
        PoolManager poolManager;
        Player.Player player;

        public EnemyManager(Game game, Player.Player _player) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();

            //technical debt this should be in the pool class 
            Queue<DrawableSprite> enemies = new Queue<DrawableSprite>();
            for (int i = 0; i < enemyPoolSize; i++)
            {
                Enemy e = new Enemy(game);
                e.Initialize();
                e.Enabled = false;
                enemies.Enqueue(e);
            }

            Pool pool = new Pool(game, enemies);
            poolManager.PoolDictionary.Add(enemyPoolTag, pool);

            r = new Random();
            player = _player;

            onCooldown = false;
            currentCooldown = CooldownTime;

            
        }

        public override void Update(GameTime gameTime)
        {
            WaveSpawn(gameTime);
            //RepeatedRandomSpawn(gameTime);

            //makes all of our enemies move towards the player
            foreach (Enemy enemy in poolManager.PoolDictionary[enemyPoolTag].objectPool)
            {
                if (enemy.Enabled)
                {
                    enemy.target = player.Location;

                    foreach (Enemy other in poolManager.PoolDictionary[enemyPoolTag].objectPool)
                    {
                        if (other.Enabled)
                        {                            
                            var dist = Vector2.Distance(enemy.Location, other.Location);
                            var sum = new Vector2();
                            int count = 0;
                            //if (enemy.Intersects(other))
                            if (dist > 0 && dist < 50)
                            {
                                var diff = Vector2.Subtract(enemy.Location, other.Location);
                                diff = Vector2.Divide(diff, dist);
                                sum = Vector2.Add(sum, diff);
                                count++;
                            }
                            if (count > 0)
                            {
                                sum = Vector2.Divide(sum, count);
                                sum.Normalize();
                                sum *= (enemy.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
                                var steer = Vector2.Subtract(sum, enemy.velocity);
                                //limit
                                enemy.AddForce(steer);
                            }                          
                        }
                    }
                    enemy.Seek(player.Location, gameTime);
                }    
            }

            //checks to see if our enemies hit anything
            CheckCollision(gameTime);
            
            base.Update(gameTime);
        }

        // not sure if this is needed
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void CheckCollision(GameTime gameTime)
        {
            foreach (Enemy enemy in poolManager.PoolDictionary[enemyPoolTag].objectPool)
            {
                if (enemy.Enabled)
                {
                    //enemy and bullet collision
                    foreach (var item in poolManager.PoolDictionary["Shots"].objectPool)
                    {
                        if (item.Enabled)
                        {
                            if (enemy.Intersects(item))
                            {
                                if (enemy.PerPixelCollision(item))
                                {
                                    poolManager.PoolDictionary["PickUps"].SpawnFromPool(enemy.Location, new Vector2(0, 0));
                                    enemy.Location = new Vector2(-100, -100);
                                    enemy.Update(gameTime);
                                    item.Location = new Vector2(-50, -50);
                                    item.Update(gameTime);
                                    enemy.Enabled = false;
                                    item.Enabled = false;
                                    numOfEnemiesKilled++;                                 
                                }                     
                            }
                        }
                    }

                    //enemy and player collision
                    if (enemy.Enabled)
                    {
                        if (enemy.Intersects(player))
                        {
                            if (enemy.PerPixelCollision(player))
                            { 
                                enemy.Enabled = false;
                                numOfEnemiesKilled++;
                            }
                        }
                    }                   
                }
            }
        }

        //used for nonstop spawning
        private void RepeatedRandomSpawn(GameTime gameTime)
        {
            if (onCooldown == false)
            {
                float randomSpawnX = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
                poolManager.PoolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(randomSpawnX, 0), new Vector2(0, 1));

                poolManager.PoolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(100, 99), new Vector2(0, 0));
                poolManager.PoolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(300, 100), new Vector2(0, 0));
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

        //used to spawn one by one
        private void RandomSpawn()
        {
            float randomSpawnX = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
            float randomSpawnY = (float)r.Next(-300, 0);
            poolManager.PoolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(randomSpawnX, randomSpawnY), new Vector2(0, 1));
        }

        private void WaveSpawn(GameTime gameTime)
        {
            if (numOfEnemiesKilled >= numOfEnemiesToSpawn)
            {
                //makes sure we done spawn enemies if we are on cooldown
                if (currentCooldown < timeBetweenRounds)
                {
                    currentCooldown += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                //if we have waited for the cooldown time between rounds
                if (currentCooldown > timeBetweenRounds)
                {
                    WaveNumber++;
                    numOfEnemiesToSpawn += 3;

                    //makes sure we dont spawn more enemies than we have
                    if (numOfEnemiesToSpawn > enemyPoolSize)
                    {
                        numOfEnemiesToSpawn = enemyPoolSize;
                    }

                    numOfEnemiesKilled = 0;

                    //spawns an enemy for the num of enenies we need this round
                    for (int i = 0; i < numOfEnemiesToSpawn; i++)
                    {
                        RandomSpawn();
                    }
                    currentCooldown = 0;
                }
            }
        }
    }
}
