using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwinStickShooter.Enemies
{
    class BasicEnemy : Enemy
    {
        float normalSpeed = 200;
        float normalHealth = 10;
        float normalDamage = 2;

        public BasicEnemy(Game game) : base (game)
        {
            this.Speed = normalSpeed;
            this.Health = normalHealth;
            this.damage = normalDamage;
            this.type = "Basic";
            this.LoadContent();
        }

        public override void Initialize()
        {
            this.LoadContent();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("RedGhost");
            base.LoadContent();
        }
    }
}
