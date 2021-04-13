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
        public Enemy(Game game) : base(game)
        {
            this.Speed = 150;
            this.Direction = new Vector2(0, 1);
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //if (this.IsOffScreen())
            //{
            //    this.Enabled = false;
            //}

            base.Update(gameTime);
        }
    }
}
