using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;
using MonoGameLibrary.Sprite;
using TwinStickShooter.Player;
using TwinStickShooter.ObjectPool;
using TwinStickShooter.Enemies;
using TwinStickShooter.Projectiles;
using TwinStickShooter.Pickups;
using TwinStickShooter.HUD;

namespace TwinStickShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Game1 : Game
    {
        enum GameState { Playing, GameOver}

        GameState gameState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PoolManager pool;

        PlayerWGun player;
        HeadsUpDisplay hud;
        ScoreManager score;

        EnemyManager em;
        ShotManager sm;
        PickUpManager pm;

        public GameConsole console;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameState = GameState.Playing;

            InitializeGame();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //IsMouseVisible = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //TODO need to fix this.
            if (player.Health <= 0)
            {
                gameState = GameState.GameOver;
                player.Enabled = false;
            }            

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                player.Enabled = true;
                reset();
                gameState = GameState.Playing;
                
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (gameState == GameState.GameOver)
            {
                SpriteFont font = this.Content.Load<SpriteFont>("Text");

                string gameOverText = "Game Over";
                string finalStats = "You're score is : " + score.score;
                string playAgainText = "Press 'R' to restart";


                spriteBatch.Begin();
                spriteBatch.DrawString(font, gameOverText, new Vector2(this.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(gameOverText).Length() / 2, this.GraphicsDevice.Viewport.Height / 3), Color.Black);
                spriteBatch.DrawString(font, finalStats, new Vector2(this.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(finalStats).Length() / 2, this.GraphicsDevice.Viewport.Height / 2), Color.Black);
                spriteBatch.DrawString(font, playAgainText, new Vector2(this.GraphicsDevice.Viewport.Width / 2 - font.MeasureString(playAgainText).Length() / 2, this.GraphicsDevice.Viewport.Height / 2 + 30), Color.Black);
                spriteBatch.End();
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }      

        void reset()
        {                      
            foreach (Pool queue in pool.PoolDictionary.Values)
            {
                foreach (DrawableSprite sprite in queue.objectPool)
                {
                    sprite.Enabled = false;
                }
            }

            this.player.Reset(this);

            this.em.Reset();
        }

        void InitializeGame()
        {
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();

            gameState = GameState.Playing;

            score = new ScoreManager();
            this.Services.AddService(typeof(IScoreManager), score);

            console = new GameConsole(this);
            this.Components.Add(console);

            pool = new PoolManager(this);
            this.Components.Add(pool);

            pm = new PickUpManager(this);
            this.Components.Add(pm);

            player = new PlayerWGun(this);
            this.Components.Add(player);
            
            em = new EnemyManager(this, player);
            this.Components.Add(em);

            sm = new ShotManager(this);
            this.Components.Add(sm);

            hud = new HeadsUpDisplay(this, player);
            this.Components.Add(hud);


        }
    }
}
