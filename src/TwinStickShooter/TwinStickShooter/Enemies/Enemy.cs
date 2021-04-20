using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework.Graphics;

namespace TwinStickShooter.Enemies
{
    class Enemy : DrawableSprite
    {
        public Vector2 velocity;
        public Vector2 desired;
        Vector2 acceleration;
        public Vector2 target;

        public Enemy(Game game) : base(game)
        {
            this.Speed = 250;
            this.Direction = new Vector2(0, 1);
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            velocity += acceleration;
            velocity.Normalize();
            velocity *= (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
            this.Location += velocity;

            acceleration *= 0;

            //this.Location += Direction * 
            //if (this.IsOffScreen())
            //{
            //    this.Enabled = false;
            //}

            base.Update(gameTime);
        }

        public void AddForce(Vector2 force)
        {
            this.acceleration += force;
        }

        public void Seek(Vector2 target, GameTime gameTime)
        {
            desired = Vector2.Subtract(target, this.Location);
            desired.Normalize();
            desired *= (Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            var steer = Vector2.Subtract(desired, velocity);
            //limit

            AddForce(steer);
        }
    }
}
