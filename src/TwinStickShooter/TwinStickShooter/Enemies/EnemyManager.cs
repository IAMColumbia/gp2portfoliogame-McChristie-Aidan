using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.ObjectPool;
using TwinStickShooter.Projectiles;
using TwinStickShooter.HUD;
using MonoGameLibrary.Sprite;

namespace TwinStickShooter.Enemies
{
    class EnemyManager : DrawableGameComponent
    {
        //wave spawn variables
        bool onCooldown;
        float CooldownTime = 500;
        float currentCooldown;
        float timeBetweenRounds = 2000;
        public int WaveNumber;
        public int numOfEnemiesToSpawn = 5;
        public int numOfEnemiesKilled = 10;
        Random r;

        //enemy pool variables
        string enemyPoolTag = "Enemies";
        int enemyPoolSize = 100;

        string enemyShotPoolTag = "EnemyShots";
        int enemyShotPoolSize = 400;

        PoolManager poolManager;
        ScoreManager scoreManager;

        //the player of our game
        Player.PlayerWGun player;

        public EnemyManager(Game game, Player.PlayerWGun _player) : base(game)
        {
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            scoreManager = (ScoreManager)this.Game.Services.GetService<IScoreManager>();

            poolManager.InstantiatePool(PoolManager.ClassType.Shot, game, enemyShotPoolSize, enemyShotPoolTag);
            poolManager.InstantiatePool(PoolManager.ClassType.Enemy, game, enemyPoolSize, enemyPoolTag);           

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
            EnemySeek(gameTime);

            //checks to see if our enemies hit anything
            CheckCollision(gameTime);

            scoreManager.waveNumber = WaveNumber;

            base.Update(gameTime);
        }

        // not sure if this is needed
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        //checks to see if the enemy collides with various things
        private void CheckCollision(GameTime gameTime)
        {
            foreach (Enemy enemy in poolManager.PoolDictionary[enemyPoolTag].objectPool)
            {
                if (enemy.Enabled)
                {
                    //enemy and bullet collision
                    foreach (Shot shot in poolManager.PoolDictionary["Shots"].objectPool)
                    {
                        if (shot.Enabled)
                        {
                            if (enemy.Intersects(shot))
                            {
                                if (enemy.PerPixelCollision(shot))
                                {
                                    //enemy takes damage and destroys the shot
                                    enemy.TakeDamage(shot.damage);
                                    shot.Dies(gameTime);

                                    if (enemy.Health < 0)
                                    {
                                        Pickups.PickUp p = (Pickups.PickUp)poolManager.PoolDictionary["PickUps"].SpawnFromPool(enemy.Location, new Vector2(0, 0));
                                        p.type = (Pickups.PickUp.PickUpType)r.Next(0, Enum.GetNames(typeof(Pickups.PickUp.PickUpType)).Length);
                                        enemy.Dies(gameTime);
                                        scoreManager.score += 10;
                                    }

                                    numOfEnemiesKilled++;
                                }
                            }
                        }
                    }

                    //enemy and player collision
                    if (enemy.Intersects(player))
                    {
                        if (enemy.PerPixelCollision(player))
                        {
                            player.TakeDamage(enemy.damage);
                            enemy.Dies(gameTime);
                            numOfEnemiesKilled++;
                        }
                    }


                    //enemy and enemy collision
                    foreach (Enemy other in poolManager.PoolDictionary[enemyPoolTag].objectPool)
                    {
                        if (other.Enabled && other.type != "Ranged" || other.type == enemy.type)
                        {
                            //code barrowed form my old sim and serious homeworks. makes the enemies bounce off of one another
                            var dist = Vector2.Distance(enemy.Location, other.Location);
                            var sum = new Vector2();
                            int count = 0;
                            //if (enemy.Intersects(other))
                            if (dist > 0 && dist < other.spriteTexture.Width)
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

                                //freezes enemy in place but its better looking than the vibrations i get now
                                //enemy.Location = enemy.lastLocation;

                                enemy.AddForce(steer);
                            }
                        }
                    }
                }
            }
        }

