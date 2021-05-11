using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwinStickShooter.Weapons;

namespace TwinStickShooter.Enemies
{
    class RangedEnemy : Enemy
    {
        float speed = 175;
        float health = 7;
        float rangedDamage = 2;
        float fireDistance = 400;

        bool onCooldown;
        float currentCooldown;

        string enemyShotPoolTag = "EnemyShots";
        Weapon gun;

        public RangedEnemy(Game game) : base (game)
        {
            this.Speed = speed;
            this.Health = health;
            this.damage = rangedDamage;
            this.type = "Ranged";

            gun = new HandGun(game, enemyShotPoolTag);

            this.LoadContent();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("PurpleGhost");
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            base.LoadContent();
        }

        public override void Initialize()
        {
            this.LoadContent();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //shoots at the target if the enemy type is ranged
            if (Vector2.Distance(playerLoc, this.Location) < fireDistance)
            {
                FireGun(playerLoc, gameTime);
                this.stationary = true;
            }
            else
            {
                this.stationary = false;
            }
            base.Update(gameTime);
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

    }
}
