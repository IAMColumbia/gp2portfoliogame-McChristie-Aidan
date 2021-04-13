using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;


namespace TwinStickShooter.Pickups
{
    class PickUp : DrawableSprite
    {
        public float pickUpValue;

        public PickUp(Game game) : base(game)
        {
            this.pickUpValue = 1f;
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("shot");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.DrawColor = Color.Red;
            base.Update(gameTime);
        }
    }
}
