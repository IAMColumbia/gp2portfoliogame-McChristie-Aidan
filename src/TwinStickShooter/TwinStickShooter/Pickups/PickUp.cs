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

        public enum PickUpType { AttackSpeed, Health, Weapon }

        public PickUpType type;

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
            ChangeDrawColor();
            base.Update(gameTime);
        }

        void ChangeDrawColor()
        {
            switch (type)
            {
                case PickUpType.AttackSpeed:
                    this.DrawColor = Color.Red;
                    break;
                case PickUpType.Health:
                    this.DrawColor = Color.Green;
                    break;
                case PickUpType.Weapon:
                    this.DrawColor = Color.Blue;
                    break;
                default:
                    break;
            }
        }
    }
}