        int NumOfActiveEnemies()
        {
            int numOfActiveEnemies = 0;
            foreach (Enemy e in poolManager.PoolDictionary[enemyPoolTag].objectPool)
            {
                if (e.Enabled)
                {
                    numOfActiveEnemies++;
                }
            }           
            return numOfActiveEnemies;
        }

        //tells the enemies what target to seek
        private void EnemySeek(GameTime gameTime)
        {
            foreach (Enemy enemy in poolManager.PoolDictionary[enemyPoolTag].objectPool)
            {
                if (enemy.Enabled)
                {                    
                    enemy.playerLoc = player.Location;
                    enemy.Seek(player.Location, gameTime);
                }
            }
        }

        //used for nonstop spawning
        private void RepeatedRandomSpawn(GameTime gameTime)
        {
            if (onCooldown == false)
            {
                RandomSpawn();

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

        //spawning waves
        private void WaveSpawn(GameTime gameTime)
        {
            if (NumOfActiveEnemies() <= 0)
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
                        float randomSpawnX = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
                        float randomSpawnY = (float)r.Next(-300, 0);
                        Vector2 spawnLoc = new Vector2(randomSpawnX, randomSpawnY);
                        //RandomSpawn();
                        int typeNum = r.Next(1, 4);
                        
                        //This is a huge hot mess.

                        switch (typeNum)
                        {
                            case 1:
                                //Enemy enemy = (Enemy)poolManager.PoolDictionary[enemyPoolTag].SpawnFromPool(spawnLoc, new Vector2(0, 0));
                                //enemy = new RangedEnemy(this.Game);
                                //enemy.Initialize();
                                DrawableSprite s = (Enemy)poolManager.PoolDictionary[enemyPoolTag].objectPool.Dequeue();

                                s.Direction = Vector2.Zero;
                                s.Direction.Normalize();
                                s.Enabled = true;
                                s = new BasicEnemy(this.Game);
                                s.Location = spawnLoc;

                                poolManager.PoolDictionary[enemyPoolTag].objectPool.Enqueue(s);

                                break;
                            case 2:
                                DrawableSprite t = (Enemy)poolManager.PoolDictionary[enemyPoolTag].objectPool.Dequeue();

                                t.Direction = Vector2.Zero;
                                t.Direction.Normalize();
                                t.Enabled = true;
                                t = new RangedEnemy(this.Game);
                                t.Location = spawnLoc;

                                poolManager.PoolDictionary[enemyPoolTag].objectPool.Enqueue(t);
                                break;
                            case 3:
                                DrawableSprite e = poolManager.PoolDictionary[enemyPoolTag].objectPool.Dequeue();

                                e.Direction = Vector2.Zero;
                                e.Direction.Normalize();
                                e.Enabled = true;
                                e = new TankEnemy(this.Game);
                                e.Location = spawnLoc;

                                poolManager.PoolDictionary[enemyPoolTag].objectPool.Enqueue(e);
                                break;
                            default:                               
                                break;
                        }

                        //switch (typeNum)
                        //{
                        //    case 1:
                        //        //Enemy e = (Enemy)poolManager.PoolDictionary[enemyPoolTag].SpawnFromPool(new Vector2(randomSpawnX, randomSpawnY), new Vector2(0, 0));
                        //        break;
                        //    case 2:
                        //        RangedEnemy re = (RangedEnemy)poolManager.PoolDictionary["Ranged"].SpawnFromPool(new Vector2(randomSpawnX, randomSpawnY), new Vector2(0, 0));
                        //        break;
                        //    case 3:
                        //        TankEnemy te = (TankEnemy)poolManager.PoolDictionary[tankPoolTag].SpawnFromPool(new Vector2(randomSpawnX, randomSpawnY), new Vector2(0, 0));
                        //        break;
                        //    default:
                        //        break;
                        //}

                    }
                    currentCooldown = 0;
                }
            }
        }
       
        public void Reset()
        {
            numOfEnemiesToSpawn = 5;
            numOfEnemiesKilled = 10;
            WaveNumber = 0;
        }
    }
}
