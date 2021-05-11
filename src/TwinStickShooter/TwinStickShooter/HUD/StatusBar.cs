using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Sprite;

namespace TwinStickShooter.HUD
{
    class StatusBar : DrawableGameComponent
    {
        SpriteBatch sb;

        string textureName;
        Texture2D barTexture;

        Rectangle rectangle;
        public Vector2 position;

        Color color;

        public float statValue;

        public StatusBar(Game game, Vector2 location, string targetContent, Color drawColor): base (game)
        {
            sb = new SpriteBatch(game.GraphicsDevice);
            position = location;
            textureName = targetContent;
            color = drawColor;
            this.Initialize();
        }

        public override void Initialize()
        {
            barTexture = this.Game.Content.Load<Texture2D>(textureName);
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)statValue, 40);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.Draw(barTexture, rectangle, color);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
