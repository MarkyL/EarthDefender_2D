using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EarthDefender
{
    class CreditsScene : Scene
    {
        Game mGame;

        SpriteFont mFont;
        SpriteBatch mSpriteBatch;
        Texture2D mBackground;
        Rectangle mMainFrame;

        public Boolean mExitGame;

        protected KeyboardState mCurrentKey, mPreviousKey;

        String creditsMessage = "~~~~~ Credits ~~~~~";
        Vector2 creditsPosition;

        String developmentMessage = "Development : Mark Lurie";
        Vector2 developmentPosition;

        String assetCredit = "Assets : www.opengameart.org";
        Vector2 assetPosition;

        String gameTitle = "Earth Defender";
        Vector2 gameTitlePosition;

        List<SpriteManager> mExplosionsList;
        Texture2D mExplosionSprite;

        public CreditsScene(Game game) : base(game)
        {
            this.mGame = game;
            mBackground = mGame.Content.Load<Texture2D>("background2");

            // set main screen size
            mMainFrame = new Rectangle(0, 0, mGame.GraphicsDevice.Viewport.Width, mGame.GraphicsDevice.Viewport.Height);

            mFont = (SpriteFont)game.Services.GetService(typeof(SpriteFont));
            mSpriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));

            initCredits();
        }

        public void addExplosionAnimations()
        {
            //float xCredits = mFont.MeasureString(creditsMessage).;
            for (int i = 0; i < 5; i++)
            {
                addExplositionAt(new Vector2(20 + i * 80, gameTitlePosition.Y -20));
            }

            for (int i = 0; i < 5; i++)
            {
                addExplositionAt(new Vector2(mMainFrame.Width - 10 - i * 80 - mExplosionSprite.Width / 2, gameTitlePosition.Y - 20));
            }
        }

        public void addExplositionAt(Vector2 vector)
        {
            SpriteManager explosionSprite = new SpriteManager(mGame, mExplosionSprite, 6, 1);
            explosionSprite.mPosition = new Vector2(vector.X, vector.Y);
            explosionSprite.AddAnimation("explode", 1);
            explosionSprite.Animation = "explode";
            explosionSprite.IsLooping = true;
            explosionSprite.Scale = 2;
            explosionSprite.mSpeed = -0.4f;
            explosionSprite.FramesPerSecond = 5;

            mExplosionsList.Add(explosionSprite);
        }

        public override void Update(GameTime gameTime)
        {
            mPreviousKey = mCurrentKey;
            mCurrentKey = Keyboard.GetState();

            if (mCurrentKey.IsKeyDown(Keys.Escape) && mPreviousKey.IsKeyUp(Keys.Escape))
            {
                mExitGame = true;
            }
            else
            {
                base.Update(gameTime);
                creditsPosition.Y -= 0.75f;
                developmentPosition.Y -= 0.75f;
                assetPosition.Y -= 0.75f;

                if (assetPosition.Y < 0)
                {
                    if (gameTitlePosition.Y > mMainFrame.Height / 2)
                    {
                        gameTitlePosition.Y -= 0.4f;
                        for (int i = 0; i < mExplosionsList.Count; i++)
                        {
                            SpriteManager sprite = mExplosionsList[i];
                            sprite.Update(gameTime);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < mExplosionsList.Count; i++)
                        {
                            SpriteManager sprite = mExplosionsList[i];
                            sprite.mSpeed = 0f;
                            sprite.Update(gameTime);
                        }
                    }
                    

                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            
            mSpriteBatch.Draw(mBackground, mMainFrame, Color.White);

            mSpriteBatch.DrawString(mFont, creditsMessage, creditsPosition, Color.White);
            mSpriteBatch.DrawString(mFont, developmentMessage, developmentPosition, Color.White);
            mSpriteBatch.DrawString(mFont, assetCredit, assetPosition, Color.White);
           
            foreach (SpriteManager explosionSprite in mExplosionsList)
            {
                explosionSprite.Draw(gameTime);
            }           
            mSpriteBatch.DrawString(mFont, gameTitle, gameTitlePosition, Color.White);
            base.Draw(gameTime);
        }

        public override void Show()
        {
            base.Show();
            initCredits();
        }

        public void initCredits()
        {
            creditsPosition = new Vector2(mMainFrame.Center.X, mMainFrame.Bottom + 80);
            Vector2 vect = mFont.MeasureString(creditsMessage);
            creditsPosition.X -= vect.X / 2;

            developmentPosition = new Vector2(mMainFrame.Center.X, mMainFrame.Bottom + 120);
            Vector2 vect2 = mFont.MeasureString(developmentMessage);
            developmentPosition.X -= vect2.X / 2;

            assetPosition = new Vector2(mMainFrame.Center.X, mMainFrame.Bottom + 160);
            Vector2 vect3 = mFont.MeasureString(assetCredit);
            assetPosition.X -= vect3.X / 2;

            gameTitlePosition = new Vector2(mMainFrame.Center.X, mMainFrame.Bottom + 30);
            Vector2 vect4 = mFont.MeasureString(gameTitle);
            gameTitlePosition.X -= vect4.X / 2;

            mExplosionSprite = mGame.Content.Load<Texture2D>("explosion");
            mExplosionsList = new List<SpriteManager>();

            mExitGame = false;

            addExplosionAnimations();
        }
    }
}
