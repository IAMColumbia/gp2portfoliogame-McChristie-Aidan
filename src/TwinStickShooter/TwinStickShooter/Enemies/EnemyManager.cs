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
                e.Visible = false;
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

            //makes all of our enemies move towards the player
            foreach ( Enemy enemy in poolManager.PoolDictionary[enemyPoolTag].objectPool)
            {
                if (enemy.Enabled)
                {
                    enemy.Direction = player.Location - enemy.Location;
                    enemy.Direction.Normalize();
                }
            }

            //checks to see if our enemies hit anything
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
                                    enemy.Enabled = false;
                                    item.Enabled = false;
                                    numOfEnemiesKilled++;
                                    poolManager.PoolDictionary["PickUps"].SpawnFromPool(enemy.Location, new Vector2(0, 0));
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
    }
}
