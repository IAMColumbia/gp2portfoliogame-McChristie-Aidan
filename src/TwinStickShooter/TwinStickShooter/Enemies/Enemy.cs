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
        public enum EnemyType { Normal, Ranged, Tank }

        public EnemyType enemyType;
        private EnemyType lastEnemyType;

        //used for movement calculations 
        public Vector2 velocity;
        public Vector2 desired;
        Vector2 acceleration;
        public Vector2 playerLoc;
        //unused currently
        //public Vector2 lastLocation;

        //just a place to store the stats for our enemy types
        const float normalSpeed = 200, rangedSpeed = 175, tankSpeed = 250;
        const float normalHealth = 10, rangedHealth = 7, tankHealth = 20;
        const float normalDamage = 2, rangedDamage = 2, tankDamage = 4;
        public float fireDistance = 500;

        public float Health { get; set; }
        public float damage;

        bool onCooldown;
        float currentCooldown;

        string enemyShotPoolTag = "EnemyShots";
        Weapon gun;

        public Enemy(Game game) : base(game)
        {
            this.Speed = normalSpeed;
            this.Direction = new Vector2(0, 1);
            this.enemyType = EnemyType.Normal;
            this.Health = normalHealth;
            this.damage = normalDamage;

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

            if (this.enemyType != EnemyType.Ranged || !(this.enemyType == EnemyType.Ranged && Vector2.Distance(playerLoc, this.Location) < fireDistance))
            {
                this.Location += velocity;
            }

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

        //used to change the stats of an enemy to that of a given type
        void ChangeEnemyType(EnemyType targetType)
        {
            switch (targetType)
            {
                case EnemyType.Normal:
                    this.Speed = normalSpeed;
                    this.Health = normalHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
                    this.scale = 1;
                    break;
                case EnemyType.Ranged:
                    this.Speed = rangedSpeed;
                    this.Health = rangedHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("PurpleGhost");
                    this.scale = 1;
                    break;
                case EnemyType.Tank:
                    this.Speed = tankSpeed;
                    this.Health = tankHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("TealGhost");
                    //TODO figure out why this breaks
                    this.scale = 1;
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
                gun.RotationFire(this.Location, this.Rotate, 400f);
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
