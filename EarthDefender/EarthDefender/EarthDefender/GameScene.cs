using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EarthDefender
{
    class GameScene : Scene
    {
        SpriteFont mFont;
        KeyboardState mKbState;
        SpriteBatch mSpriteBatch;
        Game mGame;

        Texture2D mPlayerOneSprite;
        Vector2 mPlayerOneSpritePosition;

        Texture2D mPlayerTwoSprite;
        Vector2 mPlayerTwoSpritePosition;

        Texture2D mBackground;
        Rectangle mMainFrame;

        List<SpriteManager> mAsteroidsList;
        Texture2D mAsteroidSptire;

        List<SpriteManager> mExplosionsList;
        Texture2D mExplosionSprite;

        Boolean mIsMultiplayer, mIsGameOver, mIsGamePaused;
        public Boolean mExitGame;

        String mBaseScoreText;
        int mScoreValue, mLifecount;
        
        Player mPlayer1, mPlayer2;

        private float mAsteroidsSingleTime = 3.5f, mAsteroidsMultiTime = 1f;
        private float mAsteroidsTime;

        private float mTimer;

        protected KeyboardState mCurrentKey, mPreviousKey;

        public GameScene(Game game) : base(game)
        {
            this.mGame = game;
            mSpriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));

            mBackground = mGame.Content.Load<Texture2D>("background2");

            mAsteroidSptire = mGame.Content.Load<Texture2D>("AsteroidAnimation");
            mAsteroidsList = new List<SpriteManager>();

            mExplosionSprite = mGame.Content.Load<Texture2D>("explosion");
            mExplosionsList = new List<SpriteManager>();

            // set main screen size
            mMainFrame = new Rectangle(0, 0, mGame.GraphicsDevice.Viewport.Width, mGame.GraphicsDevice.Viewport.Height);

            mFont = (SpriteFont)game.Services.GetService(typeof(SpriteFont));
        }

        //protected override void Dispose(bool disposing) { dice.Dispose();  base.Dispose(disposing); }
        public override void Update(GameTime gameTime)
        {
            mPreviousKey = mCurrentKey;
            mCurrentKey = Keyboard.GetState();

            mTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!mIsGameOver) {
                if (mCurrentKey.IsKeyDown(Keys.Escape) && mPreviousKey.IsKeyUp(Keys.Escape))
                {
                    if (mIsGamePaused)
                        mExitGame = true;
                    else 
                        mIsGamePaused = true;
                }
                else if (mIsGamePaused && mCurrentKey.IsKeyDown(Keys.Enter) && mPreviousKey.IsKeyUp(Keys.Enter))
                {
                    mIsGamePaused = false;
                }
                else if (!mIsGamePaused)
                {
                    handleAsteroid(gameTime);
                    updateAsteroid(gameTime);
                    updateExplosions(gameTime);
                    mPlayer1.update(gameTime);
                    if (mIsMultiplayer)
                        mPlayer2.update(gameTime);

                    base.Update(gameTime);
                }
            }
            else if (mCurrentKey.IsKeyDown(Keys.Escape) && mPreviousKey.IsKeyUp(Keys.Escape))
            {
                mExitGame = true;
            }
        }

        public void handleAsteroid(GameTime gameTime)
        {

            if (mTimer > mAsteroidsTime)
            {
                mTimer = 0f;
                //Asteroid asteroid = new Asteroid(mAsteroidSptire);
                SpriteManager sprite = new SpriteManager(mGame, mAsteroidSptire, 8, 8);
                sprite.mPosition = new Vector2(Game1.mRandom.Next(0, Game1.ScreenWidth - mAsteroidSptire.Width / 8), -mAsteroidSptire.Height / 8);
                sprite.AddAnimation("Fall1", 1);
                sprite.AddAnimation("Fall2", 2);
                sprite.AddAnimation("Fall3", 3);
                sprite.AddAnimation("Fall4", 4);
                sprite.AddAnimation("Fall5", 5);
                sprite.AddAnimation("Fall6", 6);
                sprite.AddAnimation("Fall7", 7);
                sprite.AddAnimation("Fall8", 8);
                chooseAnimation(sprite);
                sprite.IsLooping = true;
                sprite.FramesPerSecond = 15;
                
                mAsteroidsList.Add(sprite);
            }
        }

        private void chooseAnimation(SpriteManager sprite)
        {
            int animationNumber = Game1.mRandom.Next(1, 9);
            switch (animationNumber)
            {
                case 1:
                    sprite.Animation = "Fall1";
                    break;
                case 2:
                    sprite.Animation = "Fall2";
                    break;
                case 3:
                    sprite.Animation = "Fall3";
                    break;
                case 4:
                    sprite.Animation = "Fall4";
                    break;
                case 5:
                    sprite.Animation = "Fall5";
                    break;
                case 6:
                    sprite.Animation = "Fall6";
                    break;
                case 7:
                    sprite.Animation = "Fall7";
                    break;
                case 8:
                    sprite.Animation = "Fall8";
                    break;
            }
        }

        public void updateAsteroid(GameTime gameTime)
        {
            for (int i = 0; i < mAsteroidsList.Count; i++)
            {
                SpriteManager sprite = mAsteroidsList[i];
                sprite.Update(gameTime);
                Rectangle asteroidRectangle = sprite.getSpriteRectangle();

                if (asteroidRectangle.Bottom >= Game1.ScreenHeight || asteroidRectangle.Intersects(mPlayer1.mSpriteRectangle) || (mIsMultiplayer && asteroidRectangle.Intersects(mPlayer2.mSpriteRectangle)))
                {
                    mLifecount--;
                    mAsteroidsList.RemoveAt(i);
                    i--;
                    if (mLifecount == 0)
                    {
                        mIsGameOver = true;
                        manageGameOver();
                    }
                }
                else 
                {
                    Vector2 hitPosition = mPlayer1.hasBulletHitAsteroid(asteroidRectangle);
                    if (hitPosition != Vector2.Zero)
                    {
                        mAsteroidsList.RemoveAt(i);
                        i--;
                        mScoreValue++;
                        addExplosion(hitPosition);
                    }
                    else if (mIsMultiplayer)
                    {
                        hitPosition = mPlayer2.hasBulletHitAsteroid(asteroidRectangle);
                        if (hitPosition != Vector2.Zero)
                        {
                            mAsteroidsList.RemoveAt(i);
                            i--;
                            mScoreValue++;
                            addExplosion(hitPosition);
                        }
                    }
                }
            }
        }

        public void updateExplosions(GameTime gameTime)
        {
            for (int i = 0; i < mExplosionsList.Count; i++)
            {
                SpriteManager sprite = mExplosionsList[i];
                sprite.Update(gameTime);
                if (sprite.isLastFrame())
                {
                    mExplosionsList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void addExplosion(Vector2 explosionPosition)
        {
            SpriteManager explosionSprite = new SpriteManager(mGame, mExplosionSprite, 6, 1);
            explosionSprite.mPosition = explosionPosition;
            explosionSprite.AddAnimation("explode", 1);
            explosionSprite.Animation = "explode";
            explosionSprite.IsLooping = false;
            explosionSprite.FramesPerSecond = 15;
            explosionSprite.mSpeed = -1f;
            mExplosionsList.Add(explosionSprite);
        }

        public void manageGameOver()
        {
            mBaseScoreText = "YOU LOSE!!!";
        }

        private void _initialize() { }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Draw(mBackground, mMainFrame, Color.White);
            if (mIsGameOver)
            {
                String message = "Game over\nFinal score: " + mScoreValue.ToString() + "\nESC -> Main screen" + "\n~ -> Credits screen";
                mSpriteBatch.DrawString(mFont, message, new Vector2((Game1.ScreenWidth - mFont.MeasureString(message).X) / 2, (Game1.ScreenHeight - mFont.MeasureString(message).Y) / 2), Color.Red);
            }
            else if (mIsGamePaused)
            {
                String message = "Game paused\nESC -> end game \nENTER -> resume";
                mSpriteBatch.DrawString(mFont, message, new Vector2((Game1.ScreenWidth - mFont.MeasureString(message).X) / 2, (Game1.ScreenHeight - mFont.MeasureString(message).Y) / 2), Color.Red);
            }
            else { 
                mSpriteBatch.DrawString(mFont, mBaseScoreText + mScoreValue.ToString(), new Vector2(20, 10), Color.White);
                mSpriteBatch.DrawString(mFont, "Lives: " + mLifecount.ToString(), new Vector2(20, 50), Color.White);

            mPlayer1.draw(mSpriteBatch);
            if (mIsMultiplayer)
                mPlayer2.draw(mSpriteBatch);

            foreach(SpriteManager sprite in mAsteroidsList)
            {
                sprite.Draw(gameTime);
            }

            foreach(SpriteManager explosionSprite in mExplosionsList)
                {
                    explosionSprite.Draw(gameTime);
                }
            }
            mSpriteBatch.DrawString(mFont, "Mark Lurie", new Vector2(mGame.GraphicsDevice.Viewport.Width - 130, Game.GraphicsDevice.Viewport.Height - 30), Color.LawnGreen);
            base.Draw(gameTime);
        }

        public void setIsMultiplayer(Boolean isMulti)
        {
            mIsMultiplayer = isMulti;
            mAsteroidsTime = isMulti ? mAsteroidsMultiTime : mAsteroidsSingleTime;
        }

        public override void Show()
        {
            base.Show();
            initGame();
        }

        public void initGame()
        {
            mIsGameOver = false;
            mIsGamePaused = false;
            mExitGame = false;
            mBaseScoreText = "Score : ";
            mAsteroidsList = new List<SpriteManager>();
            mExplosionsList = new List<SpriteManager>();
            mScoreValue = 0;
            mLifecount = 3;
            initPlayers();    
        }

        public void initPlayers()
        {
            mPlayerOneSprite = mGame.Content.Load<Texture2D>("spaceship");
            Texture2D bulletSprite = mGame.Content.Load<Texture2D>("rocket");
            if (mIsMultiplayer)
            {
                mPlayerOneSpritePosition = new Vector2(mGame.GraphicsDevice.Viewport.Width / 4 - mPlayerOneSprite.Width / 2, mGame.GraphicsDevice.Viewport.Height - mPlayerOneSprite.Height / 2 - 5);

                mPlayerTwoSprite = mGame.Content.Load<Texture2D>("spaceship3");
                mPlayerTwoSpritePosition = new Vector2(3 * mGame.GraphicsDevice.Viewport.Width / 4 - mPlayerTwoSprite.Width / 2, mGame.GraphicsDevice.Viewport.Height - mPlayerTwoSprite.Height / 2 - 5);
                mPlayer2 = new EarthDefender.Player(mPlayerTwoSprite, mPlayerTwoSpritePosition, bulletSprite)
                {
                    mInput = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Fire = Keys.Space,
                    }
                };
            }
            else
            {
                mPlayerOneSpritePosition = new Vector2(mGame.GraphicsDevice.Viewport.Width / 2 - mPlayerOneSprite.Width / 2, mGame.GraphicsDevice.Viewport.Height - mPlayerOneSprite.Height / 2 - 5);
            }

            mPlayer1 = new EarthDefender.Player(mPlayerOneSprite, mPlayerOneSpritePosition, bulletSprite)
            {
                mInput = new Input()
                {
                    Up = Keys.Up,
                    Down = Keys.Down,
                    Left = Keys.Left,
                    Right = Keys.Right,
                    Fire = Keys.RightControl,
                }
            };
        }

    }
}
