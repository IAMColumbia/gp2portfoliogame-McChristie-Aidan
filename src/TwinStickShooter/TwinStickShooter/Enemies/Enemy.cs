using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework.Graphics;
using TwinStickShooter.Weapons;
using TwinStickShooter.EntitiyTraits;

namespace TwinStickShooter.Enemies
{
    class Enemy : DrawableSprite, IDamagable
    {      
        //used for movement calculations 
        public Vector2 velocity;
        public Vector2 desired;
        Vector2 acceleration;
        public Vector2 playerLoc;
        public string type;
        //unused currently
        //public Vector2 lastLocation;

        protected bool stationary;

        //just a place to store the stats for our enemy types
        

        public float Health { get; set; }
        public float damage;

        public Enemy(Game game) : base(game)
        {
        }

        public override void Initialize()
        {

            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();          
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            //lastLocation = this.Location;
            velocity += acceleration;
            velocity.Normalize();
            velocity *= (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            if(!stationary)
            {
                this.Location += velocity;
            }

            acceleration *= 0;

            //rotates the enemy to face the player
            LookAtTarget(playerLoc);          

            //this feels like it should be here but caused problems
            //if (this.Health < 0)
            //{
            //    this.Dies(gameTime);
            //}

            //if (this.IsOffScreen())
            //{
            //    this.Enabled = false;
            //}

            base.Update(gameTime);
        }

        //implements the 'TakeDamage' function
        public void TakeDamage(float damageAmmount)
        {
            this.Health -= damageAmmount;
        }

        void LookAtTarget(Vector2 target)
        {
            Vector2 distance;

            distance.X = target.X - this.Location.X;
            distance.Y = target.Y - this.Location.Y;

            this.Rotate = (float)Math.Atan2(distance.Y, distance.X);
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

        public void Dies(GameTime gameTime)
        {
            //this should be a variable
            this.Location = new Vector2(-100, -100);
            this.Update(gameTime);
            this.Enabled = false;
        }
    }
}
