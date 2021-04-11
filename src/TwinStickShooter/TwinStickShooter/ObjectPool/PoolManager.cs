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
        public Dictionary<string, Pool> poolDictionary;
        GameConsole console;

        public PoolManager(Game game) : base(game)
        {
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            this.Game.Services.AddService(typeof(IPoolManager), this);
            poolDictionary = new Dictionary<string, Pool>();

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
    }
}
