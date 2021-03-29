using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.Projectiles;
using Microsoft.Xna.Framework;
using MonoGameLibrary.GameComponents;
using MonoGameLibrary.Util;

namespace TwinStickShooter.ObjectPool
{
    public class ShotPool : DrawableGameComponent
    {
        Queue<Shot> shots;

        public ShotPool(Game game, int poolSize) : base(game)
        {
            shots = new Queue<Shot>();

            for (int i = 0; i < poolSize; i++)
            {
                Shot s = new Shot(game);
                s.Initialize();
                s.Enabled = false;
                shots.Enqueue(s);
            }

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Shot s in shots)
            {
                if (s.Enabled)
                {
                    s.Update(gameTime); //Only update enabled shots
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Shot s in shots)
            {
                if (s.Enabled)
                {
                    s.Draw(gameTime); //Only update enabled shots
                }
            }

            base.Draw(gameTime);
        }

        public void SpawnFromPool(Vector2 spawnLocation, Vector2 fireDirection)
        {
            Shot s = shots.Dequeue();

            s.Enabled = true;
            s.Location = spawnLocation;
            s.Direction = fireDirection;
            s.Direction.Normalize();

            shots.Enqueue(s);
        }
    }
}
