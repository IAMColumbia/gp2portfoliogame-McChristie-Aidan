using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.GameComponents.Player;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Sprite;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinStickShooter.Player
{
    class Player : DrawableSprite
    {
        float playerSpeed = 225;

        public IPlayerController Controller { get; protected set; }

        Vector2 distance;


        public Player(Game game) : base(game)
        {
            SetupIPlayerController(game);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteTexture = this.Game.Content.Load<Texture2D>("pacManSingle");
            this.Origin = new Vector2(this.SpriteTexture.Width / 2, this.SpriteTexture.Height / 2);
            this.Location = new Microsoft.Xna.Framework.Vector2(100, 100);
            this.Speed = playerSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            UpdatePlayerMove(gameTime, time);
            UpdateKeepOnScreen();

            base.Update(gameTime);
        }

        protected virtual void SetupIPlayerController(Game game)
        {
            this.Controller = new PlayerController(game);
        }

        protected virtual void UpdatePlayerMove(GameTime gameTime, float time)
        {
            this.Controller.Update(gameTime);

            MouseState mouse = Mouse.GetState();

            distance.X = mouse.X - this.Location.X;
            distance.Y = mouse.Y - this.Location.Y;

            //only started working onced I multiplied it by 55. further research needed
            this.Rotate = (float)Math.Atan2(distance.Y, distance.X) * 55;

            this.Location += ((this.Controller.Direction * (time / 1000)) * Speed);
        }

        protected void UpdateKeepOnScreen()
        {
            //Keep PacMan On Screen
            if (this.Location.X > Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2))
            {
                this.Location.X = Game.GraphicsDevice.Viewport.Width - (this.spriteTexture.Width / 2);
            }
            if (this.Location.X < (this.spriteTexture.Width / 2))
                this.Location.X = (this.spriteTexture.Width / 2);

            if (this.Location.Y > Game.GraphicsDevice.Viewport.Height - (this.spriteTexture.Height / 2))
                this.Location.Y = Game.GraphicsDevice.Viewport.Height - (this.spriteTexture.Height / 2);

            if (this.Location.Y < (this.spriteTexture.Height / 2))
                this.Location.Y = (this.spriteTexture.Height / 2);
        }
    }
}
