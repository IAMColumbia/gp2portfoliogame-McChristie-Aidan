using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinStickShooter.Weapons;
using TwinStickShooter.ObjectPool;
using TwinStickShooter.Projectiles;
using MonoGameLibrary.Util;
using MonoGameLibrary.Sprite;

namespace TwinStickShooter.Player
{
    class PlayerWGun : Player
    {
        bool onCooldown;
        float cooldownTime = 1000;
        float playerCooldownModifier = 0;
        int playerBulletsSize = 75;
        string shotPoolTag = "Shots";
        //technical debt the player shouldn't use this
        int shotPoolSize = 100;

        public Weapon gun;

        GameConsole console;
        PoolManager poolManager;

        public PlayerWGun(Game game) : base(game)
        {           
            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            poolManager.InstantiatePool(PoolManager.ClassType.Shot, game, shotPoolSize, shotPoolTag);

            gun = new WaveGun(game, shotPoolTag);

            this.cooldownTime = gun.CooldownTime;
            onCooldown = false;

            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
        }

        public override void Update(GameTime gameTime)
        {
            //has the player fired
            CheckForFire(gameTime);
            //have we swapped guns
            GunSwap();
            //have we hit something
            CheckCollision();
            //useful info
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
                gun.RotationFire(this.Location, this.Rotate);
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

        //checks for when the player hits something
        void CheckCollision()
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
                            playerCooldownModifier += pickUp.pickUpValue;
                            if (!onCooldown)
                            {
                                this.cooldownTime = gun.CooldownTime - (gun.CooldownTime * (playerCooldownModifier / 100));
                            }
                        }
                    }
                }
            }
        }
    }
}
