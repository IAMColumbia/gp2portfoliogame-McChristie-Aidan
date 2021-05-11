using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwinStickShooter.Enemies
{
    class TankEnemy : Enemy
    {
        float tankSpeed = 250;
        float tankHealth = 20;
        float tankDamage = 4;

        public TankEnemy(Game game) : base(game)
        {
            this.Speed = tankSpeed;
            this.Health = tankHealth;
            this.damage = tankDamage;
            this.type = "Tank";
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("TealGhost");
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            base.LoadContent();
        }

        public override void Initialize()
        {
            this.LoadContent();
            base.Initialize();
        }
    }
}
