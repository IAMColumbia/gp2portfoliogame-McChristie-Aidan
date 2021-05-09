using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TwinStickShooter.HUD
{
    class HeadsUpDisplay : DrawableGameComponent
    {
        StatusBar healthBar;
        StatusBar cooldownBar;

        HudText waveNumber;

        Player.PlayerWGun targetPlayer;
        List<DrawableGameComponent> activeHudElements;

        ScoreManager scoreManager;

        public HeadsUpDisplay(Game game, TwinStickShooter.Player.PlayerWGun player) : base (game)
        {
            scoreManager = (ScoreManager)this.Game.Services.GetService<IScoreManager>();

            activeHudElements = new List<DrawableGameComponent>();

            //I need to find better art assests
            healthBar = new StatusBar(game, new Vector2(50, 20), "block", Color.Red);
            activeHudElements.Add(healthBar);

            cooldownBar = new StatusBar(game, new Vector2(GraphicsDevice.Viewport.Width /2, GraphicsDevice.Viewport.Height - 80), "block", Color.White * 0.5f);
            activeHudElements.Add(cooldownBar);

            waveNumber = new HudText(game, new Vector2(GraphicsDevice.Viewport.Width - 300, 40), Color.Black);
            activeHudElements.Add(waveNumber);

            targetPlayer = player;
        }

        public override void Update(GameTime gameTime)
        {
            healthBar.statValue = targetPlayer.Health * 10;

            cooldownBar.statValue = targetPlayer.CoolDown;
            cooldownBar.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - cooldownBar.statValue / 2, GraphicsDevice.Viewport.Height - 80);

            waveNumber.text = "Wave Number: " + scoreManager.waveNumber.ToString();

            foreach (DrawableGameComponent item in activeHudElements)
            {
                item.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            

            foreach (DrawableGameComponent item in activeHudElements)
            {
                item.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        
    }
}
