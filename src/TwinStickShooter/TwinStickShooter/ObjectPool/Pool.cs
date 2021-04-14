using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.ObjectPool
{
    public class Pool : DrawableGameComponent
    {
        public Queue<DrawableSprite> objectPool;

        public Pool(Game game, Queue<DrawableSprite> pool) : base(game)
        {
            objectPool = pool;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (DrawableSprite sprite in objectPool)
            {
                if (sprite.Enabled)
                {
                    sprite.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (DrawableSprite sprite in objectPool)
            {
                if (sprite.Enabled)
                {
                    sprite.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        //enables the first item in that queue at the target location moving the target direction
        public void SpawnFromPool(Vector2 spawnLocation, Vector2 fireDirection)
        {
            DrawableSprite s = objectPool.Dequeue();

            s.Location = spawnLocation;
            s.Direction = fireDirection;
            s.Direction.Normalize();
            s.Enabled = true;

            objectPool.Enqueue(s);
        }
    }
}
