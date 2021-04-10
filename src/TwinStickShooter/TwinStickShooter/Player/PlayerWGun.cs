using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinStickShooter.Weapons;
using TwinStickShooter.ObjectPool;
using MonoGameLibrary.Util;

namespace TwinStickShooter.Player
{
    class PlayerWGun : Player
    {
        bool onCooldown;
        float cooldownTime = 1000;
        int playerBulletsSize = 75;
        string bulletPoolTag = "Shots";



        public Weapon gun;

        GameConsole console;
        PoolManager poolManager;

        public PlayerWGun(Game game) : base(game)
        {

            poolManager = (PoolManager)this.Game.Services.GetService<IPoolManager>();
            poolManager.InitializeShotPool(bulletPoolTag, playerBulletsSize);

            gun = new WaveGun(game, poolManager, bulletPoolTag);

            this.cooldownTime = gun.CooldownTime;
            onCooldown = false;

            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
        }

        public override void Update(GameTime gameTime)
        {
            CheckForFire(gameTime);
            GunSwap();
            console.Log("player current gun : ", this.gun.WeaponName);
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

                if (cooldownTime <= 0f)
                {
                    onCooldown = false;
                    this.cooldownTime = gun.CooldownTime;
                }
            }
        }

        void GunSwap()
        {
            if(Controller.Input.KeyboardState.HasReleasedKey(Keys.D1))
            {
                this.gun = new HandGun(this.Game, poolManager, bulletPoolTag);
                this.cooldownTime = gun.CooldownTime;
            }

            if (Controller.Input.KeyboardState.HasReleasedKey(Keys.D2))
            {
                this.gun = new WaveGun(this.Game, poolManager, bulletPoolTag);
                this.cooldownTime = gun.CooldownTime;
            }
            if (Controller.Input.KeyboardState.HasReleasedKey(Keys.D3))
            {
                this.gun = new AssultRifle(this.Game, poolManager, bulletPoolTag);
                this.cooldownTime = gun.CooldownTime;
            }
        }
    }
}
