using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinStickShooter.Weapons;
using TwinStickShooter.Projectiles;
using TwinStickShooter.ObjectPool;
using TwinStickShooter.EntitiyTraits;
using MonoGameLibrary.Util;

namespace TwinStickShooter.Player
{
    class PlayerWGun : Player, IDamagable
    {
        bool onCooldown;
        float cooldownTime = 1000;

        public float CoolDown { get { return cooldownTime; } }

        float playerCooldownModifier = 0;
        string shotPoolTag = "Shots";
        //technical debt the player shouldn't use this
        int shotPoolSize = 100;

        public float Health { get; set; }

        public Weapon gun;

        Random r;
        GameConsole console;
        HUD.ScoreManager score;
        PoolManager poolManager;

        public PlayerWGun(Game game) : base(game)
        {           
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            poolManager.InstantiatePool(PoolManager.ClassType.Shot, game, shotPoolSize, shotPoolTag);
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
            score = (HUD.ScoreManager)this.Game.Services.GetService<HUD.IScoreManager>();

            r = new Random();

            Reset(game);
        }

        public override void Update(GameTime gameTime)
        {
            //has the player fired
            CheckForFire(gameTime);
            //have we swapped guns
            GunSwap();
            //have we hit something
            CheckCollision(gameTime);
            //useful info
            console.Log("Player Health : ", this.Health.ToString());
            console.Log("player current gun : ", this.gun.WeaponName);
            console.Log("player cooldown: ", cooldownTime.ToString());

            base.Update(gameTime);
        }

        //used for firing logic
        private void CheckForFire(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && onCooldown == false)
            {
                onCooldown = true;
                gun.RotationFire(this.Location, this.Rotate, 800);
            }

            if (onCooldown)
            {
                cooldownTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                console.Log("Gun cooldown : ", cooldownTime.ToString());
          
                if (cooldownTime <= 0f)
                {
                    onCooldown = false;
                    //reduces the cooldown based on the cooldown modifier
                    this.cooldownTime = gun.CooldownTime - (gun.CooldownTime * (playerCooldownModifier / 100));
                }
            }
        }

        //tool for swaping guns for testing
        void GunSwap()
        {
            if(Controller.Input.KeyboardState.HasReleasedKey(Keys.D1))
            {
                this.gun = new HandGun(this.Game, shotPoolTag);
                this.cooldownTime = gun.CooldownTime - (gun.CooldownTime * (playerCooldownModifier / 100));
            }

            if (Controller.Input.KeyboardState.HasReleasedKey(Keys.D2))
            {
                this.gun = new WaveGun(this.Game, shotPoolTag);
                this.cooldownTime = gun.CooldownTime - (gun.CooldownTime * (playerCooldownModifier / 100));
            }

            if (Controller.Input.KeyboardState.HasReleasedKey(Keys.D3))
            {
                this.gun = new AssultRifle(this.Game, shotPoolTag);
                this.cooldownTime = gun.CooldownTime - (gun.CooldownTime * (playerCooldownModifier / 100));
            }
        }

        //implements the 'TakeDamage' function
        public void TakeDamage(float damageAmmount)
        {
            this.Health -= damageAmmount;
        }

        //checks for when the player hits something
        void CheckCollision(GameTime gameTime)
        {
            //player and pick up collision
            foreach (Pickups.PickUp pickUp in poolManager.PoolDictionary["PickUps"].objectPool)
            {
                if (pickUp.Enabled)
                {
                    if (pickUp.Intersects(this))
                    {
                        if (pickUp.PerPixelCollision(this))
                        {
                            //pickUp.Location = new Vector2(-100, -50);
                            pickUp.Enabled = false;
                            
                            switch (pickUp.type)
                            {
                                case Pickups.PickUp.PickUpType.AttackSpeed:
                                    playerCooldownModifier += pickUp.pickUpValue;
                                    break;
                                case Pickups.PickUp.PickUpType.Health:
                                    this.Health += pickUp.pickUpValue;
                                    break;
                                case Pickups.PickUp.PickUpType.Weapon:
                                    int num = r.Next(1, 4);
                                    score.score += 5;
                                    switch (num)
                                    {
                                        case 1:
                                            gun = new HandGun(this.Game, shotPoolTag);
                                            break;
                                        case 2:
                                            gun = new AssultRifle(this.Game, shotPoolTag);
                                            break;
                                        case 3:
                                            gun = new WaveGun(this.Game, shotPoolTag);
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (!onCooldown)
                            {
                                this.cooldownTime = gun.CooldownTime - (gun.CooldownTime * (playerCooldownModifier / 100));
                            }
                        }
                    }
                }
            }

            foreach (Shot s in poolManager.PoolDictionary["EnemyShots"].objectPool)
            {
                if (s.Enabled)
                {
                    if (s.Intersects(this))
                    {
                        if (s.PerPixelCollision(this))
                        {
                            s.Dies(gameTime);
                            this.TakeDamage(s.damage);
                        }
                    }
                }              
            }
        }

        public void Reset(Game game)
        {
            this.Location = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            gun = new WaveGun(game, shotPoolTag);

            this.cooldownTime = gun.CooldownTime;
            onCooldown = false;

            this.Health = 20;
        }
    }
}
