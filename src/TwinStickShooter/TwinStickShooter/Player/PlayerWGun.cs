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
        public Weapon gun;
        public bool hasFired;
        ShotPool playerBullets;
        int playerBulletsSize = 75;

        GameConsole console;

        public PlayerWGun(Game game) : base(game)
        {
            hasFired = false;
            playerBullets = new ShotPool(game, playerBulletsSize);
            game.Components.Add(playerBullets);
            gun = new ShotGun(game, playerBullets);
            console = (GameConsole)this.Game.Services.GetService<IGameConsole>();
        }

        public override void Update(GameTime gameTime)
        {
            CheckForFire();
            GunSwap();
            console.Log("player current gun : ", this.gun.WeaponName);
            base.Update(gameTime);
        }

        private void CheckForFire()
        {
            MouseState mouseState = Mouse.GetState();

            Vector2 target = mouseState.Position.ToVector2() - this.Location;
            target.Normalize();

            if (mouseState.LeftButton == ButtonState.Pressed && hasFired == false)
            {
                hasFired = true;
                gun.Fire(this.Location, target);
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                hasFired = false;
            }
        }

        void GunSwap()
        {
            if(Controller.Input.KeyboardState.HasReleasedKey(Keys.D1))
            {
                this.gun = new HandGun(this.Game, playerBullets);
            }

            if (Controller.Input.KeyboardState.HasReleasedKey(Keys.D2))
            {
                this.gun = new ShotGun(this.Game, playerBullets);
            }
        }
    }
}
