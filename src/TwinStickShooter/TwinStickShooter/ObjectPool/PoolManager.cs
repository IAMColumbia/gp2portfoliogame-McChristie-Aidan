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
        Dictionary<string, Pool> poolDictionary;
        GameConsole console;

        public enum ClassType { Enemy, Shot, Pickup }

        public Dictionary<string, Pool> PoolDictionary
        {
            get; set;
        }

        public PoolManager(Game game) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            if ((PoolManager)game.Services.GetService(typeof(IPoolManager)) == null)
            {
                this.Game.Services.AddService(typeof(IPoolManager), this);
            }
            poolDictionary = new Dictionary<string, Pool>();
            PoolDictionary = poolDictionary;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Pool pool in poolDictionary.Values)
            {
                pool.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Pool pool in poolDictionary.Values)
            {
                pool.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public void InstantiatePool(ClassType type, Game game, int poolSize, string poolTag)
        //where T : DrawableSprite, new()
        {
            //I have know idea if this is how this would work.
            //Queue<DrawableSprite> pool = new Queue<DrawableSprite>();
            //for (int i = 0; i < poolSize; i++)
            //{
            //    var instance = Activator.CreateInstance(typeof(T), new DrawableSprite[] { game, null });
            //    pool.Enqueue(instance);
            //}
            try
            {
                if (poolDictionary[poolTag] == null)
                {
                    poolDictionary.Remove(poolTag);
                }
            }
            catch (KeyNotFoundException k)
            {
                Queue<DrawableSprite> queue = new Queue<DrawableSprite>();

                for (int i = 0; i < poolSize; i++)
                {
                    switch (type)
                    {
                        case ClassType.Enemy:
                            Enemies.Enemy e = new Enemies.Enemy(game);
                            e.Initialize();
                            e.Enabled = false;
                            queue.Enqueue(e);
                            break;
                        case ClassType.Shot:
                            Shot s = new Shot(game);
                            s.Initialize();
                            s.Enabled = false;
                            queue.Enqueue(s);
                            break;
                        case ClassType.Pickup:
                            Pickups.PickUp p = new Pickups.PickUp(game);
                            p.Initialize();
                            p.Enabled = false;
                            queue.Enqueue(p);
                            break;
                        default:
                            break;
                    }
                }

                Pool pool = new Pool(game, queue);

                poolDictionary.Add(poolTag, pool);
            }                     
        }
    }
}
