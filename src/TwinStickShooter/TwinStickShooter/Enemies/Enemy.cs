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
        public enum EnemyType { Normal, Ranged, Tank }

        public EnemyType enemyType;
        private EnemyType lastEnemyType;

        //used for movement calculations 
        public Vector2 velocity;
        public Vector2 desired;
        Vector2 acceleration;
        public Vector2 target;

        //just a place to store the stats for our enemy types
        public const float normalSpeed = 250, rangedSpeed = 200, tankSpeed = 225;
        public const float normalHealth = 10, rangedHealth = 7, tankHealth = 20;

        public float currentHealth; 

        public Enemy(Game game) : base(game)
        {
            this.Speed = normalHealth;
            this.Direction = new Vector2(0, 1);
            this.enemyType = EnemyType.Normal;
            this.currentHealth = normalHealth;
        }

        public override void Initialize()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            velocity += acceleration;
            velocity.Normalize();
            velocity *= (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
            this.Location += velocity;

            acceleration *= 0;

            if (enemyType != lastEnemyType)
            {
                ChangeEnemyType(enemyType);
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
                    break;
                case EnemyType.Ranged:
                    this.Speed = rangedHealth;
                    this.currentHealth = rangedHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("PurpleGhost");
                    break;
                case EnemyType.Tank:
                    this.Speed = tankSpeed;
                    this.currentHealth = tankHealth;
                    this.spriteTexture = this.Game.Content.Load<Texture2D>("TealGhost");
                    break;
                default:
                    break;
            }
            lastEnemyType = targetType;
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
