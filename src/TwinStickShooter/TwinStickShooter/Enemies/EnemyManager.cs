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
        EnemyPool enemyPool;
        int enemyPoolSize = 50;
        bool onCooldown;
        //technical debt;
        float CooldownTime = 1500;
        Random r;

        public EnemyManager(Game game) : base(game)
        {
            r = new Random();
            onCooldown = false;
            enemyPool = new EnemyPool(game, enemyPoolSize);
            enemyPool.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            enemyPool.Update(gameTime);
            if (onCooldown == false)
            {
                float randomSpawn = (float)r.Next(0, Game.GraphicsDevice.Viewport.Width);
                enemyPool.SpawnFromPool(new Vector2(randomSpawn,0) , new Vector2(0,1));
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

        public override void Draw(GameTime gameTime)
        {
            enemyPool.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void CheckCollision()
        {
            foreach (Enemy enemy in enemyPool.enemies)
            {
            }
        }
    }
}
