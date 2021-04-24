using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework.Graphics;
using TwinStickShooter.Weapons;

namespace TwinStickShooter.Enemies
{
    class Enemy : DrawableSprite
    {
        public enum EnemyType { Normal, Ranged, Tank }

        public EnemyType enemyType;
        private EnemyType lastEnemyType;

        //used for movement calculations 
        public Vector2 velocity;
        public Vector2 desired;
        Vector2 acceleration;
        public Vector2 playerLoc;
        public Vector2 lastLocation;

        //just a place to store the stats for our enemy types
        const float normalSpeed = 250, rangedSpeed = 200, tankSpeed = 225;
        const float normalHealth = 10, rangedHealth = 7, tankHealth = 20;
        //this should be in the enemy class
        float fireDistance = 600;

        bool onCooldown;
        float currentCooldown;

        string enemyShotPoolTag = "EnemyShots";
        Weapon gun;

        public float currentHealth; 

        public Enemy(Game game) : base(game)
        {
            this.Speed = normalSpeed;
            this.Direction = new Vector2(0, 1);
            this.enemyType = EnemyType.Normal;
            this.currentHealth = normalHealth;

            this.gun = new HandGun(game, enemyShotPoolTag);
        }

        public override void Initialize()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
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
            this.Location += velocity;

            acceleration *= 0;

            //checks to see if the enemy type has changed
            if (enemyType != lastEnemyType)
            {
                ChangeEnemyType(enemyType);
            }

            //rotates the enemy to face the player
            LookAtTarget(playerLoc);

            //shoots at the target if the enemy type is ranged
            if (Vector2.Distance(playerLoc, this.Location) < fireDistance && this.enemyType == EnemyType.Ranged)
            {
                FireGun(playerLoc, gameTime);
            }

            //this.Location += Direction * 
            //if (this.IsOffScreen())
            //{
            //    this.Enabled = false;
            //}

            base.Update(gameTime);
        }

        //used to change the stats of an enemy to that of a given type
        void ChangeEnemyType(EnemyType targetType)
        {
            switch (targetType)
            {
                case EnemyType.Normal:
                    this.Speed = normalSpeed;
                    this.currentHealth = normalHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
                    this.scale = 1;
                    break;
                case EnemyType.Ranged:
                    this.Speed = rangedSpeed;
                    this.currentHealth = rangedHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("PurpleGhost");
                    this.scale = 1;
                    break;
                case EnemyType.Tank:
                    this.Speed = tankSpeed;
                    this.currentHealth = tankHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("TealGhost");
                    this.scale = 1.5f;
                    break;
                default:
                    break;
            }
            //whatever value we passed in is the new 
            lastEnemyType = targetType;
        }

        void FireGun(Vector2 target, GameTime gameTime)
        {
            if (onCooldown == false)
            {
                onCooldown = true;
                gun.RotationFire(this.Location, this.Rotate);
            }

            if (onCooldown)
            {
                currentCooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (currentCooldown <= 0f)
                {
                    onCooldown = false;
                    //reduces the cooldown based on the cooldown modifier
                    this.currentCooldown = gun.CooldownTime;
                }
            }
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
    }
}
