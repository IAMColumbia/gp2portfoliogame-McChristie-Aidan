﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.Projectiles
{
    public class Shot : DrawableSprite
    {
        float slowdownRate = 0f;

        public Shot(Game game) : base(game)
        {
            this.Speed = 800;
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("shot");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
            this.Speed -= slowdownRate * gameTime.ElapsedGameTime.Milliseconds / 1000; 



            if (this.IsOffScreen())
            {
                this.Dies(gameTime);
            }
   
            base.Update(gameTime);
        }

        public void Dies(GameTime gameTime)
        {
            //this should be done via variable
            this.Location = new Vector2(-50, -50);
            this.Update(gameTime);
            this.Enabled = false;
        }
    } 
}
