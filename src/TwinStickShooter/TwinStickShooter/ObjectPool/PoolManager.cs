using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using TwinStickShooter.Projectiles;

namespace TwinStickShooter.ObjectPool
{
    class PoolManager : DrawableGameComponent, IPoolManager
    {
        int numberOfActiveElements;
        public Dictionary<string, Queue<DrawableSprite>> poolDictionary;
        GameConsole console;

        public PoolManager(Game game) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            this.Game.Services.AddService(typeof(IPoolManager), this);
            poolDictionary = new Dictionary<string, Queue<DrawableSprite>>();


        }

        public override void Update(GameTime gameTime)
        {
            foreach (Queue<DrawableSprite> queue in poolDictionary.Values)
            {
                foreach (DrawableSprite s in queue)
                {
                    if (s.Enabled)
                    {
                        s.Update(gameTime); //Only update enabled sprites
                    }
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Queue<DrawableSprite> queue in poolDictionary.Values)
            {
                foreach (DrawableSprite s in queue)
                {
                    if (s.Enabled)
                    {
                        s.Draw(gameTime); //Only update enabled shots
                    }
                }
            }

            base.Draw(gameTime);
        }

        public void SpawnFromPool(string tag, Vector2 spawnLocation, Vector2 fireDirection)
        {
            DrawableSprite s = poolDictionary[tag].Dequeue();

            s.Enabled = true;
            s.Location = spawnLocation;
            s.Direction = fireDirection;
            s.Direction.Normalize();

            poolDictionary[tag].Enqueue(s);
        }

        //technical debt i cant figure out how to instantiate the the queue without know ing exactly whats going it it first
        //need to change this later
        public void InitializeShotPool(string name, int size)
        {
            Queue<DrawableSprite> shots = new Queue<DrawableSprite>();

            for (int i = 0; i < size; i++)
            {
                Shot s = new Shot(this.Game);
                s.Initialize();
                s.Enabled = false;
                shots.Enqueue(s);
            }

            poolDictionary.Add(name, shots);
        }

        public void InitializeEnemyPool(string name, int size)
        {
            Queue<DrawableSprite> enemies = new Queue<DrawableSprite>();

            for (int i = 0; i < size; i++)
            {
                Enemies.Enemy e = new Enemies.Enemy(this.Game);
                e.Initialize();
                e.Enabled = false;
                enemies.Enqueue(e);
            }

            poolDictionary.Add(name, enemies);
        }
    }
}
