using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.HUD
{
    class HudText : DrawableGameComponent
    {
        SpriteBatch sb;
        SpriteEffects se;

        SpriteFont font;

        public Vector2 position;

        Color color;

        public string text;

        public HudText(Game game, Vector2 location, Color drawColor) : base(game)
        {
            sb = new SpriteBatch(game.GraphicsDevice);
            se = new SpriteEffects();
            position = location;
            color = drawColor;
            this.Initialize();
        }

        public override void Initialize()
        {
            font = this.Game.Content.Load<SpriteFont>("Text");
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.DrawString(font, text, position, color, 0, new Vector2(text.Length), 2, se, 0);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
