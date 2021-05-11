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
        //the entire hud system could probably be architectured way better but I felt rushed
        StatusBar healthBar;
        StatusBar cooldownBar;

        HudText waveNumber;
        HudText scoreNumber;
        HudText gunType;

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

            scoreNumber = new HudText(game, new Vector2(GraphicsDevice.Viewport.Width - 300, 65), Color.Black);
            activeHudElements.Add(scoreNumber);

            gunType = new HudText(game, new Vector2(GraphicsDevice.Viewport.Width - 300, 120), Color.Black);
            activeHudElements.Add(gunType);

            targetPlayer = player;
        }

        public override void Update(GameTime gameTime)
        {
            healthBar.statValue = targetPlayer.Health * 10;

            cooldownBar.statValue = targetPlayer.CoolDown;
            cooldownBar.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - cooldownBar.statValue / 2, GraphicsDevice.Viewport.Height - 80);

            waveNumber.text = "Wave Number: " + scoreManager.waveNumber.ToString();

            scoreNumber.text = "Score: " + scoreManager.score;

            gunType.text = "Weapon: " + targetPlayer.gun.WeaponName;

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
