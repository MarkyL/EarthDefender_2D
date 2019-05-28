using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EarthDefender
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MainScene mMainScene;
        GameScene mGameScene;
        CreditsScene mCreditsScene;

        SpriteFont font;

        public static Random mRandom;

        public static int ScreenWidth = 1600;
        public static int ScreenHeight = 900;

        public KeyboardState kbState;
        public KeyboardState mPrevKbState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            mRandom = new Random();

            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
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
            this.IsMouseVisible = true;
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
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            font = Content.Load<SpriteFont>("Comics");
            
            Services.AddService(typeof(SpriteFont), font);

            mMainScene = new MainScene(this);
            mGameScene = new GameScene(this);
            mCreditsScene = new CreditsScene(this);

            mMainScene.Show();
            mGameScene.Hide();
            mCreditsScene.Hide();

            Components.Add(mMainScene);
            Components.Add(mGameScene);
            Components.Add(mCreditsScene);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            kbState = Keyboard.GetState();
            // Allows the game to exit
            if (!mPrevKbState.IsKeyDown(Keys.Escape) && kbState.IsKeyDown(Keys.Escape))
            {
                if (mMainScene.Visible)
                    this.Exit();
            }
                
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && mMainScene.getOnePlayerRect().Contains(mousePosition)) 
            {
                mGameScene.setIsMultiplayer(false);
                startGame();
            }

            if (mouseState.LeftButton == ButtonState.Pressed && mMainScene.getTwoPlayerRect().Contains(mousePosition))
            {
                mGameScene.setIsMultiplayer(true);
                startGame();
            }

            if (mGameScene.mExitGame)
            {
                stopGame();
            }

            if (mCreditsScene.mExitGame)
            {
                mCreditsScene.mExitGame = false;
                mCreditsScene.Hide();
                mMainScene.Show();
            }

            if (kbState.IsKeyDown(Keys.OemTilde))
            {
                stopGame();
                mMainScene.Hide();
                mCreditsScene.Show();
            }

            mPrevKbState = kbState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();         
        }

        public void startGame()
        {
            mMainScene.Hide();
            mGameScene.Show();
        }

        public void stopGame()
        {
            mMainScene.Show();
            mGameScene.Hide();
        }
    }
}
