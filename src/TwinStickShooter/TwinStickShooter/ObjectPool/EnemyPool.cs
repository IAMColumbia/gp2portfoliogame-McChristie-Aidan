using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.Enemies;
using Microsoft.Xna.Framework;
using MonoGameLibrary.GameComponents;
using MonoGameLibrary.Util;

namespace TwinStickShooter.ObjectPool
{
    class EnemyPool : DrawableGameComponent, IPoolManager
    {
        int numberOfActiveElements;
        public Queue<Enemy> enemies;
        GameConsole console;

        public EnemyPool(Game game, int poolSize) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            enemies = new Queue<Enemy>();

            for (int i = 0; i < poolSize; i++)
            {
                Enemy e = new Enemy(game);
                e.Initialize();
                e.Enabled = false;
                enemies.Enqueue(e);
            }

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Enemy e in enemies)
            {
                if (e.Enabled)
                {
                    ++numberOfActiveElements;
                    e.Update(gameTime); //Only update enabled shots
                }
            }

            console.Log("Number of active Enemies : ", numberOfActiveElements.ToString());
            numberOfActiveElements = 0;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Enemy e in enemies)
            {
                if (e.Enabled)
                {
                    e.Draw(gameTime); //Only update enabled shots
                }
            }

            base.Draw(gameTime);
        }

        public void SpawnFromPool(Vector2 spawnLocation, Vector2 fireDirection)
        {
            Enemy e = enemies.Dequeue();

            e.Enabled = true;
            e.Location = spawnLocation;
            e.Direction = fireDirection;
            e.Direction.Normalize();

            enemies.Enqueue(e);
        }
    }
}
