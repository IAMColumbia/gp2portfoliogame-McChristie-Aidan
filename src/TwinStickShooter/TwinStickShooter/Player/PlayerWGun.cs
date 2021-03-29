using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinStickShooter.Weapons;

namespace TwinStickShooter.Player
{
    class PlayerWGun : Player
    {
        Weapon gun;
        bool hasFired;
        public PlayerWGun(Game game) : base(game)
        {
            hasFired = false;
            gun = new ShotGun(game);
        }

        public override void Update(GameTime gameTime)
        {
            CheckForFire();
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
    }
}
